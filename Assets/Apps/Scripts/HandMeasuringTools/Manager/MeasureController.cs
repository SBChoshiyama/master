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
    private GameObject RecordBtnObj;
    /// <summary>

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
    private GameObject RecordSlateObj;

    /// <summary>
    /// LengthTwoHand GameObject
    /// </summary>
    private GameObject LengthTwoHandObj;
    /// <summary>
    /// �c�ԍ��pObject
    /// </summary>
    private InteractiveMeshCursor LengthTwoHand;

    /// <summary>
    /// �����������t���O
    /// </summary>
    private bool initflg = false;

    // Start is called before the first frame update
    void Start()
    {
        // �n���h���C���\���ɂ���
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

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

        // voice
        VoiceObj = GameObject.Find("VoiceCommand");
        Voice = VoiceObj.GetComponent<VoiceCommand>();

        // �L�^�\���p�{�[�h�I�u�W�F�N�g
        RecordSlateObj = GameObject.Find("RecordSlate");

        // �L�^�\���{�^���p�I�u�W�F�N�g
        RecordBtnObj = GameObject.Find("RecordButton");

        // �L�^�\���{�^���p�I�u�W�F�N�g
        LengthTwoHandObj = GameObject.Find("LengthTwoHands");

        MeasureInit();
    }

    // Update is called once per frame
    void Update()
    {
        StemRecordButtonCheck();
    }

    /// <summary>
    /// �v���R���g���[��������
    /// </summary>
    public void MeasureInit()
    {
        // �v���������[�h�ŏ�����
        NoneSelect();
        // �ݒ胁�j���[�͍ŏ��͔�\���ɐݒ�
        SettingMenuObj.SetActive(false);
        // �ݒ�J�n�{�^����\��
        SettingStartBtnObj.SetActive(false);
        // �L�^�\���{�^����\��
        RecordBtnObj.SetActive(false);
        // �L�^�\���p�{�[�h��\��
        RecordSlateObj.SetActive(false);
        // ���[�J���ϐ��N���A
        Voice.clearDistance();
        // ����������
        initflg = true;
    }

    /// <summary>
    /// �ݒ胁�j���[�L����
    /// </summary>
    public void SettingMenuEnable()
    {
        // �s���v���F���胂�[�h�I���ŏ�����
        LengthTwoHandsSelect();
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();
        // �ݒ胁�j���[�͍ŏ��͔�\���ɐݒ�
        //SettingMenuObj.SetActive(false);
        // �ݒ�J�n�{�^���\��
        SettingStartBtnObj.SetActive(true);
        PlantNoText.text = "�c�ԍ�:" + PlantNo.GetPlantNo();
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
        if (initflg)
        {
            Debug.Log("dousite koko deru");
            StemMode.UseStemLength();
            MeasuringTool.UseOneHandRuler();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "�s���v��(�Ў�)";
        }
    }

    /// <summary>
    /// �s���v���F���胂�[�h�I����
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        if (initflg)
        {
            StemMode.UseStemLength();
            MeasuringTool.UseTwoHandsRuler();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "�s���v��(����)";
        }
    }

    /// <summary>
    /// �s�a�v���F���胂�[�h�I����
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        if (initflg)
        {
            StemMode.UseStemSimpleDiameter();
            MeasuringTool.UseTwoHandsRuler();
            MeasuringTool.MeasurMiddleModeOn();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "�s�a�v��(����)";
        }
    }

    /// <summary>
    /// �s�a�v���F�摜��̓��[�h�I����
    /// </summary>
    public void DiameterGraphicSelect()
    {
        if (initflg)
        {
            MeasuringTool.UseHandRulerOFF();
            SaveMode.UsePhotoCaptureEvent();
            DistanceText.text = "�s�a�v��(�摜���)";
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
    /// �s���̃Z�b�g
    /// </summary>
    public void SetStemLength(float data)
    {
        stemLength = data;
        StemRecordButtonCheck();
        Debug.Log($"stemLength = {data}cm");
    }

    /// <summary>
    /// �s�a�̃Z�b�g
    /// </summary>
    public void SetStemDiameter(float data)
    {
        stemDiameter = data;
        Debug.Log($"stemDiameter = {data}cm");
        StemRecordButtonCheck();
    }

    /// <summary>
    /// �L�^�\���{�^���\���`�F�b�N
    /// </summary>
    public void StemRecordButtonCheck()
    {
        if (Voice.IsVoiceTriggerOn())
        {
            stemLength = Voice.getStemLength();
            stemDiameter = Voice.getStemDiameter();
            Debug.Log($"stemLength = {stemLength}cm , stemDiameter = {stemDiameter}cm");
            if ((stemLength > 0) && (stemDiameter > 0))
            {
                RecordBtnObj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �L�^���ʕ\��
    /// </summary>
    public void RecordResultDisplay()
    {
        // �A���J�[���W�E�p�x���擾
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

        RecStemLengthText.text = "�s���F" + stemLength.ToString("0.0") + " cm";
        RecStemDiamText.text = "�s�a�F" + stemDiameter.ToString("0.0") + " cm";
        //�@�L�^�\���̕\���ʒu�͊J�n�{�^���������̃A���J�[�ʒu�Ƃ���
        RecordSlateObj.transform.SetPositionAndRotation(pos, rot);


        // �L�^�\���{�^����\��
        RecordBtnObj.SetActive(false);

        // �L�^�\���p�{�[�h�\��
        RecordSlateObj.SetActive(true);
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

        // ������
        MeasureInit();

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

        // ������
        MeasureInit();

        // �c�ԍ���ʕ\��
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// �L�^�\������L�[
    /// </summary>
    public void RecordPlateCloseKey()
    {
        // �L�^�\���{�^���\��
        RecordBtnObj.SetActive(true);
    }
}
