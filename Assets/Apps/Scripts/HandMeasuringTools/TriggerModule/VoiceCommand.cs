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
    /// �v���c�[���Z���N�^GameObject
    /// </summary>
    private GameObject MeasuringToolSelectorObj;

    /// <summary>
    /// �v���c�[���Z���N�^GameObject
    /// </summary>
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// �s���[�h�I��pGameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// �s���[�h�I��p�X�N���v�gObject
    /// </summary>
    private StemModeSelector stemModeSelector;

    /// <summary>
    /// �n���h���j�^�[GameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// �n���h���j�^�[Object
    /// </summary>
    private HandMonitor HandMonitor;

    /// <summary>
    /// �v���R���g���[��GameObject
    /// </summary>
    private GameObject MeasureControlObj;

    /// <summary>
    /// �v���R���g���[��Object
    /// </summary>
    private RulerLineManager MeasureControl;

    /// <summary>
    /// �m��g���K�[�I��GameObject
    /// </summary>
    private GameObject SavingToolSelectorObj;

    /// <summary>
    /// �m��g���K�[�I���X�N���v�gObject
    /// </summary>
    private SavingToolSelector savingToolSelector;

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
        // �n���h���j�^�[�I�u�W�F�N�g
        HandMonitorObj = GameObject.Find("HandMonitor");
        HandMonitor = HandMonitorObj.GetComponent<HandMonitor>();

        // �v���c�[���Z���N�^�I�u�W�F�N�g
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();

        // �s���[�h�I��p�I�u�W�F�N�g
        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        // �v���R���g���[���I�u�W�F�N�g
        MeasureControlObj = GameObject.Find("RulerLineManager");
        MeasureControl = StemModeSelectorObj.GetComponent<RulerLineManager>();

        // Save���[�h�R���g���[���I�u�W�F�N�g
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
    /// Save�R�}���h���{����
    /// </summary>
    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;

        // �摜�B�e���[�h�̏ꍇ
        if (savingToolSelector.savingtoolSel == SavingTools.PhotoCapture)
        {
            // �B�e�C�x���g���{
            PhotoCaptureEvent();
        }
        else
        {

            if (HandMonitor.isHandTracking())
            {
                if (stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Diameter)
                {
                    // �v�Z�������{
                    CheckStemDiameter(dis);
                }
                else
                {
                    // ���b�Z�[�W�\������
                    ShowDistanceText(dis);
                }
            }
            else
            {
                DistanceText.text = "�G���[:��̌��o���s";
            }
        }
    }

    /// <summary>
    /// Clear�R�}���h���{����
    /// </summary>
    public void ClearCommandEvent()
    {
        Debug.Log("ClearEvent");

        // �������������{
        IsOneSelectDiameter = false;
        DistanceText.text = "No Distance";
    }

    /// <summary>
    /// �v�����ʕ\������
    /// </summary>
    /// <param name="dis">�\������v������</param>
    private void ShowDistanceText(float dis)
    {
        if(stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Length)
        {
            Debug.Log($"�s���̒��� = {dis}cm");
            DistanceText.text = "�s���̒��� = " + dis.ToString("0.0") + " cm";
            // �s���̐ݒ�
            locallength = dis;
            VoiceTriggerOn = true;
        }
        else
        {
            Debug.Log($"�s�a�̉~�� = {dis}cm");
            DistanceText.text = "�s�a�̉~�� = " + dis.ToString("0.0") + " cm";
            // �s�a�̐ݒ芮��
            localdiameter = dis;
            VoiceTriggerOn = true;
        }
    }

    /// <summary>
    /// �ȉ~�^�s�̌v������
    /// </summary>
    /// <param name="dis">�v�������ӂ̒���</param>
    private void CheckStemDiameter(float dis)
    {
        if (!IsOneSelectDiameter)
        {
            InnerDistance = dis;
            Debug.Log($"1��ڂ̋��� = {dis}cm");
            DistanceText.text = "1��ڂ̋���" + dis.ToString("0.0") + " cm";
            IsOneSelectDiameter = true;
        }
        else
        {
            Debug.Log($"2��ڂ̋��� = {dis}cm");
            DistanceText.text = "2��ڂ̋���" + dis.ToString("0.0") + " cm";

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

            // ���a�ő��肵�Ă��邽�߁A���a�ɒu������
            LongDis = LongDis / 2;
            MinDis = MinDis / 2;

            // �ȉ~�̉~���̋ߎ��v�Z���ɑ��
            var a = (float)(Math.PI * (LongDis + MinDis));
            var b = (LongDis - MinDis) / (LongDis + MinDis);
            var c = 3 * b * b;
            var d = (float)(10 + Math.Sqrt(4 - c));

            var total = a * (1 + (c / d));

            // ���b�Z�[�W�\������
            ShowDistanceText(total);
            IsOneSelectDiameter = false;
        }
    }

    /// <summary>
    /// �B�e�C�x���g����
    /// </summary>
    /// <remarks>GameObject��InputActionHandler�C�x���g��AirTap���\�b�h��ݒ肵�Ă��������B</remarks>
    public void PhotoCaptureEvent()
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
        Debug.Log("URL�̐ݒ�");

        UnityWebRequest webRequest = UnityWebRequest.Post(URL, form);
        Debug.Log("�ڑ�����");

        //URL�ɐڑ����Č��ʂ��߂��Ă���܂őҋ@
        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError)
        {
            // ���X�|���X�R�[�h�����ď���
            Debug.Log($"[Error]Response Code : {webRequest.responseCode}");
        }
        else if (webRequest.isNetworkError)
        {
            // �G���[���b�Z�[�W�����ď���
            Debug.Log($"[Error]Message : {webRequest.error}");
        }

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
            var diameter = float.Parse(webRequest.downloadHandler.text);
            ShowDistanceText(diameter);
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


    /// <summary>
    /// �����g���K�[���m
    /// </summary>
    public bool IsVoiceTriggerOn()
    {
        bool ret = VoiceTriggerOn;
        VoiceTriggerOn = false;
        return ret;
    }

    /// <summary>
    /// �������N���A
    /// </summary>
    public void clearDistance()
    {
        locallength = 0;
        localdiameter = 0;
    }

    /// <summary>
    /// �s���擾
    /// </summary>
    public float getStemLength()
    {
        return locallength;
    }

    /// <summary>
    /// �s�a�擾
    /// </summary>
    public float getStemDiameter()
    {
        return localdiameter;
    }

}