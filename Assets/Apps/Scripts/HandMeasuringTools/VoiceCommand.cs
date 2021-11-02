using HKT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCommand : MonoBehaviour
{
    GameObject MeasuringToolSelectorObj;
    MeasuringToolSelector measuringToolSelector;

    [SerializeField]
    private TextMesh DistanceText = default;


    // Start is called before the first frame update
    void Start()
    {
        MeasuringToolSelectorObj = GameObject.Find("MeasuringToolSelector");
        measuringToolSelector = MeasuringToolSelectorObj.GetComponent<MeasuringToolSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCommandEvent()
    {
        var dis = measuringToolSelector.LineDistance;
        Debug.Log($"‹——£ = {dis}cm");
        DistanceText.text = dis.ToString("0.0") + " cm";

    }
}
