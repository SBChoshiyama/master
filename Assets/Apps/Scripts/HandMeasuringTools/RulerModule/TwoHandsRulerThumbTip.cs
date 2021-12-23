using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TwoHandsRulerThumbTip : MonoBehaviour
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
    /// ���胂�[�h�I��pGameObject
    /// </summary>
    private GameObject MeasuingToolSelectorObj;

    /// <summary>
    /// ���胂�[�h�I��p�X�N���v�gObject
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
    /// ���`�搧��pGameObject
    /// </summary>
    private GameObject LineManagerObj;

    /// <summary>
    /// ���`�搧��p�X�N���v�gObject
    /// </summary>
    private RulerLineManager LineManager;

    /// <summary>
    /// �����\���p�e�L�X�g
    /// </summary>
    private string RulerText;

    /// <summary>
    /// �����̑���Ԋu
    /// </summary>
    float RocalTime = 0.5F;

    void Start()
    {
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

        MeasuingToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuingToolSelectorObj.GetComponent<MeasuringToolSelector>();

        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        LineManagerObj = GameObject.Find("RulerLineManager");
        LineManager = LineManagerObj.GetComponent<RulerLineManager>();

        Initialize();
    }

    public void Initialize()
    {
        LineManager.RulerLineInit();
    }

    void Update()
    {
        // ����̐e�w
        var leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
        if (leftThumbTip == null)
        {
            Debug.Log("leftThumbTip is null.");
            return;
        }

        // �E��̐e�w
        var rightThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
        if (rightThumbTip == null)
        {
            Debug.Log("rightThumbTip is null.");
            return;
        }

        // �������Z�o
        var distance = Vector3.Distance(leftThumbTip.position, rightThumbTip.position);
        // cm�ɕϊ�
        distance = distance * 100;

        // �p�u���b�N�ϐ��ɕۑ�
        switch (stemModeSelector.InnerStemMode)
        {
            // �s�����[�h
            // �s�a���[�h
            case StemModeSelector.StemMode.Length:
            case StemModeSelector.StemMode.Diameter:
                // �����_1���Ŏl�̌ܓ�
                measuringToolSelector.LineDistance = ((float)Math.Round(distance * 10)) / 10;
                break;

            // 1�ӂł̌s�a���[�h
            case StemModeSelector.StemMode.SingleDiameter:
                // �s�a�͇o�P�ʂɕϊ�(�~10)
                measuringToolSelector.LineDistance = (float)Math.Round((distance * 3.14 * 10), MidpointRounding.AwayFromZero);
                break;
        }

        RocalTime -= Time.deltaTime;
        if (RocalTime <= 0)
        {
            RulerText = distance.ToString("0.0") + " cm";
            RocalTime = 0.5F;
        }

        // ����`��
        LineManager.RulerLineDraw(leftThumbTip.position, rightThumbTip.position, RulerText);
    }
}
