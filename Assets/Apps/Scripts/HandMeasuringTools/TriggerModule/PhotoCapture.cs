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
    /// �^�C�}�[���Β��t���O
    /// </summary>
    bool IsTimer;

    /// <summary>
    /// �^�C�}�[�Ď�����
    /// </summary>
    public float totalTime;

    /// <summary>
    /// �J�E���g�_�E���^�C�}�[
    /// </summary>
    int seconds;

    /// <summary>
    /// �ۑ��p�J�E���g�_�E���^�C�}�[
    /// </summary>
    float rocal;

    /// <summary>
    /// PhotoCapture �I�u�W�F�N�g
    /// </summary>
    UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// �摜�ϊ��p�e�N�X�`���I�u�W�F�N�g
    /// </summary>
    Texture2D targetTexture = null;

    /// <summary>
    /// �ڑ���URL
    /// </summary>
    /// <remarks>�ŏI�I�ȊO��API������Ƃ��ɐ�����URL��ݒ肵�Ă��������B</remarks>
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
                Debug.Log("�摜��͂ɂ��̋�������J�n");
                DistanceText.text = "0";
                IsTimer = false;
                rocal = totalTime;
                AirTap();
            }
        }

    }

    /// <summary>
    /// �{�^�������C�x���g
    /// </summary>
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
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // PhotoCapture �I�u�W�F�N�g���쐬���܂�
        // �I�u�W�F�N�g��\�����������ꍇ�́A�擪��bool������true�ɕύX���Ă�������
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
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
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    /// <summary>
    /// �ʐ^�ۑ�����
    /// </summary>
    /// <param name="result">��������</param>
    /// <param name="photoCaptureFrame">�摜�f�[�^</param>
    void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // �^�[�Q�b�g�e�N�X�`���� RAW �摜�f�[�^���R�s�[���܂�
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);
        byte[] bodyData = targetTexture.EncodeToJPG();

        StartCoroutine(OnPostSend(bodyData));

        // �J�������A�N�e�B�u�ɂ��܂�
        Debug.Log("Saved Photo to disk!");
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bodyData">�摜�o�C�i���f�[�^</param>
    /// <returns>��������</returns>
    private IEnumerator OnPostSend(byte[] bodyData)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bodyData, "image.png", "image/png");

        UnityWebRequest webRequest = UnityWebRequest.Post(URL, form);

        //URL�ɐڑ����Č��ʂ��߂��Ă���܂őҋ@
        yield return webRequest.SendWebRequest();

        //�G���[���o�Ă��Ȃ����`�F�b�N
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            //�ʐM���s
            Debug.Log(webRequest.error);
            DistanceText.text = "�����Ɏ��s���܂���";
        }
        else
        {
            //�ʐM����
            Debug.Log("post" + " : " + webRequest.downloadHandler.text);
            DistanceText.text = webRequest.downloadHandler.text;
        }
    }

    /// <summary>
    /// �㏈��
    /// </summary>
    /// <param name="result">��������</param>
    void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // PhotoCapture �I�u�W�F�N�g��������܂��B
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}
