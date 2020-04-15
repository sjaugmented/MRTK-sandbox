using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LightSliderTest : MonoBehaviour
{
    [SerializeField] int lightChannel = 0;
    [SerializeField] string messageOSC = "/test";
    [SerializeField] float valueOSC = 0;

    public bool isMaxBright = false; // todo remove public

    DMXcontroller dmx;
    PinchSlider pinch;
    OSC osc;
    
    // Start is called before the first frame update
    void Start()
    {
        dmx = FindObjectOfType<DMXcontroller>();
        pinch = GetComponent<PinchSlider>();
        osc = FindObjectOfType<OSC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderUpdate()
    {
        // pass through slider value * 255 to corresponding channel
        float sliderVal = pinch.SliderValue;

        int sliderValInt = Mathf.RoundToInt(sliderVal * 255);

        if (sliderValInt == 255) isMaxBright = true;
        else isMaxBright = false;

        dmx.SetAddress(lightChannel, sliderValInt);
        SendOSCMessage(sliderVal);

    }

    private void SendOSCMessage(float sliderVal)
    {
        valueOSC = sliderVal;
        
        OscMessage message = new OscMessage();
        message.address = messageOSC;
        message.values.Add(valueOSC);
        osc.Send(message);
        Debug.Log(message); //todo remove
    }

    public void MinMaxToggle()
    {
        if (!isMaxBright)
        {
            dmx.SetAddress(lightChannel, 255);
            isMaxBright = true;
        }
        else
        {
            dmx.SetAddress(lightChannel, 0); 
            isMaxBright = false;
        }
    }
}
