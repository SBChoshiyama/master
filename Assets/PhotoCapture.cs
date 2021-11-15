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

    // PhotoCapture �I�u�W�F�N�g
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
                Debug.Log("2�_�Ԃ̋�������J�n");
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
    /// ���̓C�x���g����
    /// </summary>
    /// <remarks>GameObject��InputActionHandler�C�x���g��AirTap���\�b�h��ݒ肵�Ă��������B</remarks>
    public void AirTap()
    {
        Resolution cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        // PhotoCapture �I�u�W�F�N�g���쐬���܂�
        // �I�u�W�F�N�g��\�����������ꍇ�́A�擪��bool������true�ɕύX���Ă�������
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(true, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            UnityEngine.Windows.WebCam.CameraParameters cameraParameters = new UnityEngine.Windows.WebCam.CameraParameters();

            // �I�u�W�F�N�g��\�����������ꍇ�͕s�����x��ύX���Ă�������(0.9f���炢���璲��)
            cameraParameters.hologramOpacity = 0.9f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

            string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);

            // �J�������A�N�e�B�x�[�g���܂�
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
            {
                // �ʐ^���B��܂�
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToMemory);
            });
        });
    }

    void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // �J�������A�N�e�B�u�ɂ��܂�
        Debug.Log("Saved Photo to disk!");
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // PhotoCapture �I�u�W�F�N�g��������܂��B
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

}
