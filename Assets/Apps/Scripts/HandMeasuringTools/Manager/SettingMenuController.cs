using HKT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuController : MonoBehaviour
{
    /// <summary>
    /// 設定メニューパネル用GameObject
    /// </summary>
    private GameObject SettingMenuObj;

    /// <summary>
    /// 設定メニュー表示場所指定GameObject
    /// </summary>
    private GameObject SettingMenuAnchorObj;

    /// <summary>
    /// メジャーツール切替GameObject
    /// </summary>
    private GameObject MeasuringToolObj;

    /// <summary>
    /// メジャーツール切替Object
    /// </summary>
    private MeasuringToolSelector MeasuringTool;

    /// <summary>
    /// 茎測定モードGameObject
    /// </summary>
    private GameObject StemModeObj;

    /// <summary>
    /// 茎測定モードObject
    /// </summary>
    private StemModeSelector StemMode;

    /// <summary>
    ///  SaveモードGameObject
    /// </summary>
    private GameObject SaveModeObj;

    /// <summary>
    ///  SaveモードObject
    /// </summary>
    private SavingToolSelector SaveMode;

    /// <summary>
    ///  Distanceテキスト
    /// </summary>
    [SerializeField]
    private TextMesh DistanceText = default;

    // Start is called before the first frame update
    void Start()
    {
        // 設定メニューオブジェクト
        SettingMenuObj = GameObject.Find("SettingMenuPanel");

        // 設定メニュー表示場所指定オブジェクト
        SettingMenuAnchorObj = GameObject.Find("SettingMenuAnchor");

        // メジャーツール切替オブジェクト
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // 茎測定モードオブジェクト
        StemModeObj = GameObject.Find("StemModeSelector");
        StemMode = StemModeObj.GetComponent<StemModeSelector>();

        // Saveモードオブジェクト
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        DistanceText.text = "茎長計測(両手)";
        // 設定メニューは最初は非表示に設定
        SettingMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 設定メニュー表示
    /// </summary>
    public void SettingMenuDisplay()
    {
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;
        Debug.Log("rot:" + rot);
        //　設定メニューの表示位置は開始ボタン押下時のアンカー位置とする
        SettingMenuObj.transform.SetPositionAndRotation(pos, rot);

        // 設定メニュー表示
        SettingMenuObj.SetActive(true);
    }

    /// <summary>
    /// 茎長計測：片手モード選択時
    /// </summary>
    public void LengthOneHandSelect()
    {
        StemMode.UseStemLength();
        MeasuringTool.UseOneHandRuler();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "茎長計測(片手)";
    }

    /// <summary>
    /// 茎長計測：両手モード選択時
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        StemMode.UseStemLength();
        MeasuringTool.UseTwoHandsRuler();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "茎長計測(両手)";
    }

    /// <summary>
    /// 茎径計測：両手モード選択時
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        StemMode.UseStemSimpleDiameter();
        MeasuringTool.UseTwoHandsRuler();
        MeasuringTool.MeasurMiddleModeOn();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "茎径計測(両手)";
    }

    /// <summary>
    /// 茎径計測：画像解析モード選択時
    /// </summary>
    public void DiameterGraphicSelect()
    {
        MeasuringTool.UseHandRulerOFF();
        SaveMode.UsePhotoCaptureEvent();
        DistanceText.text = "茎径計測(画像解析)";
    }

}
