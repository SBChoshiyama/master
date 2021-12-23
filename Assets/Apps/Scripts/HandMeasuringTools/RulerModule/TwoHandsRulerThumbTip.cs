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
    /// HandJointServiceインスタンス
    /// </summary>
    private IMixedRealityHandJointService handJointService = null;

    /// <summary>
    /// DataProviderAccessインスタンス
    /// </summary>
    private IMixedRealityDataProviderAccess dataProviderAccess = null;

    /// <summary>
    /// 測定モード選択用GameObject
    /// </summary>
    private GameObject MeasuingToolSelectorObj;

    /// <summary>
    /// 測定モード選択用スクリプトObject
    /// </summary>
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// 茎モード選択用GameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// 茎モード選択用スクリプトObject
    /// </summary>
    private StemModeSelector stemModeSelector;

    /// <summary>
    /// 線描画制御用GameObject
    /// </summary>
    private GameObject LineManagerObj;

    /// <summary>
    /// 線描画制御用スクリプトObject
    /// </summary>
    private RulerLineManager LineManager;

    /// <summary>
    /// 距離表示用テキスト
    /// </summary>
    private string RulerText;

    /// <summary>
    /// 長さの測定間隔
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
        // 左手の親指
        var leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
        if (leftThumbTip == null)
        {
            Debug.Log("leftThumbTip is null.");
            return;
        }

        // 右手の親指
        var rightThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
        if (rightThumbTip == null)
        {
            Debug.Log("rightThumbTip is null.");
            return;
        }

        // 距離を算出
        var distance = Vector3.Distance(leftThumbTip.position, rightThumbTip.position);
        // cmに変換
        distance = distance * 100;

        // パブリック変数に保存
        switch (stemModeSelector.InnerStemMode)
        {
            // 茎長モード
            // 茎径モード
            case StemModeSelector.StemMode.Length:
            case StemModeSelector.StemMode.Diameter:
                // 小数点1桁で四捨五入
                measuringToolSelector.LineDistance = ((float)Math.Round(distance * 10)) / 10;
                break;

            // 1辺での茎径モード
            case StemModeSelector.StemMode.SingleDiameter:
                // 茎径は㎜単位に変換(×10)
                measuringToolSelector.LineDistance = (float)Math.Round((distance * 3.14 * 10), MidpointRounding.AwayFromZero);
                break;
        }

        RocalTime -= Time.deltaTime;
        if (RocalTime <= 0)
        {
            RulerText = distance.ToString("0.0") + " cm";
            RocalTime = 0.5F;
        }

        // 線を描画
        LineManager.RulerLineDraw(leftThumbTip.position, rightThumbTip.position, RulerText);
    }
}
