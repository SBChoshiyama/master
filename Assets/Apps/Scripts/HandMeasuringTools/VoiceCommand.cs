using HKT;
using System;
using UnityEngine;

public class VoiceCommand : MonoBehaviour
{
    private GameObject MeasuringToolSelectorObj;
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// �s���[�h�I��pGameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// �s���[�h�I��p�X�N���v�gObject
    /// </summary>
    private StemModeSelector stemModeSelector;

    [SerializeField]
    private TextMesh DistanceText = default;

    private bool IsOneSelectDiameter;
    private float InnerDistance;
    private float LongDis;
    private float MinDis;

    // Start is called before the first frame update
    private void Start()
    {
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();

        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;

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

    /// <summary>
    /// �v�����ʕ\������
    /// </summary>
    /// <param name="dis">�\������v������</param>
    private void ShowDistanceText(float dis)
    {
        Debug.Log($"���� = {dis}cm");
        DistanceText.text = dis.ToString("0.0") + " cm";
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
}