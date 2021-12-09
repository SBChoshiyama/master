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

        HandStatusOKObj.SetActive(false);
        HandStatusNGObj.SetActive(true);
        HandStatusTxt.text = "";
        isHandTrack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
        // (rightIndexTip.position.y > -0.2))
            
        // 写真撮影モード
        if (SaveMode.isPhotoCapture())
        {
            if (!handJointService.IsHandTracked(Handedness.Right) &&
                !handJointService.IsHandTracked(Handedness.Left))
            {
                HandStatusTxt.text = "右手・左手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }

        }
        // 片手モード
        else if (MeasuringTool.isUseOneHands())
        {
            if (!handJointService.IsHandTracked(Handedness.Right))
            {
                HandStatusTxt.text = "右手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
        }
        // 両手モード
        else
        {
            if (!handJointService.IsHandTracked(Handedness.Right) &&
                !handJointService.IsHandTracked(Handedness.Left))
            {
                HandStatusTxt.text = "右手・左手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!handJointService.IsHandTracked(Handedness.Right))
            {
                HandStatusTxt.text = "右手";
                HandStatusOKObj.SetActive(false);
                HandStatusNGObj.SetActive(true);
                isHandTrack = false;
                return;
            }
            else if (!handJointService.IsHandTracked(Handedness.Left))
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
    ///  手の検出
    /// </summary>
    public bool isHandTracking()
    {
        return isHandTrack;
    }
}
