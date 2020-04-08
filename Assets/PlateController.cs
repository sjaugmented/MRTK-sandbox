using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] string messageOSC = "/test/";
    [SerializeField] float valueOSC = 1;
    [SerializeField] int DMXchannel = 1;

    DMXcontroller dmx;
    OSC osc;

    // Start is called before the first frame update
    void Start()
    {
        dmx = FindObjectOfType<DMXcontroller>();
        osc = FindObjectOfType<OSC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spell"))
        {
            Destroy(other);
            dmx.SetAddress(DMXchannel, 255);
            SendOSCMessage();
        }
    }

    private void SendOSCMessage()
    {
        OscMessage message = new OscMessage();
        message.address = messageOSC;
        message.values.Add(valueOSC);
        osc.Send(message);
        Debug.Log(message); //todo remove
    }
}
