using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LightSliderTest : MonoBehaviour
{
    [SerializeField] int lightChannel = 0;

    public bool isMaxBright = false;

    DMXcontroller dmx;
    PinchSlider pinch;
    
    // Start is called before the first frame update
    void Start()
    {
        dmx = FindObjectOfType<DMXcontroller>();
        pinch = GetComponent<PinchSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderUpdate()
    {
        // pass through slider value * 255 to corresponding channel
        float sliderVal = pinch.SliderValue;

        int brightness = Mathf.RoundToInt(sliderVal * 255);
        //Debug.Log(brightness); //remove

        if (brightness == 255) isMaxBright = true;
        else isMaxBright = false;

        dmx.SetAddress(lightChannel, brightness);

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
