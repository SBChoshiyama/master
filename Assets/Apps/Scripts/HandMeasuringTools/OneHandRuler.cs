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
    public class OneHandRuler : MonoBehaviour
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
        /// 右手用の線オブジェクト(茎径モード時)
        /// </summary>
        [SerializeField]
        private LineRenderer rightLine2 = default;

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
        /// 円の半径
        /// </summary>
        private float m_radius = 0;

        /// <summary>
        /// 円の線の太さ
        /// </summary>
        private float m_lineWidth = 0.002f;

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

            rightLine2.SetPosition(0, Vector3.zero);
            rightLine2.SetPosition(1, Vector3.zero);
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

            // 線を描画
            leftLine.SetPosition(0, leftIndexTip.position);
            leftLine.SetPosition(1, leftThumbTip.position);
            leftLine.startWidth = 0.001f;
            leftLine.endWidth = 0.001f;

            // 距離を算出
            var leftDistance = Vector3.Distance(leftIndexTip.position, leftThumbTip.position);

            // cmに変換
            leftDistance = leftDistance * 100;

            leftrocal -= distanceTime;
            if (leftrocal <= 0)
            {
                leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
                leftrocal = 0.5F;
            }

            leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
            leftDistanceText.transform.position = (leftIndexTip.position + leftThumbTip.position) / 2;

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

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                case StemModeSelector.StemMode.Length:
                    // 線を描画
                    rightLine.positionCount = 2;
                    rightLine.SetPosition(0, rightIndexTip.position);
                    rightLine.SetPosition(1, rightThumbTip.position);
                    rightLine.startWidth = 0.001f;
                    rightLine.endWidth = 0.001f;
                    rightLine.loop = false;

                    rightLine2.enabled = false;
                    break;

                // 茎径モード
                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // 円を描画
                    DrawCircle();
                    // 距離を円の直径に変更
                    rightDistance = rightDistance / 5;
                    break;
            }

            // cmに変換
            rightDistance = rightDistance * 100;

            // パブリック変数に保存
            switch (stemModeSelector.InnerStemMode)
            {
                // 茎長モード
                // 茎径モード
                case StemModeSelector.StemMode.Length:
                case StemModeSelector.StemMode.Diameter:
                    measuringToolSelector.LineDistance = rightDistance;
                    rightrocal -= distanceTime;
                    if (rightrocal <= 0)
                    {
                        rightDistanceText.text = rightDistance.ToString("0.0") + " cm";
                        rightrocal = 0.5F;
                    }

                    rightDistanceText.transform.position = (rightIndexTip.position + rightThumbTip.position) / 2;
                    break;

                // 1辺での茎径モード
                case StemModeSelector.StemMode.SingleDiameter:
                    measuringToolSelector.LineDistance = (float)(rightDistance * 3.14);
                    rightrocal -= distanceTime;
                    if (rightrocal <= 0)
                    {
                        rightDistanceText.text = rightDistance.ToString("0.0") + " cm";
                        rightrocal = 0.5F;
                    }
                    // 距離表示は人差し指寄りに(重なるとマーカが見えにくいため)
                    rightDistanceText.transform.position = rightIndexTip.position;
                    break;
            }
        }

        /// <summary>
        /// 円形マーカー描画
        /// </summary>
        private void DrawCircle()
        {
            var segments = 360;

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
            m_radius = rightDistance / 2;

            // 中心点の算出
            Vector3 center = (rightIndexTip.position + rightThumbTip.position) * 0.5f;

            rightLine.startWidth = m_lineWidth;
            rightLine.endWidth = m_lineWidth;
            rightLine.positionCount = segments;
            rightLine.loop = true;

            rightLine2.startWidth = m_lineWidth;
            rightLine2.endWidth = m_lineWidth;
            rightLine2.positionCount = segments;
            rightLine2.loop = true;
            rightLine2.enabled = true;

            var points = new Vector3[segments * 2];
            var points2 = new Vector3[segments * 2];

            for (int i = 0; i < segments; i++)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                float x = Mathf.Sin(rad) * m_radius;
                float y = Mathf.Cos(rad) * m_radius;
                points[i] = new Vector3(center.x + x / 5, center.y, center.z - y / 5);
                points2[i] = new Vector3(center.x + x / 5, center.y, center.z + y / 5);
            }

            rightLine.SetPositions(points);
            rightLine2.SetPositions(points2);
        }
    }
}