using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System;

namespace MRTK_HKSample
{
    public class TwoHandsRulerMiddle : MonoBehaviour
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

        /// <summary>
        /// 指位置と計測点の離間距離(cm)
        /// </summary>
        private float ReleaseLen = 2f;

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
            // 左手
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // 右手
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // 距離を算出
            var distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
            // cmに変換
            distance = distance * 100;
            Vector3 p1 = leftIndexTip.position;
            Vector3 p2 = rightIndexTip.position;

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                case StemModeSelector.StemMode.Length:
                   break;

                // 茎径モード
                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // 間隔が(離間距離×2)cmよりも大きいか
                    if (distance > (ReleaseLen * 2))
                    {
                        var point = distance - ReleaseLen;

                        p1 = (rightIndexTip.position * (distance - point) + leftIndexTip.position * point) / distance;
                        p2 = (rightIndexTip.position * point + leftIndexTip.position * (distance - point)) / distance;
                        distance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        p1 = p2 = (rightIndexTip.position + leftIndexTip.position) / 2;
                        distance = 0;
                    }
                    break;
            }

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
            LineManager.RulerLineDraw(p1, p2, RulerText);
        }
    }
}
