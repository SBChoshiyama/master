using HKT;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MeasureController : MonoBehaviour
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
    /// 設定メニュー開始ボタンGameObject
    /// </summary>
    private GameObject SettingStartBtnObj;

    /// <summary>
    /// 苗番号用GameObject
    /// </summary>
    private GameObject PlantNoObj;

    /// <summary>
    /// 苗番号用Object
    /// </summary>
    private PlantNoManager PlantNo;

    /// <summary>
    /// 苗番号入力画面GameObject
    /// </summary>
    private GameObject PlantNoPlateObj;

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

    /// <summary>
    ///  PlantNoテキスト
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoText = default;

    /// <summary>
    ///  記録窓茎長テキスト
    /// </summary>
    [SerializeField]
    private TextMesh RecStemLengthText = default;
    /// <summary>
    ///  記録窓茎径テキスト
    /// </summary>
    [SerializeField]
    private TextMesh RecStemDiamText = default;

    /// <summary>
    /// 茎長の測定結果
    /// </summary>
    private float stemLength;

    /// <summary>
    /// 茎径の測定結果
    /// </summary>
    private float stemDiameter;

    /// <summary>
    /// 記録表示ボタン用GameObject
    /// </summary>
    private GameObject RecordBtnObj;
    /// <summary>

    /// <summary>
    /// Voice用GameObject
    /// </summary>
    private GameObject VoiceObj;

    /// VoiceObject
    /// </summary>
    private VoiceCommand Voice;

    /// <summary>
    /// 記録表示用GameObject
    /// </summary>
    private GameObject RecordSlateObj;

    /// <summary>
    /// LengthTwoHand GameObject
    /// </summary>
    private GameObject LengthTwoHandObj;
    /// <summary>
    /// 苗番号用Object
    /// </summary>
    private InteractiveMeshCursor LengthTwoHand;

    /// <summary>
    /// 初期化完了フラグ
    /// </summary>
    private bool initflg = false;

    // Start is called before the first frame update
    void Start()
    {
        // ハンドレイを非表示にする
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        // 苗番号オブジェクト
        PlantNoObj = GameObject.Find("PlantNoManager");
        PlantNo = PlantNoObj.GetComponent<PlantNoManager>();

        // 苗番号入力画面GameObject
        PlantNoPlateObj = GameObject.Find("PlantNoInputPlate");

        // 設定メニューオブジェクト
        SettingMenuObj = GameObject.Find("SettingMenuPanel");

        // 設定メニュー表示場所指定オブジェクト
        SettingMenuAnchorObj = GameObject.Find("SettingMenuAnchor");

        /// 設定メニュー開始ボタンオブジェクト
        SettingStartBtnObj = GameObject.Find("SettingStartButton");

        // メジャーツール切替オブジェクト
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // 茎測定モードオブジェクト
        StemModeObj = GameObject.Find("StemModeSelector");
        StemMode = StemModeObj.GetComponent<StemModeSelector>();

        // Saveモードオブジェクト
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        // voice
        VoiceObj = GameObject.Find("VoiceCommand");
        Voice = VoiceObj.GetComponent<VoiceCommand>();

        // 記録表示用ボードオブジェクト
        RecordSlateObj = GameObject.Find("RecordSlate");

        // 記録表示ボタン用オブジェクト
        RecordBtnObj = GameObject.Find("RecordButton");

        // 記録表示ボタン用オブジェクト
        LengthTwoHandObj = GameObject.Find("LengthTwoHands");

        MeasureInit();
    }

    // Update is called once per frame
    void Update()
    {
        StemRecordButtonCheck();
    }

    /// <summary>
    /// 計測コントローラ初期化
    /// </summary>
    public void MeasureInit()
    {
        // 計測無しモードで初期化
        NoneSelect();
        // 設定メニューは最初は非表示に設定
        SettingMenuObj.SetActive(false);
        // 設定開始ボタン非表示
        SettingStartBtnObj.SetActive(false);
        // 記録表示ボタン非表示
        RecordBtnObj.SetActive(false);
        // 記録表示用ボード非表示
        RecordSlateObj.SetActive(false);
        // ローカル変数クリア
        Voice.clearDistance();
        // 初期化完了
        initflg = true;
    }

    /// <summary>
    /// 設定メニュー有効化
    /// </summary>
    public void SettingMenuEnable()
    {
        // 茎長計測：両手モード選択で初期化
        LengthTwoHandsSelect();
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();
        // 設定メニューは最初は非表示に設定
        //SettingMenuObj.SetActive(false);
        // 設定開始ボタン表示
        SettingStartBtnObj.SetActive(true);
        PlantNoText.text = "苗番号:" + PlantNo.GetPlantNo();
    }
    /// <summary>
    /// 設定メニュー表示
    /// </summary>
    public void SettingMenuDisplay()
    {
        // アンカー座標・角度を取得
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

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
        if (initflg)
        {
            Debug.Log("dousite koko deru");
            StemMode.UseStemLength();
            MeasuringTool.UseOneHandRuler();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "茎長計測(片手)";
        }
    }

    /// <summary>
    /// 茎長計測：両手モード選択時
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        if (initflg)
        {
            StemMode.UseStemLength();
            MeasuringTool.UseTwoHandsRuler();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "茎長計測(両手)";
        }
    }

    /// <summary>
    /// 茎径計測：両手モード選択時
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        if (initflg)
        {
            StemMode.UseStemSimpleDiameter();
            MeasuringTool.UseTwoHandsRuler();
            MeasuringTool.MeasurMiddleModeOn();
            SaveMode.UseVoiceCommandEvent();
            DistanceText.text = "茎径計測(両手)";
        }
    }

    /// <summary>
    /// 茎径計測：画像解析モード選択時
    /// </summary>
    public void DiameterGraphicSelect()
    {
        if (initflg)
        {
            MeasuringTool.UseHandRulerOFF();
            SaveMode.UsePhotoCaptureEvent();
            DistanceText.text = "茎径計測(画像解析)";
        }
    }

    /// <summary>
    /// 計測無しモード(計測関係の表示をしたくないとき)
    /// </summary>
    public void NoneSelect()
    {
        MeasuringTool.UseHandRulerOFF();
        SaveMode.UseVoiceCommandEvent();
        DistanceText.text = "";
        PlantNoText.text = "";
    }

    /// <summary>
    /// 茎長のセット
    /// </summary>
    public void SetStemLength(float data)
    {
        stemLength = data;
        StemRecordButtonCheck();
        Debug.Log($"stemLength = {data}cm");
    }

    /// <summary>
    /// 茎径のセット
    /// </summary>
    public void SetStemDiameter(float data)
    {
        stemDiameter = data;
        Debug.Log($"stemDiameter = {data}cm");
        StemRecordButtonCheck();
    }

    /// <summary>
    /// 記録表示ボタン表示チェック
    /// </summary>
    public void StemRecordButtonCheck()
    {
        if (Voice.IsVoiceTriggerOn())
        {
            stemLength = Voice.getStemLength();
            stemDiameter = Voice.getStemDiameter();
            Debug.Log($"stemLength = {stemLength}cm , stemDiameter = {stemDiameter}cm");
            if ((stemLength > 0) && (stemDiameter > 0))
            {
                RecordBtnObj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 記録結果表示
    /// </summary>
    public void RecordResultDisplay()
    {
        // アンカー座標・角度を取得
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

        RecStemLengthText.text = "茎長：" + stemLength.ToString("0.0") + " cm";
        RecStemDiamText.text = "茎径：" + stemDiameter.ToString("0.0") + " cm";
        //　記録表示の表示位置は開始ボタン押下時のアンカー位置とする
        RecordSlateObj.transform.SetPositionAndRotation(pos, rot);


        // 記録表示ボタン非表示
        RecordBtnObj.SetActive(false);

        // 記録表示用ボード表示
        RecordSlateObj.SetActive(true);
    }

    /// <summary>
    /// 計測結果確定
    /// </summary>
    public void SetRecords()
    {
        // 計測結果送信処理(TBD)


        // 計測結果クリア
        stemLength = 0;
        stemDiameter = 0;

        // 初期化
        MeasureInit();

        // 苗番号画面表示
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// 苗番号入力へ戻る
    /// </summary>
    public void ReturnKey()
    {
        // 計測結果クリア
        stemLength = 0;
        stemDiameter = 0;

        // 初期化
        MeasureInit();

        // 苗番号画面表示
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// 記録表示閉じるキー
    /// </summary>
    public void RecordPlateCloseKey()
    {
        // 記録表示ボタン表示
        RecordBtnObj.SetActive(true);
    }
}
