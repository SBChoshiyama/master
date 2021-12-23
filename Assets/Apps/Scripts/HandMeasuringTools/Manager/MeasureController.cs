using HKT;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MeasureController : MonoBehaviour
{
    /// <summary>
    /// �n���h���j�^�pGameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// �ݒ胁�j���[�p�l���pGameObject
    /// </summary>
    private GameObject SettingMenuObj;

    /// <summary>
    /// �ݒ胁�j���[�\���ꏊ�w��GameObject
    /// </summary>
    private GameObject SettingMenuAnchorObj;

    /// <summary>
    /// �ݒ胁�j���[�J�n�{�^��GameObject
    /// </summary>
    private GameObject SettingStartBtnObj;

    /// <summary>
    /// �c�ԍ��pGameObject
    /// </summary>
    private GameObject PlantNoObj;

    /// <summary>
    /// �c�ԍ��pObject
    /// </summary>
    private PlantNoManager PlantNo;

    /// <summary>
    /// �c�ԍ����͉��GameObject
    /// </summary>
    private GameObject PlantNoPlateObj;

    /// <summary>
    /// ���W���[�c�[���ؑ�GameObject
    /// </summary>
    private GameObject MeasuringToolObj;

    /// <summary>
    /// ���W���[�c�[���ؑ�Object
    /// </summary>
    private MeasuringToolSelector MeasuringTool;

    /// <summary>
    /// �s���胂�[�hGameObject
    /// </summary>
    private GameObject StemModeObj;

    /// <summary>
    /// �s���胂�[�hObject
    /// </summary>
    private StemModeSelector StemMode;

    /// <summary>
    ///  Save���[�hGameObject
    /// </summary>
    private GameObject SaveModeObj;

    /// <summary>
    ///  Save���[�hObject
    /// </summary>
    private SavingToolSelector SaveMode;

    /// <summary>
    /// ���`�搧��pGameObject
    /// </summary>
    private GameObject LineManagerObj;

    /// <summary>
    /// ���`�搧��p�X�N���v�gObject
    /// </summary>
    private RulerLineManager LineManager;

    /// <summary>
    ///  Distance�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh DistanceText = default;

    /// <summary>
    ///  PlantNo�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoText = default;

    /// <summary>
    ///  �L�^��PlantNo�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh RecPlantNoText = default;

    /// <summary>
    ///  �L�^���s���e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh RecStemLengthText = default;
    /// <summary>
    ///  �L�^���s�a�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh RecStemDiamText = default;

    /// <summary>
    /// �s���̑��茋��
    /// </summary>
    private float stemLength;

    /// <summary>
    /// �s�a�̑��茋��
    /// </summary>
    private float stemDiameter;

    /// <summary>
    /// �L�^�\���{�^���pGameObject
    /// </summary>
    private GameObject DisplayResultBtnObj;

    /// <summary>
    /// ReturnTop�{�^���pGameObject
    /// </summary>
    private GameObject ReturnTopBtnObj;

    /// <summary>
    /// Voice�pGameObject
    /// </summary>
    private GameObject VoiceObj;

    /// VoiceObject
    /// </summary>
    private VoiceCommand Voice;

    /// <summary>
    /// �L�^�\���pGameObject
    /// </summary>
    private GameObject MesuringResultSlateObj;

    /// <summary>
    /// LengthTwoHand GameObject
    /// </summary>
    private GameObject LengthTwoHandObj;
    /// <summary>
    /// �c�ԍ��pObject
    /// </summary>
    private InteractiveMeshCursor LengthTwoHand;

    /// <summary>
    /// �����\���e�L�X�g
    /// </summary>
    private string DefaultText;

    /// <summary>
    /// �������t���O
    /// </summary>
    private bool InitFlg = false;

    /// <summary>
    /// �N������̂ݎ��s�p
    /// </summary>
    private bool isFirstFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        // �n���h���C���\���ɂ���
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        // �n���h���j�^GameObject
        HandMonitorObj = GameObject.Find("HandStatus");

        // �c�ԍ��I�u�W�F�N�g
        PlantNoObj = GameObject.Find("PlantNoManager");
        PlantNo = PlantNoObj.GetComponent<PlantNoManager>();

        // �c�ԍ����͉��GameObject
        PlantNoPlateObj = GameObject.Find("PlantNoInputPlate");

        // �ݒ胁�j���[�I�u�W�F�N�g
        SettingMenuObj = GameObject.Find("SettingMenuPanel");

        // �ݒ胁�j���[�\���ꏊ�w��I�u�W�F�N�g
        SettingMenuAnchorObj = GameObject.Find("SettingMenuAnchor");

        /// �ݒ胁�j���[�J�n�{�^���I�u�W�F�N�g
        SettingStartBtnObj = GameObject.Find("SettingStartButton");

        // ���W���[�c�[���ؑփI�u�W�F�N�g
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // �s���胂�[�h�I�u�W�F�N�g
        StemModeObj = GameObject.Find("StemModeSelector");
        StemMode = StemModeObj.GetComponent<StemModeSelector>();

        // Save���[�h�I�u�W�F�N�g
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        // ���`��I�u�W�F�N�g
        LineManagerObj = GameObject.Find("RulerLineManager");
        LineManager = LineManagerObj.GetComponent<RulerLineManager>();

        // voice
        VoiceObj = GameObject.Find("VoiceCommand");
        Voice = VoiceObj.GetComponent<VoiceCommand>();

        // �L�^�\���p�{�[�h�I�u�W�F�N�g
        MesuringResultSlateObj = GameObject.Find("MesuringResultSlate");

        // �L�^�\���{�^���p�I�u�W�F�N�g
        DisplayResultBtnObj = GameObject.Find("DisplayResultButton");

        // ReturnTop�{�^���p�I�u�W�F�N�g
        ReturnTopBtnObj = GameObject.Find("ReturnTopButton");
        
        // �����\���p�e�L�X�g
        DefaultText = "";

        MeasureInit();

        // �������t���Oset
        InitFlg = true;

        // ����t���Oset
        isFirstFlg = true;

    }

    // Update is called once per frame
    void Update()
    {
        // �������m������΍X�V
        if (Voice.IsVoiceTriggerOn())
        {
            MesuringResultButtonCheck();
        }
    }

    /// <summary>
    /// �v���R���g���[��������
    /// </summary>
    public void MeasureInit()
    {
        // �ݒ胁�j���[�͍ŏ��͔�\���ɐݒ�
        SettingMenuObj.SetActive(false);
        // �ݒ�J�n�{�^����\��
        SettingStartBtnObj.SetActive(false);
        // �L�^�\���p�{�[�h��\��
        MesuringResultSlateObj.SetActive(false);
        // �L�^�\���{�^����\��
        DisplayResultBtnObj.SetActive(false);
        // ReturnTop�{�^����\��
        ReturnTopBtnObj.SetActive(false);
        // ���[�J���ϐ��N���A
        Voice.clearDistance();

        // �v���������[�h�ŋN��
        NoneSelect();

        // �܂�����c�[���͒�~
        MeasureStop();

        // �n���h���j�^����
        HandMonitorObj.SetActive(false);
    }

    /// <summary>
    /// �ݒ胁�j���[�L����
    /// </summary>
    public void SettingMenuEnable()
    {
        // ������菈��
        if (isFirstFlg)
        {
            // �s���v���F���胂�[�h�I���ŏ�����
            LengthTwoHandsSelect();
            isFirstFlg = false;
        }
        // ����c�[���͐ݒ�
        MeasuringTool.MeasurToolSet();
        // �ݒ�J�n�{�^���\��
        SettingStartBtnObj.SetActive(true);
        DistanceText.text = DefaultText;
        PlantNoText.text = "�c�ԍ�:" + PlantNo.GetPlantNo();
        // �����֌W�̃I�u�W�F�N�g�ĕ\��
        SaveMode.VoiceObjRedisplay();
        // �n���h���j�^�L��
        HandMonitorObj.SetActive(true);
        // �L�^�\���{�^���\���`�F�b�N
        MesuringResultButtonCheck();
    }
    /// <summary>
    /// �ݒ胁�j���[�\��
    /// </summary>
    public void SettingMenuDisplay()
    {
        // �A���J�[���W�E�p�x���擾
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

        //�@�ݒ胁�j���[�̕\���ʒu�͊J�n�{�^���������̃A���J�[�ʒu�Ƃ���
        SettingMenuObj.transform.SetPositionAndRotation(pos, rot);

        // �ݒ胁�j���[�\��
        SettingMenuObj.SetActive(true);
    }

    /// <summary>
    /// �s���v���F�Ў胂�[�h�I����
    /// </summary>
    public void LengthOneHandSelect()
    {
        // �������������Ȃ��Ə������Ȃ�
        if (InitFlg)
        {
            // �ݒ�o�^
            StemMode.UseStemLength();
            MeasuringTool.UseOneHandRuler();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "�s���v��(�Ў�)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// �s���v���F���胂�[�h�I����
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        // �������������Ȃ��Ə������Ȃ�
        if (InitFlg)
        {
            // �ݒ�o�^
            StemMode.UseStemLength();
            MeasuringTool.UseTwoHandsRuler();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "�s���v��(����)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// �s�a�v���F���胂�[�h�I����
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        // �������������Ȃ��Ə������Ȃ�
        if (InitFlg)
        {
            // �ݒ�o�^
            StemMode.UseStemSimpleDiameter();
            MeasuringTool.UseTwoHandsRuler();
            MeasuringTool.MeasurMiddleModeOn();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "�s�a�v��(����)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// �s�a�v���F�摜��̓��[�h�I����
    /// </summary>
    public void DiameterGraphicSelect()
    {
        if (InitFlg)
        {
            MeasuringTool.UseHandRulerOFF();
            SaveMode.UsePhotoCaptureEvent();
            DefaultText = "�s�a�v��(�摜���)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// �v���������[�h(�v���֌W�̕\�����������Ȃ��Ƃ�)
    /// </summary>
    public void NoneSelect()
    {
        MeasuringTool.UseHandRulerOFF();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "";
        PlantNoText.text = "";
    }

    /// <summary>
    /// �v����~(�v���֌W�̕\�����������Ȃ��Ƃ�)
    /// </summary>
    public void MeasureStop()
    {
        DistanceText.text = "";
        PlantNoText.text = "";
        MeasuringTool.MeasurToolOff();
    }

    /// <summary>
    /// �s���̃Z�b�g
    /// </summary>
    public void SetStemLength(float data)
    {
        stemLength = data;
        MesuringResultButtonCheck();
        Debug.Log($"stemLength = {data}cm");
    }

    /// <summary>
    /// �s�a�̃Z�b�g
    /// </summary>
    public void SetStemDiameter(float data)
    {
        stemDiameter = data;
        Debug.Log($"stemDiameter = {data}cm");
        MesuringResultButtonCheck();
    }

    /// <summary>
    /// �v�����ʕ\���{�^���\���`�F�b�N
    /// </summary>
    public void MesuringResultButtonCheck()
    {
        stemLength = Voice.getStemLength();
        stemDiameter = Voice.getStemDiameter();
        Debug.Log($"stemLength = {stemLength}cm , stemDiameter = {stemDiameter}cm");
        if ((stemLength > 0) && (stemDiameter > 0))
        {
            // ReturnTop�{�^����\��
            ReturnTopBtnObj.SetActive(false);
            // �L�^�\���{�^���\��
            DisplayResultBtnObj.SetActive(true);
        }
        else
        {
            // �L�^�\���{�^����\��
            DisplayResultBtnObj.SetActive(false);
            // ReturnTop�{�^���\��
            ReturnTopBtnObj.SetActive(true);
        }
    }

    /// <summary>
    /// �L�^���ʕ\��
    /// </summary>
    public void MesuringResultDisplay()
    {
        // �A���J�[���W�E�p�x���擾
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

        RecPlantNoText.text = "�c�ԍ�:" + PlantNo.GetPlantNo();
        RecStemLengthText.text = "�s���F" + stemLength.ToString("0.0") + " cm";
        RecStemDiamText.text = "�s�a�F" + stemDiameter.ToString("0") + " mm";
        //�@�L�^�\���̕\���ʒu�͊J�n�{�^���������̃A���J�[�ʒu�Ƃ���
        MesuringResultSlateObj.transform.SetPositionAndRotation(pos, rot);

        // �v����~
        MeasureStop();
        
        // ���`�����
        LineManager.RulerLineErase();

        // �n���h���j�^����
        HandMonitorObj.SetActive(false);

        // �ݒ�J�n�{�^����\��
        SettingStartBtnObj.SetActive(false);

        // �L�^�\���{�^����\��
        DisplayResultBtnObj.SetActive(false);

        // �����֌W�̃I�u�W�F�N�g����
        SaveMode.VoiceObjClear();

        // �L�^�\���p�{�[�h�\��
        MesuringResultSlateObj.SetActive(true);
    }

    /// <summary>
    /// �v�����ʊm��
    /// </summary>
    public void SetRecords()
    {
        // �v�����ʑ��M����(TBD)


        // �v�����ʃN���A
        stemLength = 0;
        stemDiameter = 0;
        Voice.clearDistance();

        // �L�^�\���p�{�[�h��\��
        MesuringResultSlateObj.SetActive(false);

        // �c�ԍ���ʕ\��
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// �c�ԍ����֖͂߂�
    /// </summary>
    public void ReturnKey()
    {
        // �v�����ʃN���A
        stemLength = 0;
        stemDiameter = 0;
        Voice.clearDistance();

        // �L�^�\���p�{�[�h��\��
        MesuringResultSlateObj.SetActive(false);

        // �c�ԍ���ʕ\��
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// �c�ԍ����֖͂߂�(����)
    /// </summary>
    public void DirectReturnKey()
    {
        // �v�����ʃN���A
        stemLength = 0;
        stemDiameter = 0;
        Voice.clearDistance();

        // �v����~
        MeasureStop();

        // ���`�����
        LineManager.RulerLineErase();

        // �n���h���j�^����
        HandMonitorObj.SetActive(false);

        // �ݒ�J�n�{�^����\��
        SettingStartBtnObj.SetActive(false);

        // �����֌W�̃I�u�W�F�N�g����
        SaveMode.VoiceObjClear();

        // ReturnTop�{�^����\��
        ReturnTopBtnObj.SetActive(false);

        // �c�ԍ���ʕ\��
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// �L�^�\������L�[
    /// </summary>
    public void RecordPlateCloseKey()
    {
        // �ݒ�J�n�{�^���\��
        SettingStartBtnObj.SetActive(true);

        // �L�^�\���{�^���\��
        DisplayResultBtnObj.SetActive(true);

        // �v���ĊJ
        SettingMenuEnable();
    }
}
