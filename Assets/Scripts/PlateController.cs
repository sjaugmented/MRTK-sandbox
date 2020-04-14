using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] string messageOSC = "/test/";
    [SerializeField] float valueOSC = 1;
    [Tooltip("DMX channels to control")] [SerializeField] int[] DMXchannels;
    [Tooltip("Brightness value for corresponding channel - !ORDER MUST MATCH DMX CHANNEL ORDER!")] [Range(0, 255)] [SerializeField] int[] DMXvalues;
    [SerializeField] bool timedEffect = true;
    [SerializeField] float timingOfBlackout = 1f;
    [SerializeField] float holoFlashDuration = 0.2f;

    DMXcontroller dmx;
    OSC osc;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        dmx = FindObjectOfType<DMXcontroller>();
        osc = FindObjectOfType<OSC>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        dmx.ResetDMX();

        if (other.CompareTag("Spell"))
        {
            StartCoroutine("FlashHolo");
            SendDMX();
            SendOSCMessage();

            if (timedEffect)
            {
                StartCoroutine("TimedLight");
            }

            if (other.GetComponent<SphereController>().isBullet == false)
            {
                Destroy(other);
            }
        }
    }

    IEnumerator FlashHolo()
    {
        Color currColor = rend.material.color;
        rend.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(holoFlashDuration);
        rend.material.SetColor("_Color", currColor);
    }
    private void SendDMX()
    {
        if (DMXchannels.Length == DMXvalues.Length)
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
        Debug.Log(message); //todo remove
    }

    IEnumerator TimedLight()
    {
        yield return new WaitForSeconds(timingOfBlackout);
        dmx.ResetDMX();
    }

    
}
