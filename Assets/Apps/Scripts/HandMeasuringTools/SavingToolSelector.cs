using System.Collections.Generic;
using UnityEngine;

public class SavingToolSelector : MonoBehaviour
{
    /// <summary>
    /// Save���[�h
    /// </summary>
    public enum SavingTools
    {
        /// <summary>
        /// �^�C�}�[
        /// </summary>
        Timer = 0,

        /// <summary>
        /// �{�C�X�R�}���h
        /// </summary>
        VoiceCommand,

        /// <summary>
        /// �A�C�g���b�L���O
        /// </summary>
        EyeTracking
    }

    [SerializeField]
    private List<GameObject> tools = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        Initialise();
    }

    /// <summary>
    /// ���񉽂��ݒ肳��Ă��Ȃ��ꍇ�̏�����
    /// </summary>
    private void Initialise()
    {
        // �����N�����ĂȂ����[�h�ŋN��
        UseEyeTrackingEvent();
    }

    /// <summary>
    /// �^�C�}�[���[�h�̃I�u�W�F�N�g�\������
    /// </summary>
    public void UseTimerEvevt()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.Timer].SetActive(true);
    }

    /// <summary>
    /// �{�C�X�R�}���h���[�h�̃I�u�W�F�N�g�\������
    /// </summary>
    public void UseVoiceCommandEvent()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.VoiceCommand].SetActive(true);
    }

    /// <summary>
    /// �A�C�g���b�L���O���[�h�̃I�u�W�F�N�g�\������
    /// </summary>
    public void UseEyeTrackingEvent()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        //tools[(int)SavingTools.EyeTracking].SetActive(true);
    }
}