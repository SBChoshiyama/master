using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonApp : MonoBehaviour
{
    public QRCodeReader QRCodeReader;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Erase Button
    public void EraseButton()
    {
        QRCodeReader.EraseQRText();
    }
}
