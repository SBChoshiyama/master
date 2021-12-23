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
    /// ハンドモニタ用GameObject
    /// </summary>
    private GameObject HandMonitorObj;

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
    /// 線描画制御用GameObject
    /// </summary>
    private GameObject LineManagerObj;

    /// <summary>
    /// 線描画制御用スクリプトObject
    /// </summary>
    private RulerLineManager LineManager;

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
    ///  記録窓PlantNoテキスト
    /// </summary>
    [SerializeField]
    private TextMesh RecPlantNoText = default;

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
    private GameObject DisplayResultBtnObj;

    /// <summary>
    /// ReturnTopボタン用GameObject
    /// </summary>
    private GameObject ReturnTopBtnObj;

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
    private GameObject MesuringResultSlateObj;

    /// <summary>
    /// LengthTwoHand GameObject
    /// </summary>
    private GameObject LengthTwoHandObj;
    /// <summary>
    /// 苗番号用Object
    /// </summary>
    private InteractiveMeshCursor LengthTwoHand;

    /// <summary>
    /// 初期表示テキスト
    /// </summary>
    private string DefaultText;

    /// <summary>
    /// 初期化フラグ
    /// </summary>
    private bool InitFlg = false;

    /// <summary>
    /// 起動初回のみ実行用
    /// </summary>
    private bool isFirstFlg = false;

    // Start is called before the first frame update
    void Start()
    {
        // ハンドレイを非表示にする
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

        // ハンドモニタGameObject
        HandMonitorObj = GameObject.Find("HandStatus");

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

        // 線描画オブジェクト
        LineManagerObj = GameObject.Find("RulerLineManager");
        LineManager = LineManagerObj.GetComponent<RulerLineManager>();

        // voice
        VoiceObj = GameObject.Find("VoiceCommand");
        Voice = VoiceObj.GetComponent<VoiceCommand>();

        // 記録表示用ボードオブジェクト
        MesuringResultSlateObj = GameObject.Find("MesuringResultSlate");

        // 記録表示ボタン用オブジェクト
        DisplayResultBtnObj = GameObject.Find("DisplayResultButton");

        // ReturnTopボタン用オブジェクト
        ReturnTopBtnObj = GameObject.Find("ReturnTopButton");
        
        // 初期表示用テキスト
        DefaultText = "";

        MeasureInit();

        // 初期化フラグset
        InitFlg = true;

        // 初回フラグset
        isFirstFlg = true;

    }

    // Update is called once per frame
    void Update()
    {
        // 音声検知があれば更新
        if (Voice.IsVoiceTriggerOn())
        {
            MesuringResultButtonCheck();
        }
    }

    /// <summary>
    /// 計測コントローラ初期化
    /// </summary>
    public void MeasureInit()
    {
        // 設定メニューは最初は非表示に設定
        SettingMenuObj.SetActive(false);
        // 設定開始ボタン非表示
        SettingStartBtnObj.SetActive(false);
        // 記録表示用ボード非表示
        MesuringResultSlateObj.SetActive(false);
        // 記録表示ボタン非表示
        DisplayResultBtnObj.SetActive(false);
        // ReturnTopボタン非表示
        ReturnTopBtnObj.SetActive(false);
        // ローカル変数クリア
        Voice.clearDistance();

        // 計測無しモードで起動
        NoneSelect();

        // まだ測定ツールは停止
        MeasureStop();

        // ハンドモニタ無効
        HandMonitorObj.SetActive(false);
    }

    /// <summary>
    /// 設定メニュー有効化
    /// </summary>
    public void SettingMenuEnable()
    {
        // 初回限定処理
        if (isFirstFlg)
        {
            // 茎長計測：両手モード選択で初期化
            LengthTwoHandsSelect();
            isFirstFlg = false;
        }
        // 測定ツールは設定
        MeasuringTool.MeasurToolSet();
        // 設定開始ボタン表示
        SettingStartBtnObj.SetActive(true);
        DistanceText.text = DefaultText;
        PlantNoText.text = "苗番号:" + PlantNo.GetPlantNo();
        // 音声関係のオブジェクト再表示
        SaveMode.VoiceObjRedisplay();
        // ハンドモニタ有効
        HandMonitorObj.SetActive(true);
        // 記録表示ボタン表示チェック
        MesuringResultButtonCheck();
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
        // 初期化完了しないと処理しない
        if (InitFlg)
        {
            // 設定登録
            StemMode.UseStemLength();
            MeasuringTool.UseOneHandRuler();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "茎長計測(片手)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// 茎長計測：両手モード選択時
    /// </summary>
    public void LengthTwoHandsSelect()
    {
        // 初期化完了しないと処理しない
        if (InitFlg)
        {
            // 設定登録
            StemMode.UseStemLength();
            MeasuringTool.UseTwoHandsRuler();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "茎長計測(両手)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// 茎径計測：両手モード選択時
    /// </summary>
    public void DiameterTwoHandsSelect()
    {
        // 初期化完了しないと処理しない
        if (InitFlg)
        {
            // 設定登録
            StemMode.UseStemSimpleDiameter();
            MeasuringTool.UseTwoHandsRuler();
            MeasuringTool.MeasurMiddleModeOn();
            SaveMode.UseVoiceCommandEvent();
            DefaultText = "茎径計測(両手)";
            DistanceText.text = DefaultText;
        }
    }

    /// <summary>
    /// 茎径計測：画像解析モード選択時
    /// </summary>
    public void DiameterGraphicSelect()
    {
        if (InitFlg)
        {
            MeasuringTool.UseHandRulerOFF();
            SaveMode.UsePhotoCaptureEvent();
            DefaultText = "茎径計測(画像解析)";
            DistanceText.text = DefaultText;
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
    /// 計測停止(計測関係の表示をしたくないとき)
    /// </summary>
    public void MeasureStop()
    {
        DistanceText.text = "";
        PlantNoText.text = "";
        MeasuringTool.MeasurToolOff();
    }

    /// <summary>
    /// 茎長のセット
    /// </summary>
    public void SetStemLength(float data)
    {
        stemLength = data;
        MesuringResultButtonCheck();
        Debug.Log($"stemLength = {data}cm");
    }

    /// <summary>
    /// 茎径のセット
    /// </summary>
    public void SetStemDiameter(float data)
    {
        stemDiameter = data;
        Debug.Log($"stemDiameter = {data}cm");
        MesuringResultButtonCheck();
    }

    /// <summary>
    /// 計測結果表示ボタン表示チェック
    /// </summary>
    public void MesuringResultButtonCheck()
    {
        stemLength = Voice.getStemLength();
        stemDiameter = Voice.getStemDiameter();
        Debug.Log($"stemLength = {stemLength}cm , stemDiameter = {stemDiameter}cm");
        if ((stemLength > 0) && (stemDiameter > 0))
        {
            // ReturnTopボタン非表示
            ReturnTopBtnObj.SetActive(false);
            // 記録表示ボタン表示
            DisplayResultBtnObj.SetActive(true);
        }
        else
        {
            // 記録表示ボタン非表示
            DisplayResultBtnObj.SetActive(false);
            // ReturnTopボタン表示
            ReturnTopBtnObj.SetActive(true);
        }
    }

    /// <summary>
    /// 記録結果表示
    /// </summary>
    public void MesuringResultDisplay()
    {
        // アンカー座標・角度を取得
        var pos = SettingMenuAnchorObj.transform.position;
        var rot = SettingMenuAnchorObj.transform.rotation;

        RecPlantNoText.text = "苗番号:" + PlantNo.GetPlantNo();
        RecStemLengthText.text = "茎長：" + stemLength.ToString("0.0") + " cm";
        RecStemDiamText.text = "茎径：" + stemDiameter.ToString("0") + " mm";
        //　記録表示の表示位置は開始ボタン押下時のアンカー位置とする
        MesuringResultSlateObj.transform.SetPositionAndRotation(pos, rot);

        // 計測停止
        MeasureStop();
        
        // 線描画消去
        LineManager.RulerLineErase();

        // ハンドモニタ無効
        HandMonitorObj.SetActive(false);

        // 設定開始ボタン非表示
        SettingStartBtnObj.SetActive(false);

        // 記録表示ボタン非表示
        DisplayResultBtnObj.SetActive(false);

        // 音声関係のオブジェクト消去
        SaveMode.VoiceObjClear();

        // 記録表示用ボード表示
        MesuringResultSlateObj.SetActive(true);
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
        Voice.clearDistance();

        // 記録表示用ボード非表示
        MesuringResultSlateObj.SetActive(false);

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
        Voice.clearDistance();

        // 記録表示用ボード非表示
        MesuringResultSlateObj.SetActive(false);

        // 苗番号画面表示
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// 苗番号入力へ戻る(直接)
    /// </summary>
    public void DirectReturnKey()
    {
        // 計測結果クリア
        stemLength = 0;
        stemDiameter = 0;
        Voice.clearDistance();

        // 計測停止
        MeasureStop();

        // 線描画消去
        LineManager.RulerLineErase();

        // ハンドモニタ無効
        HandMonitorObj.SetActive(false);

        // 設定開始ボタン非表示
        SettingStartBtnObj.SetActive(false);

        // 音声関係のオブジェクト消去
        SaveMode.VoiceObjClear();

        // ReturnTopボタン非表示
        ReturnTopBtnObj.SetActive(false);

        // 苗番号画面表示
        PlantNoPlateObj.SetActive(true);
    }

    /// <summary>
    /// 記録表示閉じるキー
    /// </summary>
    public void RecordPlateCloseKey()
    {
        // 設定開始ボタン表示
        SettingStartBtnObj.SetActive(true);

        // 記録表示ボタン表示
        DisplayResultBtnObj.SetActive(true);

        // 計測再開
        SettingMenuEnable();
    }
}
