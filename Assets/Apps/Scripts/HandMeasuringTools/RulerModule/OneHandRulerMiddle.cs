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
        /// ���`�搧��pGameObject
        /// </summary>
        private GameObject LineManagerObj;

        /// <summary>
        /// ���`�搧��p�X�N���v�gObject
        /// </summary>
        private RulerLineManager LineManager;

        /// <summary>
        /// ���苗���\���p�e�L�X�g
        /// </summary>
        private string LeftRulerText;

        /// <summary>
        /// �E�苗���\���p�e�L�X�g
        /// </summary>
        private string RightRulerText;

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

            Vector3 p1 = leftIndexTip.position;
            Vector3 p2 = leftThumbTip.position;
            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                case StemModeSelector.StemMode.Length:
                    break;

                // �s�a���[�h
                // 1�ӂł̌s�a���[�h
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // �Ԋu��(���ԋ����~2)cm�����傫����
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
            // ����p�v������`��
            LineManager.RulerLineLeftDraw(p1, p2, LeftRulerText);

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


            p1 = rightIndexTip.position;
            p2 = rightThumbTip.position;
            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                case StemModeSelector.StemMode.Length:
                    break;

                // �s�a���[�h
                // 1�ӂł̌s�a���[�h
                case StemModeSelector.StemMode.SingleDiameter:
                case StemModeSelector.StemMode.Diameter:
                    // �Ԋu��(���ԋ����~2)cm�����傫����
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
                RightRulerText = rightDistance.ToString("0.0") + " cm";
                rightrocal = 0.5F;
            }
            // �v������`��
            LineManager.RulerLineDraw(p1, p2, RightRulerText);
        }
    }
}
