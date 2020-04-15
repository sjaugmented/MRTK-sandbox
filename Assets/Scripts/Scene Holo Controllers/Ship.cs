using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    bool isActive = true;
    [SerializeField] GameObject explosionFX;
    [SerializeField] bool hasButton = true;

    DMXcontroller dmxCont;

    Vector3 spawnPos;

    private void Awake()
    {
        dmxCont = FindObjectOfType<DMXcontroller>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        spawnPos = transform.position;

        if (hasButton)
        {
            gameObject.SetActive(false);
            isActive = false;
        }
            
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void InstantiateShip()
    {
        Debug.Log("button pressed"); // todo remove
        
        if (!isActive)
        {
            gameObject.SetActive(true);
            transform.position = spawnPos;
            isActive = true;
        }
        else
        {
            gameObject.SetActive(false);
            isActive = false;
        }
        
    }

    public void DestroyShip()
    {
        Debug.Log("ship clicked"); //todo remove
        if (isActive)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        Debug.Log("ship destroyed"); //todo remove
        Instantiate(explosionFX, transform.position, Quaternion.identity);
        RedLightOn();
        yield return new WaitForSeconds(1.5f);
        isActive = false;
        RedLightOff();
        gameObject.SetActive(false);
    }

    private void RedLightOn()
    {
        dmxCont.SetAddress(1, 255);
    }

    private void RedLightOff()
    {
        dmxCont.SetAddress(1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spell"))
        {
            DestroyShip();
            SendBoomOSC();
            Destroy(other);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("particle hit");
        DestroyShip();
        SendBoomOSC();
    }

    private void SendBoomOSC()
    {
        OSC osc = FindObjectOfType<OSC>();

        OscMessage message = new OscMessage();
        message.address = "/boom";
        message.values.Add(1);
        osc.Send(message);
        Debug.Log("Boom"); //todo remove
    }
}
