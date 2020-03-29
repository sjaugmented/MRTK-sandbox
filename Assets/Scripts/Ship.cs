using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] GameObject explosionFX;

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
        gameObject.SetActive(false);
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
}
