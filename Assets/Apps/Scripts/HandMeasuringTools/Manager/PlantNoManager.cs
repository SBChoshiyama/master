using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantNoManager : MonoBehaviour
{
    /// <summary>
    /// 計測コントローラ用GameObject
    /// </summary>
    private GameObject MeasureControllerObj;

    /// <summary>
    /// 計測コントローラ用Object
    /// </summary>
    private MeasureController MeasureController;

    /// <summary>
    /// 苗番号入力画面GameObject
    /// </summary>
    private GameObject PlantNoPlateObj;

    /// <summary>
    /// 苗番号Aパート(アルファベット部)テキスト
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoAText = default;

    /// <summary>
    /// 苗番号Bバート(ナンバー部)テキスト
    /// </summary>
    [SerializeField]
    private TextMesh PlantNoBText = default;

    /// <summary>
    /// 苗番号バートテキスト
    /// </summary>
    private string[] PlantNoASel = new string[] { "A", "B", "C","D","E","F","G","H","I","J","K","L","M","N","O","P"};
    private string[] PlantNoBSel = new string[] { "1", "2" };

    /// <summary>
    /// 苗番号選択番号
    /// </summary>
    private int ASel;
    private int BSel;

    /// <summary>
    /// 苗番号データ
    /// </summary>
    private string PlantNoData;

    // Start is called before the first frame update
    void Start()
    {
        // 設定メニューオブジェクト
        MeasureControllerObj = GameObject.Find("MeasureController");
        MeasureController = MeasureControllerObj.GetComponent<MeasureController>();

        // 苗番号入力画面GameObject
        PlantNoPlateObj = GameObject.Find("PlantNoInputPlate");

        // 初期化
        PlantNoInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void PlantNoInit()
    {
        // 設定初期化
        ASel = 0;
        BSel = 0;

        PlantNoAText.text = PlantNoASel[ASel];
        PlantNoBText.text = PlantNoBSel[BSel];
        PlantNoDataMake();
    }

    /// <summary>
    /// 苗番号AパートUPキー
    /// </summary>
    public void PlantNoAUp()
    {
        ASel++;
        if (ASel >= PlantNoASel.Length)
            ASel = 0;
        PlantNoAText.text = PlantNoASel[ASel];
    }

    /// <summary>
    /// 苗番号AパートDOWNキー
    /// </summary>
    public void PlantNoADown()
    {
        ASel--;
        if (ASel < 0)
            ASel = PlantNoASel.Length - 1;
        PlantNoAText.text = PlantNoASel[ASel];
    }

    /// <summary>
    /// 苗番号BパートUPキー
    /// </summary>
    public void PlantNoBUp()
    {
        BSel++;
        if (BSel >= PlantNoBSel.Length)
            BSel = 0;
        PlantNoBText.text = PlantNoBSel[BSel];
    }

    /// <summary>
    /// 苗番号BパートDOWNキー
    /// </summary>
    public void PlantNoBDown()
    {
        BSel--;
        if (BSel < 0)
            BSel = PlantNoBSel.Length - 1;
        PlantNoBText.text = PlantNoBSel[BSel];
    }

    /// <summary>
    /// 苗番号取得
    /// </summary>
    public string GetPlantNo()
    {
        return PlantNoData;
    }

    /// <summary>
    /// Nextキー
    /// </summary>
    public void NextKey()
    {
        // 苗番号生成
        PlantNoDataMake();
        // 苗番号画面消去
        PlantNoPlateObj.SetActive(false);
        // 設定開始ボタン有効化
        MeasureController.SettingMenuEnable();
    }

    /// <summary>
    /// 苗番号データ生成
    /// </summary>
    private void PlantNoDataMake()
    {
        PlantNoData = PlantNoASel[ASel] + "-" + PlantNoBSel[BSel];
    }

}
