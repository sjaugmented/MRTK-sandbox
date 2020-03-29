using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LightSliderTest : MonoBehaviour
{
    [SerializeField] int lightChannel;

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
        float sliderValue = pinch.SliderValue;

        int brightness = Mathf.RoundToInt(sliderValue * 255);
        //Debug.Log(brightness); //remove

        dmx.SetAddress(lightChannel, brightness);

    }
}
