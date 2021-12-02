using System.Collections;
using HKT;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class RulerLineManager : MonoBehaviour
{
    /// <summary>
    /// 長さ表示テキスト
    /// </summary>
    [SerializeField]
    private TextMesh RulerText = default;

    /// <summary>
    /// 長さ表示テキスト2
    /// </summary>
    [SerializeField]
    private TextMesh RulerTextLeft = default;

    /// <summary>
    /// 線オブジェクト
    /// </summary>
    [SerializeField]
    private LineRenderer RulerLine = default;

    /// <summary>
    /// 線オブジェクト2
    /// </summary>
    [SerializeField]
    private LineRenderer RulerLineLeft = default;
    
    // Start is called before the first frame update
    void Start()
    {
        RulerLineInit();
        // ハンドレイを非表示にする
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
        // ハンドレイを非表示にする
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 計測線初期化
    /// </summary>
    public void RulerLineInit()
    {
        RulerLine.SetPosition(0, Vector3.zero);
        RulerLine.SetPosition(1, Vector3.zero);
        RulerLineErase();

        RulerLineLeft.SetPosition(0, Vector3.zero);
        RulerLineLeft.SetPosition(1, Vector3.zero);
        RulerLineLeftErase();
    }

    /// <summary>
    /// 計測線の描画
    /// </summary>
    public void RulerLineDraw(Vector3 pos1, Vector3 pos2, string txtdat)
    {
        RulerLine.gameObject.SetActive(true);
        RulerLine.SetPosition(0, pos1);
        RulerLine.SetPosition(1, pos2);
        RulerLine.startWidth = 0.002f;
        RulerLine.endWidth = 0.002f;
        RulerLine.enabled = true;
        RulerText.text = txtdat;
        var textpos = (pos1 + pos2) / 2;
        textpos.y += 0.05f;
        RulerText.transform.position = textpos;
    }
    /// <summary>
    /// 左手用計測線の描画2
    /// </summary>
    public void RulerLineLeftDraw(Vector3 pos1, Vector3 pos2, string txtdat)
    {
        RulerLineLeft.gameObject.SetActive(true);
        RulerLineLeft.SetPosition(0, pos1);
        RulerLineLeft.SetPosition(1, pos2);
        RulerLineLeft.startWidth = 0.002f;
        RulerLineLeft.endWidth = 0.002f;
        RulerLineLeft.enabled = true;
        RulerTextLeft.text = txtdat;
        var textpos = (pos1 + pos2) / 2;
        textpos.y += 0.05f;
        RulerTextLeft.transform.position = textpos;
    }
    /// <summary>
    /// 計測線消去
    /// </summary>
    public void RulerLineErase()
    {
        RulerLine.gameObject.SetActive(false);
        RulerText.text = "";
    }

    /// <summary>
    /// 左手用計測線消去
    /// </summary>
    public void RulerLineLeftErase()
    {
        RulerLineLeft.gameObject.SetActive(false);
        RulerTextLeft.text = "";
    }
}
