using HKT;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.WebCam;
using static SavingToolSelector;

public class VoiceCommand : MonoBehaviour
{
    /// <summary>
    /// 計測ツールセレクタGameObject
    /// </summary>
    private GameObject MeasuringToolSelectorObj;

    /// <summary>
    /// 計測ツールセレクタGameObject
    /// </summary>
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// 茎モード選択用GameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// 茎モード選択用スクリプトObject
    /// </summary>
    private StemModeSelector stemModeSelector;

    /// <summary>
    /// ハンドモニターGameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// ハンドモニターObject
    /// </summary>
    private HandMonitor HandMonitor;

    /// <summary>
    /// 計測コントローラGameObject
    /// </summary>
    private GameObject MeasureControlObj;

    /// <summary>
    /// 計測コントローラObject
    /// </summary>
    private RulerLineManager MeasureControl;

    /// <summary>
    /// 確定トリガー選択GameObject
    /// </summary>
    private GameObject SavingToolSelectorObj;

    /// <summary>
    /// 確定トリガー選択スクリプトObject
    /// </summary>
    private SavingToolSelector savingToolSelector;

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


    [SerializeField]
    private TextMesh DistanceText = default;

    private bool IsOneSelectDiameter;
    private float InnerDistance;
    private float LongDis;
    private float MinDis;
    private bool VoiceTriggerOn = false;

    private float locallength;
    private float localdiameter;

    // Start is called before the first frame update
    private void Start()
    {
        // ハンドモニターオブジェクト
        HandMonitorObj = GameObject.Find("HandMonitor");
        HandMonitor = HandMonitorObj.GetComponent<HandMonitor>();

        // 計測ツールセレクタオブジェクト
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();

        // 茎モード選択用オブジェクト
        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        // 計測コントローラオブジェクト
        MeasureControlObj = GameObject.Find("RulerLineManager");
        MeasureControl = StemModeSelectorObj.GetComponent<RulerLineManager>();

        // Saveモードコントローラオブジェクト
        SavingToolSelectorObj = GameObject.Find("SavingToolSelector");
        savingToolSelector = SavingToolSelectorObj.GetComponent<SavingToolSelector>();

        locallength = 0;
        localdiameter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Saveコマンド実施処理
    /// </summary>
    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;

        // 画像撮影モードの場合
        if (savingToolSelector.savingtoolSel == SavingTools.PhotoCapture)
        {
            // 撮影イベント実施
            PhotoCaptureEvent();
        }
        else
        {

            if (HandMonitor.isHandTracking())
            {
                if (stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Diameter)
                {
                    // 計算処理実施
                    CheckStemDiameter(dis);
                }
                else
                {
                    // メッセージ表示処理
                    ShowDistanceText(dis);
                }
            }
            else
            {
                DistanceText.text = "エラー:手の検出失敗";
            }
        }
    }

    /// <summary>
    /// Clearコマンド実施処理
    /// </summary>
    public void ClearCommandEvent()
    {
        Debug.Log("ClearEvent");

        // 初期化処理実施
        IsOneSelectDiameter = false;
        DistanceText.text = "No Distance";
    }

    /// <summary>
    /// 計測結果表示処理
    /// </summary>
    /// <param name="dis">表示する計測結果</param>
    private void ShowDistanceText(float dis)
    {
        if(stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Length)
        {
            Debug.Log($"茎長の長さ = {dis}cm");
            DistanceText.text = "茎長の長さ = " + dis.ToString("0.0") + " cm";
            // 茎長の設定
            locallength = dis;
            VoiceTriggerOn = true;
        }
        else
        {
            Debug.Log($"茎径の円周 = {dis}cm");
            DistanceText.text = "茎径の円周 = " + dis.ToString("0.0") + " cm";
            // 茎径の設定完了
            localdiameter = dis;
            VoiceTriggerOn = true;
        }
    }

    /// <summary>
    /// 楕円型茎の計測処理
    /// </summary>
    /// <param name="dis">計測した辺の長さ</param>
    private void CheckStemDiameter(float dis)
    {
        if (!IsOneSelectDiameter)
        {
            InnerDistance = dis;
            Debug.Log($"1回目の距離 = {dis}cm");
            DistanceText.text = "1回目の距離" + dis.ToString("0.0") + " cm";
            IsOneSelectDiameter = true;
        }
        else
        {
            Debug.Log($"2回目の距離 = {dis}cm");
            DistanceText.text = "2回目の距離" + dis.ToString("0.0") + " cm";

            if (InnerDistance > dis)
            {
                LongDis = InnerDistance;
                MinDis = dis;
            }
            else
            {
                LongDis = dis;
                MinDis = InnerDistance;
            }

            // 直径で測定しているため、半径に置き換え
            LongDis = LongDis / 2;
            MinDis = MinDis / 2;

            // 楕円の円周の近似計算式に代入
            var a = (float)(Math.PI * (LongDis + MinDis));
            var b = (LongDis - MinDis) / (LongDis + MinDis);
            var c = 3 * b * b;
            var d = (float)(10 + Math.Sqrt(4 - c));

            var total = a * (1 + (c / d));

            // メッセージ表示処理
            ShowDistanceText(total);
            IsOneSelectDiameter = false;
        }
    }

    /// <summary>
    /// 撮影イベント処理
    /// </summary>
    /// <remarks>GameObjectのInputActionHandlerイベントにAirTapメソッドを設定してください。</remarks>
    public void PhotoCaptureEvent()
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
        Debug.Log("URLの設定");

        UnityWebRequest webRequest = UnityWebRequest.Post(URL, form);
        Debug.Log("接続完了");

        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError)
        {
            // レスポンスコードを見て処理
            Debug.Log($"[Error]Response Code : {webRequest.responseCode}");
        }
        else if (webRequest.isNetworkError)
        {
            // エラーメッセージを見て処理
            Debug.Log($"[Error]Message : {webRequest.error}");
        }

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
            var diameter = float.Parse(webRequest.downloadHandler.text);
            ShowDistanceText(diameter);
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


    /// <summary>
    /// 音声トリガー検知
    /// </summary>
    public bool IsVoiceTriggerOn()
    {
        bool ret = VoiceTriggerOn;
        VoiceTriggerOn = false;
        return ret;
    }

    /// <summary>
    /// 距離情報クリア
    /// </summary>
    public void clearDistance()
    {
        locallength = 0;
        localdiameter = 0;
    }

    /// <summary>
    /// 茎長取得
    /// </summary>
    public float getStemLength()
    {
        return locallength;
    }

    /// <summary>
    /// 茎径取得
    /// </summary>
    public float getStemDiameter()
    {
        return localdiameter;
    }

}