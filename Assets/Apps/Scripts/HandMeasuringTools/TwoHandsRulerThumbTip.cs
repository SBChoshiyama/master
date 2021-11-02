using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandsRulerThumbTip : MonoBehaviour
{
    [SerializeField]
    private TextMesh DistanceText = default;

    [SerializeField]
    private LineRenderer line = default;

    private IMixedRealityHandJointService handJointService = null;
    private IMixedRealityDataProviderAccess dataProviderAccess = null;

    GameObject MeasuingToolSelectorObj;
    MeasuringToolSelector measuringToolSelector;

    float rocal = 0.5F;

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

        // ハンドレイを非表示にする
        //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        MeasuingToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuingToolSelectorObj.GetComponent<MeasuringToolSelector>();

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
        // 左手の親指
        var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
        if (leftIndexTip == null)
        {
            Debug.Log("leftIndexTip is null.");
            return;
        }

        // 右手の親指
        var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
        if (rightIndexTip == null)
        {
            Debug.Log("rightIndexTip is null.");
            return;
        }

        // 線を描画
        line.SetPosition(0, leftIndexTip.position);
        line.SetPosition(1, rightIndexTip.position);
        line.startWidth = 0.001f;
        line.endWidth = 0.001f;

        // 距離を算出
        var distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
        // cmに変換
        distance = distance * 100;

        // パブリック変数に保存
        measuringToolSelector.LineDistance = distance;

        rocal -= Time.deltaTime;
        if (rocal <= 0)
        {
            DistanceText.text = distance.ToString("0.0") + " cm";
            rocal = 0.5F;
        }

        DistanceText.transform.position = (leftIndexTip.position + rightIndexTip.position) / 2;
    }
}
