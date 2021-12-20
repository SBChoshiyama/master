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
        /// �ʐ^�B�e
        /// </summary>
        PhotoCapture,

        /// <summary>
        /// �N���X�|�C���^�[
        /// </summary>
        Cross
    }

    [SerializeField]
    private List<GameObject> tools = new List<GameObject>();

    /// <summary>
    /// Save���[�h
    /// </summary>
    public SavingTools savingtoolSel;

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
        // �{�C�X�R�}���h���[�h�ŋN��
        UseVoiceCommandEvent();
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
        savingtoolSel = SavingTools.Timer;
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
        savingtoolSel = SavingTools.VoiceCommand;
    }

    /// <summary>
    /// �ʐ^�B�e���[�h�̃I�u�W�F�N�g�\������
    /// </summary>
    public void UsePhotoCaptureEvent()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.PhotoCapture].SetActive(true);
        tools[(int)SavingTools.Cross].SetActive(true);
        savingtoolSel = SavingTools.PhotoCapture;
    }

    /// <summary>
    /// �ʐ^�B�e���[�h����
    /// </summary>
    public bool isPhotoCapture()
    {
        if(savingtoolSel == SavingTools.PhotoCapture)
        {
            return true;
        }
       return false;
    }
}