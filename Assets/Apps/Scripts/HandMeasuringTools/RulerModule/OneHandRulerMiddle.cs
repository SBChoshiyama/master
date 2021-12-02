using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// 片手のハンド定規
    /// </summary>
    public class OneHandRulerMiddle : MonoBehaviour
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

        /// <summary>
        /// 指位置と計測点の離間距離(cm)
        /// </summary>
        private float ReleaseLen = 2f;

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

        private void Update()
        {
            ////////////////////////////////////////////////////////////
            ///左手
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

            Vector3 p1 = leftIndexTip.position;
            Vector3 p2 = leftThumbTip.position;
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
                    if (leftDistance > (ReleaseLen * 2))
                    {
                        var point = leftDistance - ReleaseLen;

                        p1 = (leftIndexTip.position * (leftDistance - point) + leftThumbTip.position * point) / leftDistance;
                        p2 = (leftIndexTip.position * point + leftThumbTip.position * (leftDistance - point)) / leftDistance;

                        leftDistance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        p1 = (leftIndexTip.position + leftThumbTip.position) / 2;
                        p2 = (leftIndexTip.position + leftThumbTip.position) / 2;
                        leftDistance = 0;
                    }
                    break;
            }

            leftrocal -= distanceTime;
            if (leftrocal <= 0)
            {
                LeftRulerText = leftDistance.ToString("0.0") + " cm";
                leftrocal = 0.5F;
            }
            // 左手用計測線を描画
            LineManager.RulerLineLeftDraw(p1, p2, LeftRulerText);

            ////////////////////////////////////////////////////////////
            ///右手
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


            p1 = rightIndexTip.position;
            p2 = rightThumbTip.position;
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
                    if (rightDistance > (ReleaseLen * 2))
                    {
                        var point = rightDistance - ReleaseLen;

                        p1 = (rightIndexTip.position * (rightDistance - point) + rightThumbTip.position * point) / rightDistance;
                        p2 = (rightIndexTip.position * point + rightThumbTip.position * (rightDistance - point)) / rightDistance;
                        rightDistance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        p1 = (rightIndexTip.position + rightThumbTip.position) / 2;
                        p2 = (rightIndexTip.position + rightThumbTip.position) / 2;
                        rightDistance = 0;
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
                    measuringToolSelector.LineDistance = rightDistance;
                    break;

                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                    measuringToolSelector.LineDistance = (float)(rightDistance * 3.14);
                    break;
            }

            rightrocal -= distanceTime;
            if (rightrocal <= 0)
            {
                RightRulerText = rightDistance.ToString("0.0") + " cm";
                rightrocal = 0.5F;
            }
            // 計測線を描画
            LineManager.RulerLineDraw(p1, p2, RightRulerText);
        }
    }
}
