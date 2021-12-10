using System.Collections;
using HKT;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class RulerLineManager : MonoBehaviour
{
    /// <summary>
    /// �n���h���j�^�[GameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// �n���h���j�^�[Object
    /// </summary>
    private HandMonitor HandMonitor;

    /// <summary>
    /// �����\���e�L�X�g
    /// </summary>
    [SerializeField]
    private TextMesh RulerText = default;

    /// <summary>
    /// �����\���e�L�X�g2
    /// </summary>
    [SerializeField]
    private TextMesh RulerTextLeft = default;

    /// <summary>
    /// ���I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private LineRenderer RulerLine = default;

    /// <summary>
    /// ���I�u�W�F�N�g2
    /// </summary>
    [SerializeField]
    private LineRenderer RulerLineLeft = default;
    
    // Start is called before the first frame update
    void Start()
    {
        // �n���h���j�^�[�I�u�W�F�N�g
        HandMonitorObj = GameObject.Find("HandMonitor");
        HandMonitor = HandMonitorObj.GetComponent<HandMonitor>();

        RulerLineInit();
        // �n���h���C���\���ɂ���
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
        // �n���h���C���\���ɂ���
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// �v����������
    /// </summary>
    public void RulerLineInit()
    {
        RulerLine.SetPosition(0, Vector3.zero);
        RulerLine.SetPosition(1, Vector3.zero);
        RulerLineErase();

        RulerLineLeft.SetPosition(0, Vector3.zero);
        RulerLineLeft.SetPosition(1, Vector3.zero);
        RulerLineLeftErase();
    }

    /// <summary>
    /// �v�����̕`��
    /// </summary>
    public void RulerLineDraw(Vector3 pos1, Vector3 pos2, string txtdat)
    {
        if (HandMonitor.isHandTracking())
        {
            // ������m���Ă���Ƃ������`��
            RulerLine.gameObject.SetActive(true);
            RulerLine.SetPosition(0, pos1);
            RulerLine.SetPosition(1, pos2);
            RulerLine.startWidth = 0.002f;
            RulerLine.endWidth = 0.002f;
            RulerLine.enabled = true;
            RulerText.text = txtdat;
            var textpos = (pos1 + pos2) / 2;
            textpos.y += 0.05f;
            RulerText.transform.position = textpos;
        }
        else
        {
            RulerText.text = "";
            RulerLine.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// ����p�v�����̕`��2
    /// </summary>
    public void RulerLineLeftDraw(Vector3 pos1, Vector3 pos2, string txtdat)
    {
        if (HandMonitor.isLeftHandTracking())
        {
            // ������m���Ă���Ƃ������`��
            RulerLineLeft.gameObject.SetActive(true);
            RulerLineLeft.SetPosition(0, pos1);
            RulerLineLeft.SetPosition(1, pos2);
            RulerLineLeft.startWidth = 0.002f;
            RulerLineLeft.endWidth = 0.002f;
            RulerLineLeft.enabled = true;
            RulerTextLeft.text = txtdat;
            var textpos = (pos1 + pos2) / 2;
            textpos.y += 0.05f;
            RulerTextLeft.transform.position = textpos;
        }
        else
        {
            RulerTextLeft.text = "";
            RulerLineLeft.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// �v��������
    /// </summary>
    public void RulerLineErase()
    {
        RulerLine.gameObject.SetActive(false);
        RulerText.text = "";
    }

    /// <summary>
    /// ����p�v��������
    /// </summary>
    public void RulerLineLeftErase()
    {
        RulerLineLeft.gameObject.SetActive(false);
        RulerTextLeft.text = "";
    }
}
