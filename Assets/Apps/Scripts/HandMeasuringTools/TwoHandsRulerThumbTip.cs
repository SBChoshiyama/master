using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandsRulerThumbTip : MonoBehaviour
{
    /// <summary>
    /// �����\���e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh DistanceText = default;

    /// <summary>
    /// ���̃I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private LineRenderer line = default;

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

        // �n���h���C���\���ɂ���
        //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        MeasuingToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuingToolSelectorObj.GetComponent<MeasuringToolSelector>();

        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        Initialize();
    }

    public void Initialize()
    {
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        DistanceText.text = "0 cm";
    }

    void Update()
    {
        // ����̐e�w
        var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
        if (leftIndexTip == null)
        {
            Debug.Log("leftIndexTip is null.");
            return;
        }

        // �E��̐e�w
        var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
        if (rightIndexTip == null)
        {
            Debug.Log("rightIndexTip is null.");
            return;
        }

        // ����`��
        line.SetPosition(0, leftIndexTip.position);
        line.SetPosition(1, rightIndexTip.position);
        line.startWidth = 0.001f;
        line.endWidth = 0.001f;

        // �������Z�o
        var distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
        // cm�ɕϊ�
        distance = distance * 100;

        // �p�u���b�N�ϐ��ɕۑ�
        switch (stemModeSelector.InnerStemMode)
        {
            // �s�����[�h
            // �s�a���[�h
            case StemModeSelector.StemMode.Length:
            case StemModeSelector.StemMode.Diameter:
                measuringToolSelector.LineDistance = distance;
                break;

            // 1�ӂł̌s�a���[�h
            case StemModeSelector.StemMode.SingleDiameter:
                measuringToolSelector.LineDistance = (float)(distance * 3.14);
                break;
        }

        RocalTime -= Time.deltaTime;
        if (RocalTime <= 0)
        {
            DistanceText.text = distance.ToString("0.0") + " cm";
            RocalTime = 0.5F;
        }

        DistanceText.transform.position = (leftIndexTip.position + rightIndexTip.position) / 2;
    }
}
