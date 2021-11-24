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
        /// 左手の長さ表示テキスト
        /// </summary>
        [SerializeField]
        private TextMesh leftDistanceText = default;

        /// <summary>
        /// 右手の長さ表示テキスト
        /// </summary>
        [SerializeField]
        private TextMesh rightDistanceText = default;

        /// <summary>
        /// 左手用の線オブジェクト
        /// </summary>
        [SerializeField]
        private LineRenderer leftLine = default;

        /// <summary>
        /// 右手用の線オブジェクト
        /// </summary>
        [SerializeField]
        private LineRenderer rightLine = default;

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

            Initialize();
        }

        public void Initialize()
        {
            leftLine.SetPosition(0, Vector3.zero);
            leftLine.SetPosition(1, Vector3.zero);
            leftDistanceText.text = "0 cm";

            rightLine.SetPosition(0, Vector3.zero);
            rightLine.SetPosition(1, Vector3.zero);
            rightDistanceText.text = "0 cm";
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

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                case StemModeSelector.StemMode.Length:
                    // 線を描画
                    leftLine.SetPosition(0, leftIndexTip.position);
                    leftLine.SetPosition(1, leftThumbTip.position);
                    leftLine.startWidth = 0.001f;
                    leftLine.endWidth = 0.001f;
                    break;

                // 茎径モード
                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // 間隔が(離間距離×2)cmよりも大きいか
                    if (leftDistance > (ReleaseLen * 2))
                    {
                        var point = leftDistance - ReleaseLen;

                        Vector3 p1 = (leftIndexTip.position * (leftDistance - point) + leftThumbTip.position * point) / leftDistance;
                        Vector3 p2 = (leftIndexTip.position * point + leftThumbTip.position * (leftDistance - point)) / leftDistance;
                        // 距離を太線の長さに変更
                        leftLine.startWidth = 0.002f;
                        leftLine.endWidth = 0.002f;
                        leftLine.SetPosition(0, p1);
                        leftLine.SetPosition(1, p2);

                        leftDistance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        leftDistance = 0;
                    }
                    break;
            }

            leftrocal -= distanceTime;
            if (leftrocal <= 0)
            {
                leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
                leftrocal = 0.5F;
            }

            leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
            var ltextPos = (leftIndexTip.position + leftThumbTip.position) / 2;
            ltextPos.y += 0.05f;
            leftDistanceText.transform.position = ltextPos;

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

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                case StemModeSelector.StemMode.Length:
                    // 線を描画
                    rightLine.SetPosition(0, rightIndexTip.position);
                    rightLine.SetPosition(1, rightThumbTip.position);
                    rightLine.startWidth = 0.001f;
                    rightLine.endWidth = 0.001f;
                    break;

                // 茎径モード
                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // 間隔が(離間距離×2)cmよりも大きいか
                    if (rightDistance > (ReleaseLen * 2))
                    {
                        var point = rightDistance - ReleaseLen;

                        Vector3 p1 = (rightIndexTip.position * (rightDistance - point) + rightThumbTip.position * point) / rightDistance;
                        Vector3 p2 = (rightIndexTip.position * point + rightThumbTip.position * (rightDistance - point)) / rightDistance;
                        // 距離を太線の長さに変更
                        rightLine.enabled = true;
                        rightLine.startWidth = 0.002f;
                        rightLine.endWidth = 0.002f;
                        rightLine.SetPosition(0, p1);
                        rightLine.SetPosition(1, p2);

                        rightDistance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        rightLine.enabled = false;
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
                rightDistanceText.text = rightDistance.ToString("0.0") + " cm";
                rightrocal = 0.5F;
            }

            var rtextPos = (rightIndexTip.position + rightThumbTip.position) / 2;
            rtextPos.y += 0.05f;
            rightDistanceText.transform.position = rtextPos;
        }
    }
}
