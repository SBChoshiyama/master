using HKT;
using System;
using UnityEngine;

public class VoiceCommand : MonoBehaviour
{
    /// <summary>
    /// 計測ツールセレクタGameObject
    /// </summary>
    private GameObject MeasuringToolSelectorObj;

    /// <summary>
    /// 計測ツールセレクタGameObject
    /// </summary>
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// 茎モード選択用GameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// 茎モード選択用スクリプトObject
    /// </summary>
    private StemModeSelector stemModeSelector;

    /// <summary>
    /// ハンドモニターGameObject
    /// </summary>
    private GameObject HandMonitorObj;

    /// <summary>
    /// ハンドモニターObject
    /// </summary>
    private HandMonitor HandMonitor;

    /// <summary>
    /// 計測コントローラGameObject
    /// </summary>
    private GameObject MeasureControlObj;

    /// <summary>
    /// 計測コントローラObject
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
        // ハンドモニターオブジェクト
        HandMonitorObj = GameObject.Find("HandMonitor");
        HandMonitor = HandMonitorObj.GetComponent<HandMonitor>();

        // 計測ツールセレクタオブジェクト
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();

        // 茎モード選択用オブジェクト
        StemModeSelectorObj = GameObject.Find("StemModeSelector");
        stemModeSelector = StemModeSelectorObj.GetComponent<StemModeSelector>();

        // 計測コントローラオブジェクト
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
    /// Saveコマンド実施処理
    /// </summary>
    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;

        if (HandMonitor.isHandTracking())
        {
            if (stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Diameter)
            {
                // 計算処理実施
                CheckStemDiameter(dis);
            }
            else
            {
                // メッセージ表示処理
                ShowDistanceText(dis);
            }
        }
        else
        {
            DistanceText.text = "エラー:手の検出失敗";
        }
    }

    /// <summary>
    /// Clearコマンド実施処理
    /// </summary>
    public void ClearCommandEvent()
    {
        Debug.Log("ClearEvent");

        // 初期化処理実施
        IsOneSelectDiameter = false;
        DistanceText.text = "No Distance";
    }

    /// <summary>
    /// 計測結果表示処理
    /// </summary>
    /// <param name="dis">表示する計測結果</param>
    private void ShowDistanceText(float dis)
    {
        if(stemModeSelector.InnerStemMode == StemModeSelector.StemMode.Length)
        {
            Debug.Log($"茎長の長さ = {dis}cm");
            DistanceText.text = "茎長の長さ = " + dis.ToString("0.0") + " cm";
            // 茎長の設定
            locallength = dis;
            VoiceTriggerOn = true;
        }
        else
        {
            Debug.Log($"茎径の円周 = {dis}cm");
            DistanceText.text = "茎径の円周 = " + dis.ToString("0.0") + " cm";
            // 茎径の設定完了
            localdiameter = dis;
            VoiceTriggerOn = true;
        }
    }

    /// <summary>
    /// 楕円型茎の計測処理
    /// </summary>
    /// <param name="dis">計測した辺の長さ</param>
    private void CheckStemDiameter(float dis)
    {
        if (!IsOneSelectDiameter)
        {
            InnerDistance = dis;
            Debug.Log($"1回目の距離 = {dis}cm");
            DistanceText.text = "1回目の距離" + dis.ToString("0.0") + " cm";
            IsOneSelectDiameter = true;
        }
        else
        {
            Debug.Log($"2回目の距離 = {dis}cm");
            DistanceText.text = "2回目の距離" + dis.ToString("0.0") + " cm";

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

            // 直径で測定しているため、半径に置き換え
            LongDis = LongDis / 2;
            MinDis = MinDis / 2;

            // 楕円の円周の近似計算式に代入
            var a = (float)(Math.PI * (LongDis + MinDis));
            var b = (LongDis - MinDis) / (LongDis + MinDis);
            var c = 3 * b * b;
            var d = (float)(10 + Math.Sqrt(4 - c));

            var total = a * (1 + (c / d));

            // メッセージ表示処理
            ShowDistanceText(total);
            IsOneSelectDiameter = false;
        }
    }

    /// <summary>
    /// 音声トリガー検知
    /// </summary>
    public bool IsVoiceTriggerOn()
    {
        bool ret = VoiceTriggerOn;
        VoiceTriggerOn = false;
        return ret;
    }

    /// <summary>
    /// 距離情報クリア
    /// </summary>
    public void clearDistance()
    {
        locallength = 0;
        localdiameter = 0;
    }

    /// <summary>
    /// 茎長取得
    /// </summary>
    public float getStemLength()
    {
        return locallength;
    }

    /// <summary>
    /// 茎径取得
    /// </summary>
    public float getStemDiameter()
    {
        return localdiameter;
    }

}