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

        HandStatusOKObj.SetActive(false);
        HandStatusNGObj.SetActive(true);
        HandStatusTxt.text = "";
        isHandTrack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
        // (rightIndexTip.position.y > -0.2))
            
        // �ʐ^�B�e���[�h
        if (SaveMode.isPhotoCapture())
        {
            if (!handJointService.IsHandTracked(Handedness.Right) &&
                !handJointService.IsHandTracked(Handedness.Left))
            {
                HandStatusTxt.text = "�E��E����";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }

        }
        // �Ў胂�[�h
        else if (MeasuringTool.isUseOneHands())
        {
            if (!handJointService.IsHandTracked(Handedness.Right))
            {
                HandStatusTxt.text = "�E��";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        // ���胂�[�h
        else
        {
            if (!handJointService.IsHandTracked(Handedness.Right) &&
                !handJointService.IsHandTracked(Handedness.Left))
            {
                HandStatusTxt.text = "�E��E����";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!handJointService.IsHandTracked(Handedness.Right))
            {
                HandStatusTxt.text = "�E��";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!handJointService.IsHandTracked(Handedness.Left))
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
    ///  ��̌��o
    /// </summary>
    public bool isHandTracking()
    {
        return isHandTrack;
    }
}
