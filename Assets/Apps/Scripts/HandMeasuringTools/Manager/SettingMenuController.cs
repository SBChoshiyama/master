using HKT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuController : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        // �ݒ胁�j���[�I�u�W�F�N�g
        SettingMenuObj = GameObject.Find("SettingMenuPanel");

        // �ݒ胁�j���[�\���ꏊ�w��I�u�W�F�N�g
        SettingMenuAnchorObj = GameObject.Find("SettingMenuAnchor");

        // ���W���[�c�[���ؑփI�u�W�F�N�g
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // �s���胂�[�h�I�u�W�F�N�g
        StemModeObj = GameObject.Find("StemModeSelector");
        StemMode = StemModeObj.GetComponent<StemModeSelector>();

        // Save���[�h�I�u�W�F�N�g
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        DistanceText.text = "�s���v��(����)";
        // �ݒ胁�j���[�͍ŏ��͔�\���ɐݒ�
        SettingMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �ݒ胁�j���[�\��
    /// </summary>
    public void SettingMenuDisplay()
    {
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;
        Debug.Log("rot:" + rot);
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
        StemMode.UseStemLength();
        MeasuringTool.UseOneHandRuler();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "�s���v��(�Ў�)";
    }

    /// <summary>
    /// �s���v���F���胂�[�h�I����
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        StemMode.UseStemLength();
        MeasuringTool.UseTwoHandsRuler();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "�s���v��(����)";
    }

    /// <summary>
    /// �s�a�v���F���胂�[�h�I����
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        StemMode.UseStemSimpleDiameter();
        MeasuringTool.UseTwoHandsRuler();
        MeasuringTool.MeasurMiddleModeOn();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "�s�a�v��(����)";
    }

    /// <summary>
    /// �s�a�v���F�摜��̓��[�h�I����
    /// </summary>
    public void DiameterGraphicSelect()
    {
        MeasuringTool.UseHandRulerOFF();
        SaveMode.UsePhotoCaptureEvent();
        DistanceText.text = "�s�a�v��(�摜���)";
    }

}
