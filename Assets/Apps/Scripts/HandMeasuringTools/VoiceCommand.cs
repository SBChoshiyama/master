using HKT;
using System;
using UnityEngine;

public class VoiceCommand : MonoBehaviour
{
    private GameObject MeasuringToolSelectorObj;
    private MeasuringToolSelector measuringToolSelector;

    /// <summary>
    /// 茎モード選択用GameObject
    /// </summary>
    private GameObject StemModeSelectorObj;

    /// <summary>
    /// 茎モード選択用スクリプトObject
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
            // 計算処理実施
            CheckStemDiameter(dis);
        }
        else
        {
            // メッセージ表示処理
            ShowDistanceText(dis);
        }
    }

    /// <summary>
    /// 計測結果表示処理
    /// </summary>
    /// <param name="dis">表示する計測結果</param>
    private void ShowDistanceText(float dis)
    {
        Debug.Log($"距離 = {dis}cm");
        DistanceText.text = dis.ToString("0.0") + " cm";
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
}