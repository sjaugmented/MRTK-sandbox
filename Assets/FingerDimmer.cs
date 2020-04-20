using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FingerDimmerScaler))]
public class FingerDimmer : MonoBehaviour
{
    [SerializeField] int dmxChannel = 1;
    [SerializeField] Transform lvlIndicator;
    [SerializeField] float bufferZone = 0.05f;
    [SerializeField] GameObject lightObj; // remove after testing

    /*[SerializeField] Transform tip;         //
    [SerializeField] Transform knuckle;     // todo remove*/

    Light workLight;
    FingerDimmerScaler dimmer;
    DMXcontroller dmx;

    void Start()
    {
        dmx = FindObjectOfType<DMXcontroller>();
        workLight = lightObj.GetComponent<Light>();
        dimmer = GetComponent<FingerDimmerScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIndicatorPos();
    }

    private void ProcessIndicatorPos()
    {
        Vector3 fingerTip = dimmer.GetIndexTip();
        Vector3 fingerKnuckle = dimmer.GetIndexKnuckle();
        float fingerLength = dimmer.GetFingerLength();

        float distToTip = Vector3.Distance(lvlIndicator.position, fingerTip);
        float distToKnuckle = Vector3.Distance(lvlIndicator.position, fingerKnuckle);

        if (distToTip <= fingerLength * bufferZone)
        {
            workLight.intensity = 25;
            dmx.SetAddress(dmxChannel, 255);
        }
        else if (distToKnuckle <= fingerLength * bufferZone)
        {
            workLight.intensity = 0;
            dmx.SetAddress(dmxChannel, 0);
        }
        else
        {
            float dimmerFloat = distToKnuckle / fingerLength;

            workLight.intensity = dimmerFloat * 25;
            dmx.SetAddress(dmxChannel, Mathf.RoundToInt(dimmerFloat * 255));
        }
    }
}
