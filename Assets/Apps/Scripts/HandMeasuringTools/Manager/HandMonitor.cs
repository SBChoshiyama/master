using HKT;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandMonitor : MonoBehaviour
{
    /// <summary>
    /// HandJointServiceインスタンス
    /// </summary>
    private IMixedRealityHandJointService handJointService = null;

    /// <summary>
    /// DataProviderAccessインスタンス
    /// </summary>
    private IMixedRealityDataProviderAccess dataProviderAccess = null;

    /// <summary>
    /// HandStatus(OK)表示用GameObject
    /// </summary>
    private GameObject HandStatusOKObj;

    /// <summary>
    /// HandStatus(NG)表示用GameObject
    /// </summary>
    private GameObject HandStatusNGObj;

    /// <summary>
    /// メジャーツール切替GameObject
    /// </summary>
    private GameObject MeasuringToolObj;

    /// <summary>
    /// メジャーツール切替Object
    /// </summary>
    private MeasuringToolSelector MeasuringTool;

    /// <summary>
    ///  SaveモードGameObject
    /// </summary>
    private GameObject SaveModeObj;

    /// <summary>
    ///  SaveモードObject
    /// </summary>
    private SavingToolSelector SaveMode;

    /// <summary>
    /// HandStatus表示用テキスト
    /// </summary>
    [SerializeField]
    private TextMesh HandStatusTxt = default;

    /// <summary>
    ///  手の検出フラグ
    /// </summary>
    private bool isHandTrack;

    /// <summary>
    ///  右手の検出用カウンタ
    /// </summary>
    private int RightHandCnt;

    /// <summary>
    ///  左手の検出用カウンタ
    /// </summary>
    private int LeftHandCnt;

    /// <summary>
    ///  何回検出で有効とするか
    /// </summary>
    private int CNTMAX = 10;

    // Start is called before the first frame update
    void Start()
    {
        // HandStatus(OK)表示用オブジェクト
        HandStatusOKObj = GameObject.Find("HandStatusOK");

        // HandStatus(NG)表示用オブジェクト
        HandStatusNGObj = GameObject.Find("HandStatusNG");

        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService == null)
        {
            Debug.LogError("Can't get IMixedRealityHandJointService.");
            return;
        }

        dataProviderAccess = CoreServices.InputSystem as IMixedRealityDataProviderAccess;
        if (dataProviderAccess == null)
        {
            Debug.LogError("Can't get IMixedRealityDataProviderAccess.");
            return;
        }

        // メジャーツール切替オブジェクト
        MeasuringToolObj = GameObject.Find("MeasuringToolSelector");
        MeasuringTool = MeasuringToolObj.GetComponent<MeasuringToolSelector>();

        // Saveモードオブジェクト
        SaveModeObj = GameObject.Find("SavingToolSelector");
        SaveMode = SaveModeObj.GetComponent<SavingToolSelector>();

        // 初期化
        HandMonitorInit();
    }

    // Update is called once per frame
    void Update()
    {
        // 手の検出判定処理
        // 右手検出チェック
        if (handJointService.IsHandTracked(Handedness.Right))
        {
            if (RightHandCnt > 0)
                RightHandCnt--;
        }
        else
        {
            RightHandCnt = CNTMAX;
        }
        // 左検出チェック
        if (handJointService.IsHandTracked(Handedness.Left))
        {
            if (LeftHandCnt > 0)
                LeftHandCnt--;
        }
        else
        {
            LeftHandCnt = CNTMAX;
        }

        // モード別に有効な手をチェック
        // 写真撮影モード(右手か左手を検知していればOK)
        if (SaveMode.isPhotoCapture())
        {
            if (!isRightHandFind() && !isLeftHandFind())
            {
                HandStatusTxt.text = "右手・左手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }

        }
        // 手測定OFFモード
        else if (MeasuringTool.isUseHandRulerOFF())
        {
            HandStatusTxt.text = "";
            HandStatusOKObj.SetActive(false);
            HandStatusNGObj.SetActive(false);
            isHandTrack = false;
            return;
        }
        // 片手モード(右手を検知していればOK)
        else if (MeasuringTool.isUseOneHands())
        {
            if (!isRightHandFind())
            {
                HandStatusTxt.text = "右手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        // 両手モード(右手・左手ともに検知していればOK)
        else
        {
            if (!isRightHandFind() && !isLeftHandFind())
            {
                HandStatusTxt.text = "右手・左手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!isRightHandFind())
            {
                HandStatusTxt.text = "右手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!isLeftHandFind())
            {
                HandStatusTxt.text = "左手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        HandStatusTxt.text = "";
        HandStatusOKObj.SetActive(true);
        HandStatusNGObj.SetActive(false);
        isHandTrack = true;
    }
    /// <summary>
    ///  ハンドモニター初期化
    /// </summary>
    private void HandMonitorInit()
    {
        HandStatusOKObj.SetActive(false);
        HandStatusNGObj.SetActive(false);
        HandStatusTxt.text = "";
        isHandTrack = false;

        RightHandCnt = CNTMAX;
        LeftHandCnt = CNTMAX;
    }

    /// <summary>
    ///  右手検出
    /// </summary>
    private bool isRightHandFind()
    {
        if (RightHandCnt == 0)
            return true;
        return false;
    }

    /// <summary>
    ///  左手検出
    /// </summary>
    private bool isLeftHandFind()
    {
        if (LeftHandCnt == 0)
            return true;
        return false;
    }

    /// <summary>
    ///  手の検出
    /// </summary>
    public bool isHandTracking()
    {
        return isHandTrack;
    }
    /// <summary>
    ///  左手の検出
    /// </summary>
    public bool isLeftHandTracking()
    {
        if (isLeftHandFind())
            return true;
        return false;
    }
}
