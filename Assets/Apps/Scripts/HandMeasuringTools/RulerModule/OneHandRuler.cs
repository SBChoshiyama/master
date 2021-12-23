using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System;

namespace MRTK_HKSample
{
    /// <summary>
    /// 片手のハンド定規
    /// </summary>
    public class OneHandRuler : MonoBehaviour
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
        /// 左手距離表示用テキスト
        /// </summary>
        private string LeftRulerText;

        /// <summary>
        /// 右手距離表示用テキスト
        /// </summary>
        private string RightRulerText;

        /// <summary>
        /// 左手用の長さの測定間隔
        /// </summary>
        private float leftrocal = 0.5F;

        /// <summary>
        /// 右手用の長さの測定間隔
        /// </summary>
        private float rightrocal = 0.5F;

        private void Start()
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

        private void Update()
        {
            // 左手 人差し指
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // 左手 親指
            var leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
            if (leftThumbTip == null)
            {
                Debug.Log("leftThumbTip is null.");
                return;
            }

            var distanceTime = Time.deltaTime;

            // 距離を算出
            var leftDistance = Vector3.Distance(leftIndexTip.position, leftThumbTip.position);

            // cmに変換
            leftDistance = leftDistance * 100;

            leftrocal -= distanceTime;
            if (leftrocal <= 0)
            {
                LeftRulerText = leftDistance.ToString("0.0") + " cm";
                leftrocal = 0.5F;
            }
            // 左手計測線を描画
            LineManager.RulerLineLeftDraw(leftThumbTip.position, leftIndexTip.position, LeftRulerText);

            // 右手 人差し指
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // 右手 親指
            var rightThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
            if (rightThumbTip == null)
            {
                Debug.Log("rightThumbTip is null.");
                return;
            }

            // 距離を算出
            var rightDistance = Vector3.Distance(rightIndexTip.position, rightThumbTip.position);

            // cmに変換
            rightDistance = rightDistance * 100;

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                // 茎径モード
                case StemModeSelector.StemMode.Length:
                case StemModeSelector.StemMode.Diameter:
                    // 小数点1桁で四捨五入
                    measuringToolSelector.LineDistance = ((float)Math.Round(rightDistance * 10))/10;
                    break;

                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                    // 茎径は㎜単位に変換(×10)
                    measuringToolSelector.LineDistance = (float)Math.Round((rightDistance * 3.14 * 10), MidpointRounding.AwayFromZero);
                    break;
            }

            rightrocal -= distanceTime;
            if (rightrocal <= 0)
            {
                RightRulerText = rightDistance.ToString("0.0") + " cm";
                rightrocal = 0.5F;
            }
            // 計測線を描画
            LineManager.RulerLineDraw(rightIndexTip.position, rightThumbTip.position, RightRulerText);
        }
    }
}