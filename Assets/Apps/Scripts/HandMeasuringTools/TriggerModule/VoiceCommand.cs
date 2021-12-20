using HKT;
using System;
using UnityEngine;

public class VoiceCommand : MonoBehaviour
{
    /// <summary>
    /// �v���c�[���Z���N�^GameObject
    /// </summary>
    private GameObject MeasuringToolSelectorObj;

    /// <summary>
    /// �v���c�[���Z���N�^GameObject
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
    /// �n���h���j�^�[GameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// �n���h���j�^�[Object
    /// </summary>
    private HandMonitor HandMonitor;

    /// <summary>
    /// �v���R���g���[��GameObject
    /// </summary>
    private GameObject MeasureControlObj;

    /// <summary>
    /// �v���R���g���[��Object
    /// </summary>
    private RulerLineManager MeasureControl;

    [SerializeField]
    private TextMesh DistanceText = default;

    private bool IsOneSelectDiameter;
    private float InnerDistance;
    private float LongDis;
    private float MinDis;
    private bool VoiceTriggerOn = false;

    private float locallength;
    private float localdiameter;

    // Start is called before the first frame update
    private void Start()
    {
        // �n���h���j�^�[�I�u�W�F�N�g
        HandMonitorObj = GameObject.Find("HandMonitor");
        HandMonitor = HandMonitorObj.GetComponent<HandMonitor>();

        // �v���c�[���Z���N�^�I�u�W�F�N�g
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();

        // �s���[�h�I��p�I�u�W�F�N�g
        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        // �v���R���g���[���I�u�W�F�N�g
        MeasureControlObj = GameObject.Find("RulerLineManager");
        MeasureControl = StemModeSelectorObj.GetComponent<RulerLineManager>();
        
        locallength = 0;
        localdiameter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Save�R�}���h���{����
    /// </summary>
    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;

        if (HandMonitor.isHandTracking())
        {
            if (stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Diameter)
            {
                // �v�Z�������{
                CheckStemDiameter(dis);
            }
            else
            {
                // ���b�Z�[�W�\������
                ShowDistanceText(dis);
            }
        }
        else
        {
            DistanceText.text = "�G���[:��̌��o���s";
        }
    }

    /// <summary>
    /// Clear�R�}���h���{����
    /// </summary>
    public void ClearCommandEvent()
    {
        Debug.Log("ClearEvent");

        // �������������{
        IsOneSelectDiameter = false;
        DistanceText.text = "No Distance";
    }

    /// <summary>
    /// �v�����ʕ\������
    /// </summary>
    /// <param name="dis">�\������v������</param>
    private void ShowDistanceText(float dis)
    {
        if(stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Length)
        {
            Debug.Log($"�s���̒��� = {dis}cm");
            DistanceText.text = "�s���̒��� = " + dis.ToString("0.0") + " cm";
            // �s���̐ݒ�
            locallength = dis;
            VoiceTriggerOn = true;
        }
        else
        {
            Debug.Log($"�s�a�̉~�� = {dis}cm");
            DistanceText.text = "�s�a�̉~�� = " + dis.ToString("0.0") + " cm";
            // �s�a�̐ݒ芮��
            localdiameter = dis;
            VoiceTriggerOn = true;
        }
    }

    /// <summary>
    /// �ȉ~�^�s�̌v������
    /// </summary>
    /// <param name="dis">�v�������ӂ̒���</param>
    private void CheckStemDiameter(float dis)
    {
        if (!IsOneSelectDiameter)
        {
            InnerDistance = dis;
            Debug.Log($"1��ڂ̋��� = {dis}cm");
            DistanceText.text = "1��ڂ̋���" + dis.ToString("0.0") + " cm";
            IsOneSelectDiameter = true;
        }
        else
        {
            Debug.Log($"2��ڂ̋��� = {dis}cm");
            DistanceText.text = "2��ڂ̋���" + dis.ToString("0.0") + " cm";

            if (InnerDistance > dis)
            {
                LongDis = InnerDistance;
                MinDis = dis;
            }
            else
            {
                LongDis = dis;
                MinDis = InnerDistance;
            }

            // ���a�ő��肵�Ă��邽�߁A���a�ɒu������
            LongDis = LongDis / 2;
            MinDis = MinDis / 2;

            // �ȉ~�̉~���̋ߎ��v�Z���ɑ��
            var a = (float)(Math.PI * (LongDis + MinDis));
            var b = (LongDis - MinDis) / (LongDis + MinDis);
            var c = 3 * b * b;
            var d = (float)(10 + Math.Sqrt(4 - c));

            var total = a * (1 + (c / d));

            // ���b�Z�[�W�\������
            ShowDistanceText(total);
            IsOneSelectDiameter = false;
        }
    }

    /// <summary>
    /// �����g���K�[���m
    /// </summary>
    public bool IsVoiceTriggerOn()
    {
        bool ret = VoiceTriggerOn;
        VoiceTriggerOn = false;
        return ret;
    }

    /// <summary>
    /// �������N���A
    /// </summary>
    public void clearDistance()
    {
        locallength = 0;
        localdiameter = 0;
    }

    /// <summary>
    /// �s���擾
    /// </summary>
    public float getStemLength()
    {
        return locallength;
    }

    /// <summary>
    /// �s�a�擾
    /// </summary>
    public float getStemDiameter()
    {
        return localdiameter;
    }

}