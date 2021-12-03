using HKT;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using UnityEngine.Networking;

public class PhotoCapture : MonoBehaviour
{
    GameObject MeasuringToolSelectorObj;
    MeasuringToolSelector measuringToolSelector;

    [SerializeField]
    private TextMesh DistanceText = default;

    /// <summary>
    /// タイマー発火中フラグ
    /// </summary>
    bool IsTimer;

    /// <summary>
    /// タイマー監視時間
    /// </summary>
    public float totalTime;

    /// <summary>
    /// カウントダウンタイマー
    /// </summary>
    int seconds;

    /// <summary>
    /// 保存用カウントダウンタイマー
    /// </summary>
    float rocal;

    /// <summary>
    /// PhotoCapture オブジェクト
    /// </summary>
    UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// 画像変換用テクスチャオブジェクト
    /// </summary>
    Texture2D targetTexture = null;

    /// <summary>
    /// 接続先URL
    /// </summary>
    /// <remarks>最終的な外部API化するときに正式なURLを設定してください。</remarks>
    private const string URL = "http://xxx.xxx.x.xxx:xxxx/";

    // Start is called before the first frame update
    void Start()
    {
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();
        IsTimer = false;
        rocal = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimer)
        {
            rocal -= Time.deltaTime;
            if (rocal > 0)
            {
                seconds = (int)rocal;
                DistanceText.text = seconds.ToString();
            }
            else
            {
                Debug.Log("画像解析によるの距離測定開始");
                DistanceText.text = "0";
                IsTimer = false;
                rocal = totalTime;
                AirTap();
            }
        }

    }

    /// <summary>
    /// ボタン押下イベント
    /// </summary>
    public void ButtonClickEvent()
    {
        IsTimer = true;
    }

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    /// <remarks>GameObjectのInputActionHandlerイベントにAirTapメソッドを設定してください。</remarks>
    public void AirTap()
    {
        Resolution cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // PhotoCapture オブジェクトを作成します
        // オブジェクトを表示させたい場合は、先頭のbool引数をtrueに変更してください
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            UnityEngine.Windows.WebCam.CameraParameters cameraParameters = new UnityEngine.Windows.WebCam.CameraParameters();

            // オブジェクトを表示させたい場合は不透明度を変更してください(0.9fくらいから調整)
            cameraParameters.hologramOpacity = 0.9f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

            string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);

            // カメラをアクティベートします
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
            {
                // 写真を撮ります
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    /// <summary>
    /// 写真保存処理
    /// </summary>
    /// <param name="result">処理結果</param>
    /// <param name="photoCaptureFrame">画像データ</param>
    void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // ターゲットテクスチャに RAW 画像データをコピーします
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        byte[] bodyData = targetTexture.EncodeToJPG();

        StartCoroutine(OnPostSend(bodyData));

        // カメラを非アクティブにします
        Debug.Log("Saved Photo to disk!");
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bodyData">画像バイナリデータ</param>
    /// <returns>処理結果</returns>
    private IEnumerator OnPostSend(byte[] bodyData)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bodyData, "image.png", "image/png");

        UnityWebRequest webRequest = UnityWebRequest.Post(URL, form);

        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            //通信失敗
            Debug.Log(webRequest.error);
            DistanceText.text = "処理に失敗しました";
        }
        else
        {
            //通信成功
            Debug.Log("post" + " : " + webRequest.downloadHandler.text);
            DistanceText.text = webRequest.downloadHandler.text;
        }
    }

    /// <summary>
    /// 後処理
    /// </summary>
    /// <param name="result">処理結果</param>
    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // PhotoCapture オブジェクトを解放します。
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
