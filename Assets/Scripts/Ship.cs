using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] GameObject explosionFX;

    DMXcontrol dmx;

    private void Awake()
    {
        dmx = FindObjectOfType<DMXcontrol>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void InstantiateShip()
    {
        Debug.Log("button pressed");
        
        if (!isActive)
        {
            gameObject.SetActive(true);
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
        Debug.Log("ship clicked");
        if (isActive)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        Debug.Log("ship destroyed");
        Instantiate(explosionFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        isActive = false;
        gameObject.SetActive(false);
    }
}
