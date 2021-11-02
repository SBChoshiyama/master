using HKT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerButton : MonoBehaviour
{
    GameObject MeasuringToolSelectorObj;
    MeasuringToolSelector measuringToolSelector;

    [SerializeField]
    private TextMesh DistanceText = default;

    bool IsTimer;
    public float totalTime;
    int seconds;
    float rocal;

    // Start is called before the first frame update
    void Start()
    {
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();
        IsTimer = false;
        rocal = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimer)
        {
            rocal -= Time.deltaTime;
            if (rocal > 0)
            {
                seconds = (int)rocal;
                DistanceText.text = seconds.ToString();
            }
            else
            {
                Debug.Log("2“_ŠÔ‚Ì‹——£‘ª’èŠJŽn");
                DistanceText.text = "0";
                IsTimer = false;
                rocal = totalTime;
                LineDistance();
            }
        }

    }

    public void ButtonClickEvent()
    {
        IsTimer = true;
    }

    private void LineDistance()
    {
        var dis = measuringToolSelector.LineDistance;
        Debug.Log($"‹——£ = {dis}cm");
        DistanceText.text = dis.ToString("0.0") + " cm";
    }
}
