using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTesting : MonoBehaviour
{
    [SerializeField] float delayBetweenCasts = 0.2f;
    [Tooltip("Visual representation of Light spell")] [SerializeField] GameObject lightCaster;
    [Tooltip("Visual representation of Fire spell")] [SerializeField] GameObject fireCaster;
    [Tooltip("Visual representation of Water spell")] [SerializeField] GameObject waterCaster;
    [Tooltip("Visual representation of Wind spell")] [SerializeField] GameObject windCaster;
    [Tooltip("Visual representation of Earth spell")] [SerializeField] GameObject earthCaster;
    [Tooltip("Light spell prefab to cast")] [SerializeField] GameObject lightSpell;
    [Tooltip("Fire spell prefab to cast")] [SerializeField] GameObject fireSpell;
    [Tooltip("Water spell prefab to cast")] [SerializeField] GameObject waterSpell;
    [Tooltip("Wind spell prefab to cast")] [SerializeField] GameObject windSpell;
    [Tooltip("Earth spell prefab to cast")] [SerializeField] GameObject earthSpell;

    // used to create rate of fire for spells
    bool ableToCast = true;

    int casterID = 0;
    Transform castPosition;
    GameObject spellToCast;

    FingerTracker fingerTracker;

    // Start is called before the first frame update
    void Start()
    {
        fingerTracker = FindObjectOfType<FingerTracker>();
        ResetCasters();
    }

    private void ResetCasters()
    {
        lightCaster.SetActive(false);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fingerTracker.GetFingerUp());//remove
        if (fingerTracker.GetFingerUp() == true) ActivateCaster();
        else ResetCasters();
    }

    public void ActivateCaster()
    {
        ResetCasters();

        if (casterID == 1)
        {
            lightCaster.SetActive(true);
            castPosition = lightCaster.transform;
        }
        else if (casterID == 2)
        {
            fireCaster.SetActive(true);
            castPosition = fireCaster.transform;
        }
        else if (casterID == 3)
        {
            waterCaster.SetActive(true);
            castPosition = waterCaster.transform;
        }
        else if (casterID == 4)
        {
            windCaster.SetActive(true);
            castPosition = windCaster.transform;
        }
        else if (casterID == 5)
        {
            earthCaster.SetActive(true);
            castPosition = earthCaster.transform;
        }
        else return;
    }

    public void CastTestSpell()
    {
        if (ableToCast)
        {
            if (casterID == 1)
            {
                Instantiate(lightSpell, castPosition.position, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 2)
            {
                Instantiate(fireSpell, castPosition.position, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 3)
            {
                Instantiate(waterSpell, castPosition.position, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 4)
            {
                Instantiate(windSpell, castPosition.position, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 5)
            {
                Instantiate(earthSpell, castPosition.position, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else return;
        }
        else return;
        
    }

    IEnumerator CastDelay()
    {
        ableToCast = false;
        yield return new WaitForSeconds(delayBetweenCasts);
        ableToCast = true;
    }
    public void SetLight()
    {
        casterID = 1;
    }

    public void SetFire()
    {
        casterID = 2;
    }

    public void SetWater()
    {
        casterID = 3;
    }

    public void SetWind()
    {
        casterID = 4;
    }

    public void SetEarth()
    {
        casterID = 5;
    }
}

