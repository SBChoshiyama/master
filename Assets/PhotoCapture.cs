using HKT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class PhotoCapture : MonoBehaviour
{
    GameObject MeasuringToolSelectorObj;
    MeasuringToolSelector measuringToolSelector;

    [SerializeField]
    private TextMesh DistanceText = default;

    bool IsTimer;
    public float totalTime;
    int seconds;
    float rocal;

    // PhotoCapture オブジェクト
    UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject = null;

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
                Debug.Log("2点間の距離測定開始");
                DistanceText.text = "0";
                IsTimer = false;
                rocal = totalTime;
                AirTap();
            }
        }

    }

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

        // PhotoCapture オブジェクトを作成します
        // オブジェクトを表示させたい場合は、先頭のbool引数をtrueに変更してください
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(true, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
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
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // カメラを非アクティブにします
        Debug.Log("Saved Photo to disk!");
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // PhotoCapture オブジェクトを解放します。
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

}
