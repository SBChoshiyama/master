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
        /// �����\���p�e�L�X�g
        /// </summary>
        private string RulerText;

        /// <summary>
        /// �����̑���Ԋu
        /// </summary>
        float RocalTime = 0.5F;

        /// <summary>
        /// �w�ʒu�ƌv���_�̗��ԋ���(cm)
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

        void Update()
        {
            // ����
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // �E��
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // �������Z�o
            var distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
            // cm�ɕϊ�
            distance = distance * 100;
            Vector3 p1 = leftIndexTip.position;
            Vector3 p2 = rightIndexTip.position;

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

            // �p�u���b�N�ϐ��ɕۑ�
            switch (stemModeSelector.InnerStemMode)
            {
                // �s�����[�h
                // �s�a���[�h
                case StemModeSelector.StemMode.Length:
                case StemModeSelector.StemMode.Diameter:
                    // �����_1���Ŏl�̌ܓ�
                    measuringToolSelector.LineDistance = ((float)Math.Round(distance * 10)) / 10;
                    break;

                // 1�ӂł̌s�a���[�h
                case StemModeSelector.StemMode.SingleDiameter:
                    // �s�a�͇o�P�ʂɕϊ�(�~10)
                    measuringToolSelector.LineDistance = (float)Math.Round((distance * 3.14 * 10), MidpointRounding.AwayFromZero);
                    break;
            }

            RocalTime -= Time.deltaTime;
            if (RocalTime <= 0)
            {
                RulerText = distance.ToString("0.0") + " cm";
                RocalTime = 0.5F;
            }

            // ����`��
            LineManager.RulerLineDraw(p1, p2, RulerText);
        }
    }
}
