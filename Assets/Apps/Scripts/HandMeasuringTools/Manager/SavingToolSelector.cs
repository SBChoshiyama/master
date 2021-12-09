using System.Collections.Generic;
using UnityEngine;

public class SavingToolSelector : MonoBehaviour
{
    /// <summary>
    /// Saveモード
    /// </summary>
    public enum SavingTools
    {
        /// <summary>
        /// タイマー
        /// </summary>
        Timer = 0,

        /// <summary>
        /// ボイスコマンド
        /// </summary>
        VoiceCommand,

        /// <summary>
        /// 写真撮影
        /// </summary>
        PhotoCapture,

        /// <summary>
        /// クロスポインター
        /// </summary>
        Cross
    }

    [SerializeField]
    private List<GameObject> tools = new List<GameObject>();

    private int savingtoolSel;

    // Start is called before the first frame update
    private void Start()
    {
        Initialise();
    }

    /// <summary>
    /// 初回何も設定されていない場合の初期化
    /// </summary>
    private void Initialise()
    {
        // ボイスコマンドモードで起動
        UseVoiceCommandEvent();
    }

    /// <summary>
    /// タイマーモードのオブジェクト表示処理
    /// </summary>
    public void UseTimerEvevt()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.Timer].SetActive(true);
        savingtoolSel = (int)SavingTools.Timer;
    }

    /// <summary>
    /// ボイスコマンドモードのオブジェクト表示処理
    /// </summary>
    public void UseVoiceCommandEvent()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.VoiceCommand].SetActive(true);
        savingtoolSel = (int)SavingTools.VoiceCommand;
    }

    /// <summary>
    /// 写真撮影モードのオブジェクト表示処理
    /// </summary>
    public void UsePhotoCaptureEvent()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
        tools[(int)SavingTools.PhotoCapture].SetActive(true);
        tools[(int)SavingTools.Cross].SetActive(true);
        savingtoolSel = (int)SavingTools.PhotoCapture;
    }

    /// <summary>
    /// 写真撮影モード判定
    /// </summary>
    public bool isPhotoCapture()
    {
        if(savingtoolSel == (int)SavingTools.PhotoCapture)
        {
            return true;
        }
       return false;
    }
}