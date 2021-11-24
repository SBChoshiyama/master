using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// ハンド定規
    /// 参考動画：https://twitter.com/hi_rom_/status/1267100537578639363
    /// </summary>
    public class TwoHandsRulerMiddle : MonoBehaviour
    {
        /// <summary>
        /// 長さ表示テキスト
        /// </summary>
        [SerializeField]
        private TextMesh DistanceText = default;

        /// <summary>
        /// 線のオブジェクト
        /// </summary>
        [SerializeField]
        private LineRenderer line = default;

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

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                case StemModeSelector.StemMode.Length:
                    // 線を描画
                    line.SetPosition(0, leftIndexTip.position);
                    line.SetPosition(1, rightIndexTip.position);
                    line.startWidth = 0.001f;
                    line.endWidth = 0.001f;
                    break;

                // 茎径モード
                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // 間隔が(離間距離×2)cmよりも大きいか
                    if (distance > (ReleaseLen * 2))
                    {
                        var point = distance - ReleaseLen;

                        Vector3 p1 = (rightIndexTip.position * (distance - point) + leftIndexTip.position * point) / distance;
                        Vector3 p2 = (rightIndexTip.position * point + leftIndexTip.position * (distance - point)) / distance;
                        // 距離を太線の長さに変更
                        line.enabled = true;
                        line.startWidth = 0.002f;
                        line.endWidth = 0.002f;
                        line.SetPosition(0, p1);
                        line.SetPosition(1, p2);

                        distance -= (ReleaseLen * 2);
                    }
                    else
                    {
                        line.enabled = false;
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
                    measuringToolSelector.LineDistance = distance;
                    break;

                // 1辺での茎径モード
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

            var textPos = (leftIndexTip.position + rightIndexTip.position) / 2;
            textPos.y += 0.05f;
            DistanceText.transform.position = textPos;
        }
    }
}
