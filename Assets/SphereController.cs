using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [Tooltip("Time before self-destruct")] [SerializeField] float lifeSpan;
    [SerializeField] float force = 50;
    [SerializeField] string messageOSC = "/test/";
    [SerializeField] float valueOSC = 1f;
    [Tooltip("DMX channels to control")] [SerializeField] int[] DMXchannels;
    [Tooltip("Brightness value for corresponding channel - !ORDER MUST MATCH DMX CHANNEL ORDER!")] [Range(0,255)] [SerializeField] int[] DMXvalues;
    [SerializeField] bool timedEffect = true;
    [SerializeField] float timingOfBlackout = 1;
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
        SendOSCMessage();

        if (timedEffect)
        {
            StartCoroutine("TimedLight");
        }

        StartCoroutine("SelfDestruct");
    }

    private void SendDMX()
    {
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
    private void SendOSCMessage()
    {
        OscMessage message = new OscMessage();
        message.address = messageOSC;
        message.values.Add(valueOSC);
        osc.Send(message);
        Debug.Log("sending OSC: " + message); //todo remove
    }

    IEnumerator TimedLight()
    {
        yield return new WaitForSeconds(timingOfBlackout);
        dmx.ResetDMX();
        Debug.Log("resetting DMX"); //todo remove
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
        
    }
}
