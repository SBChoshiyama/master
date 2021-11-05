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
        /// アイトラッキング
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
    /// 初回何も設定されていない場合の初期化
    /// </summary>
    private void Initialise()
    {
        // 何も起動してないモードで起動
        UseEyeTrackingEvent();
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
    }

    /// <summary>
    /// アイトラッキングモードのオブジェクト表示処理
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