using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCtest : MonoBehaviour
{
    OSC osc;
    Ship ship;

    int messageValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        osc = FindObjectOfType<OSC>();
        osc.SetAddressHandler("/DestShip", OnReceiveDestShip);

        ship = FindObjectOfType<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendFartOSC()
    {
        OscMessage message = new OscMessage();
        message.address = "/fart";
        message.values.Add(messageValue++); // ++ makes sure subsequent presses register in OSCulator
        osc.Send(message);
        Debug.Log("Fart"); //todo remove 
    }

    private void OnReceiveDestShip(OscMessage message)
    {
        ship.DestroyShip();
    }
}
