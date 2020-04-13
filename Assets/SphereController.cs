using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Time before self-destruct")] [SerializeField] float lifeSpan;
    [SerializeField] float force = 50;
    
    [Header("DMX/OSC")]
    [SerializeField] string messageOSC;
    [Tooltip("This value is overriden if 'Lock OSC Value to DMX' is true")] [SerializeField] float valueOSC = 1f;
    [Tooltip("Converts DMX values to OSC floats")] [SerializeField] bool lockOSCValueToDMX = false;

    [Tooltip("DMX channels to control")] [SerializeField] int[] DMXchannels;
    [Tooltip("Brightness value for corresponding channel - !ORDER MUST MATCH DMX CHANNEL ORDER!")] [Range(0,255)] [SerializeField] int[] DMXvalues;

    [Header("Misc Controls")]
    [SerializeField] bool timedEffect = true;
    [SerializeField] float timingOfBlackout = 1;
    [Tooltip("Dim light(s) over time or leave at set values?")] [SerializeField] bool dimOverTime = true;
    [Tooltip("Percent of dimming per frame")] [Range(0, 100)] [SerializeField] int rateOfDim = 20;

    public bool isBullet = true;

    Rigidbody rigidBody;
    DMXcontroller dmx;
    OSC osc;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        dmx = FindObjectOfType<DMXcontroller>();
        osc = FindObjectOfType<OSC>();
        collider = GetComponent<SphereCollider>();
        
        SendDMX();
        SendOSCMessage(valueOSC); //todo refactor for lockOSCvalueToDMX

        if (timedEffect)
        {
            StartCoroutine("TimedLight");
        }

        StartCoroutine("SelfDestruct");
    }

    private void SendDMX()
    {
        if (DMXchannels.Length == 0) return;

        if(DMXchannels.Length == DMXvalues.Length)
        {
            for (int i = 0; i < DMXchannels.Length; i++)
            {
                dmx.SetAddress(DMXchannels[i], DMXvalues[i]);
            }
        }
        else
        {
            Debug.LogError("Mismatch between channels and values arrays - check inspector fields.");
        }  
    }
    private void SendOSCMessage(float oscVal)
    {
        if (!lockOSCValueToDMX)
        {
            OscMessage message = new OscMessage();
            message.address = messageOSC;
            message.values.Add(valueOSC);
            osc.Send(message);
            //Debug.Log("sending OSC: " + message + oscVal); //todo remove
        }
        else
        {
            OscMessage message = new OscMessage();
            message.address = messageOSC;
            message.values.Add(oscVal);
            osc.Send(message);
            //Debug.Log("sending OSC: " + message + oscVal); //todo remove
        }


    }

    private void DimLight()
    {
        if (DMXchannels.Length == 0) return; 

        for (int i = 0; i < DMXchannels.Length; i++)
        {   
            if (DMXvalues[i] > 0)
            {
                DMXvalues[i] -= rateOfDim;
                if (DMXvalues[i] < 0) DMXvalues[i] = 0;
                dmx.SetAddress(DMXchannels[i], DMXvalues[i]);

                /*if (lockOSCValueToDMX)
                {
                    float maxOSCval = 255;
                    float oscDmxConvert = DMXvalues[i] / maxOSCval;
                    SendOSCMessage(oscDmxConvert);
                }*/
            }

            
        }
    }

    IEnumerator TimedLight()
    {
        if (DMXchannels.Length == 0)
        {
            // do nothing
        }

        yield return new WaitForSeconds(timingOfBlackout);
        dmx.ResetDMX();
        //Debug.Log("resetting DMX"); //todo remove
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isBullet)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rigidBody.AddForce(transform.forward * force);
    }

    // Update is called once per frame
    void Update()
    {
        if (dimOverTime)
        {
            DimLight();
        }
    }
}
