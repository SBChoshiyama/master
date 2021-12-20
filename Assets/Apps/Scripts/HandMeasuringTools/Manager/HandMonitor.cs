using HKT;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandMonitor : MonoBehaviour
{
    /// <summary>
    /// HandJointService�C���X�^���X
    /// </summary>
    private IMixedRealityHandJointService handJointService = null;

    /// <summary>
    /// DataProviderAccess�C���X�^���X
    /// </summary>
    private IMixedRealityDataProviderAccess dataProviderAccess = null;

    /// <summary>
    /// HandStatus(OK)�\���pGameObject
    /// </summary>
    private GameObject HandStatusOKObj;

    /// <summary>
    /// HandStatus(NG)�\���pGameObject
    /// </summary>
    private GameObject HandStatusNGObj;

    /// <summary>
    /// ���W���[�c�[���ؑ�GameObject
    /// </summary>
    private GameObject MeasuringToolObj;

    /// <summary>
    /// ���W���[�c�[���ؑ�Object
    /// </summary>
    private MeasuringToolSelector MeasuringTool;

    /// <summary>
    ///  Save���[�hGameObject
    /// </summary>
    private GameObject SaveModeObj;

    /// <summary>
    ///  Save���[�hObject
    /// </summary>
    private SavingToolSelector SaveMode;

    /// <summary>
    /// HandStatus�\���p�e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh HandStatusTxt = default;

    /// <summary>
    ///  ��̌��o�t���O
    /// </summary>
    private bool isHandTrack;

    /// <summary>
    ///  �E��̌��o�p�J�E���^
    /// </summary>
    private int RightHandCnt;

    /// <summary>
    ///  ����̌��o�p�J�E���^
    /// </summary>
    private int LeftHandCnt;

    /// <summary>
    ///  ���񌟏o�ŗL���Ƃ��邩
    /// </summary>
    private int CNTMAX = 10;

    // Start is called before the first frame update
    void Start()
    {
        // HandStatus(OK)�\���p�I�u�W�F�N�g
        HandStatusOKObj = GameObject.Find("HandStatusOK");

        // HandStatus(NG)�\���p�I�u�W�F�N�g
        HandStatusNGObj = GameObject.Find("HandStatusNG");

        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService == null)
        {
            Debug.LogError("Can't get IMixedRealityHandJointService.");
            return;
        }

        dataProviderAccess = CoreServices.InputSystem as IMixedRealityDataProviderAccess;
        if (dataProviderAccess == null)
        {
            Debug.LogError("Can't get IMixedRealityDataProviderAccess.");
            return;
        }

        // ���W���[�c�[���ؑփI�u�W�F�N�g
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // Save���[�h�I�u�W�F�N�g
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        // ������
        HandMonitorInit();
    }

    // Update is called once per frame
    void Update()
    {
        // ��̌��o���菈��
        // �E�茟�o�`�F�b�N
        if (handJointService.IsHandTracked(Handedness.Right))
        {
            if (RightHandCnt > 0)
                RightHandCnt--;
        }
        else
        {
            RightHandCnt = CNTMAX;
        }
        // �����o�`�F�b�N
        if (handJointService.IsHandTracked(Handedness.Left))
        {
            if (LeftHandCnt > 0)
                LeftHandCnt--;
        }
        else
        {
            LeftHandCnt = CNTMAX;
        }

        // ���[�h�ʂɗL���Ȏ���`�F�b�N
        // �ʐ^�B�e���[�h(�E�肩��������m���Ă����OK)
        if (SaveMode.isPhotoCapture())
        {
            if (!isRightHandFind() && !isLeftHandFind())
            {
                HandStatusTxt.text = "�E��E����";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }

        }
        // �葪��OFF���[�h
        else if (MeasuringTool.isUseHandRulerOFF())
        {
            HandStatusTxt.text = "";
            HandStatusOKObj.SetActive(false);
            HandStatusNGObj.SetActive(false);
            isHandTrack = false;
            return;
        }
        // �Ў胂�[�h(�E������m���Ă����OK)
        else if (MeasuringTool.isUseOneHands())
        {
            if (!isRightHandFind())
            {
                HandStatusTxt.text = "�E��";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        // ���胂�[�h(�E��E����Ƃ��Ɍ��m���Ă����OK)
        else
        {
            if (!isRightHandFind() && !isLeftHandFind())
            {
                HandStatusTxt.text = "�E��E����";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!isRightHandFind())
            {
                HandStatusTxt.text = "�E��";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!isLeftHandFind())
            {
                HandStatusTxt.text = "����";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        HandStatusTxt.text = "";
        HandStatusOKObj.SetActive(true);
        HandStatusNGObj.SetActive(false);
        isHandTrack = true;
    }
    /// <summary>
    ///  �n���h���j�^�[������
    /// </summary>
    private void HandMonitorInit()
    {
        HandStatusOKObj.SetActive(false);
        HandStatusNGObj.SetActive(false);
        HandStatusTxt.text = "";
        isHandTrack = false;

        RightHandCnt = CNTMAX;
        LeftHandCnt = CNTMAX;
    }

    /// <summary>
    ///  �E�茟�o
    /// </summary>
    private bool isRightHandFind()
    {
        if (RightHandCnt == 0)
            return true;
        return false;
    }

    /// <summary>
    ///  ���茟�o
    /// </summary>
    private bool isLeftHandFind()
    {
        if (LeftHandCnt == 0)
            return true;
        return false;
    }

    /// <summary>
    ///  ��̌��o
    /// </summary>
    public bool isHandTracking()
    {
        return isHandTrack;
    }
    /// <summary>
    ///  ����̌��o
    /// </summary>
    public bool isLeftHandTracking()
    {
        if (isLeftHandFind())
            return true;
        return false;
    }
}
