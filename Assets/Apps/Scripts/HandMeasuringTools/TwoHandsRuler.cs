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
    public class TwoHandsRuler : MonoBehaviour
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

            DistanceText.transform.position = (leftIndexTip.position + rightIndexTip.position) / 2;
        }
    }
}
