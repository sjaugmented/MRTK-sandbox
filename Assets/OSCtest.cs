using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCtest : MonoBehaviour
{
    OSC osc;

    // Start is called before the first frame update
    void Start()
    {
        osc = FindObjectOfType<OSC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendFartOSC()
    {
        OscMessage message = new OscMessage();
        message.address = "/fart";
        message.values.Add(1);
        osc.Send(message);
        Debug.Log("Fart"); //todo remove
    }
}
