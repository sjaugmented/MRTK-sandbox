using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCReceiver : MonoBehaviour
{
    OSC osc;
    Ship ship;

    [SerializeField] string OSCmess = "/1/push1";

    // Start is called before the first frame update
    void Start()
    {
        osc = FindObjectOfType<OSC>();
        osc.SetAllMessageHandler(OnReceiveDestShip);

        ship = FindObjectOfType<Ship>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnReceiveDestShip(OscMessage message)
    {
        Debug.Log("message received");
        ship.DestroyShip();
    }
}
