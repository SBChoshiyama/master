using HKT;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// �Ў�̃n���h��K
    /// </summary>
    public class OneHandRulerMiddle : MonoBehaviour
    {
        /// <summary>
        /// ����̒����\���e�L�X�g
        /// </summary>
        [SerializeField]
        private TextMesh leftDistanceText = default;

        /// <summary>
        /// �E��̒����\���e�L�X�g
        /// </summary>
        [SerializeField]
        private TextMesh rightDistanceText = default;

        /// <summary>
        /// ����p�̐��I�u�W�F�N�g
        /// </summary>
        [SerializeField]
        private LineRenderer leftLine = default;

        /// <summary>
        /// �E��p�̐��I�u�W�F�N�g
        /// </summary>
        [SerializeField]
        private LineRenderer rightLine = default;

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
        /// ����p�̒����̑���Ԋu
        /// </summary>
        private float leftrocal = 0.5F;

        /// <summary>
        /// �E��p�̒����̑���Ԋu
        /// </summary>
        private float rightrocal = 0.5F;

        /// <summary>
        /// �w�ʒu�ƌv���_�̗��ԋ���(cm)
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
            ///����
            // ���� �l�����w
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // ���� �e�w
            var leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
            if (leftThumbTip == null)
            {
                Debug.Log("leftThumbTip is null.");
                return;
            }

            var distanceTime = Time.deltaTime;

            // �������Z�o
            var leftDistance = Vector3.Distance(leftIndexTip.position, leftThumbTip.position);

            // cm�ɕϊ�
            leftDistance = leftDistance * 100;

            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                case StemModeSelector.StemMode.Length:
                    // ����`��
                    leftLine.SetPosition(0, leftIndexTip.position);
                    leftLine.SetPosition(1, leftThumbTip.position);
                    leftLine.startWidth = 0.001f;
                    leftLine.endWidth = 0.001f;
                    break;

                // �s�a���[�h
                // 1�ӂł̌s�a���[�h
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // �Ԋu��(���ԋ����~2)cm�����傫����
                    if (leftDistance > (ReleaseLen * 2))
                    {
                        var point = leftDistance - ReleaseLen;

                        Vector3 p1 = (leftIndexTip.position * (leftDistance - point) + leftThumbTip.position * point) / leftDistance;
                        Vector3 p2 = (leftIndexTip.position * point + leftThumbTip.position * (leftDistance - point)) / leftDistance;
                        // �����𑾐��̒����ɕύX
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
            ///�E��
            // �E�� �l�����w
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // �E�� �e�w
            var rightThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
            if (rightThumbTip == null)
            {
                Debug.Log("rightThumbTip is null.");
                return;
            }

            // �������Z�o
            var rightDistance = Vector3.Distance(rightIndexTip.position, rightThumbTip.position);

            // cm�ɕϊ�
            rightDistance = rightDistance * 100;

            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                case StemModeSelector.StemMode.Length:
                    // ����`��
                    rightLine.SetPosition(0, rightIndexTip.position);
                    rightLine.SetPosition(1, rightThumbTip.position);
                    rightLine.startWidth = 0.001f;
                    rightLine.endWidth = 0.001f;
                    break;

                // �s�a���[�h
                // 1�ӂł̌s�a���[�h
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // �Ԋu��(���ԋ����~2)cm�����傫����
                    if (rightDistance > (ReleaseLen * 2))
                    {
                        var point = rightDistance - ReleaseLen;

                        Vector3 p1 = (rightIndexTip.position * (rightDistance - point) + rightThumbTip.position * point) / rightDistance;
                        Vector3 p2 = (rightIndexTip.position * point + rightThumbTip.position * (rightDistance - point)) / rightDistance;
                        // �����𑾐��̒����ɕύX
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

            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                // �s�a���[�h
                case StemModeSelector.StemMode.Length:
                case StemModeSelector.StemMode.Diameter:
                    measuringToolSelector.LineDistance = rightDistance;
                    break;

                // 1�ӂł̌s�a���[�h
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
