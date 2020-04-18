using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FingerTracker))]  
public class SpellManager : MonoBehaviour
{
    [SerializeField] float delayBetweenCasts = 0.2f;

    [Header("Caster and Spell hook-ups")]
    [Tooltip("Visual representation of Light spell")] 
    [SerializeField] MeshRenderer lightCaster;
    [Tooltip("Visual representation of Fire spell")] 
    [SerializeField] MeshRenderer fireCaster;
    [Tooltip("Visual representation of Water spell")] 
    [SerializeField] MeshRenderer waterCaster;
    [Tooltip("Visual representation of Wind spell")] 
    [SerializeField] MeshRenderer windCaster;
    [Tooltip("Visual representation of Earth spell")] 
    [SerializeField] MeshRenderer earthCaster;
    [Tooltip("Light spell prefab to cast")] 
    [SerializeField] GameObject lightSpell;
    [Tooltip("Fire spell prefab to cast")] 
    [SerializeField] GameObject fireSpell;
    [Tooltip("Water spell prefab to cast")] 
    [SerializeField] GameObject waterSpell;
    [Tooltip("Wind spell prefab to cast")] 
    [SerializeField] GameObject windSpell;
    [Tooltip("Earth spell prefab to cast")] 
    [SerializeField] GameObject earthSpell;

    [Header("Palm Spellbook")]
    [Tooltip("If false, turn off Palm Menu solvers")]
    [SerializeField] bool usePalmMenu = true;
    [Tooltip("Parent gameObject for the Palm Menu")]
    [SerializeField] GameObject palmMenuParent;
    [Tooltip("Parent gamObject for the Palm Menu visuals")]
    [SerializeField] GameObject palmMenuVisuals;

    [Header("Two Finger Spellbook")]
    [Tooltip("Distance between index fingers that activates Spellbook")]
    [SerializeField] float spellbookDistThresh = 0.8f;
    [SerializeField] int numOfSpells = 5;

    // used to create rate of fire for spells
    bool ableToCast = true;

    public int casterID = 1; //todo remove public
    Vector3 castPosition;

    FingerTracker fingerTracker;

    // Start is called before the first frame update
    void Start()
    {
        fingerTracker = FindObjectOfType<FingerTracker>();
        TurnOffCasters();
    }

    private void TurnOffCasters()
    {
        lightCaster.enabled = false;
        fireCaster.enabled = false;
        waterCaster.enabled = false;
        windCaster.enabled = false;
        earthCaster.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (usePalmMenu)
        {
            palmMenuParent.SetActive(true);
            if (!palmMenuVisuals.activeInHierarchy)
            {
                LookForCastFinger();
                LookForFingerSpellbook();
            }
            else TurnOffCasters();
        }
        else
        {
            palmMenuParent.SetActive(false);
            LookForCastFinger();
            LookForFingerSpellbook();
        }
    }

    private void LookForCastFinger()
    {
        if (fingerTracker.GetCastFingerUp() == true) ActivateCaster();
        else TurnOffCasters();
    }

    private void LookForFingerSpellbook()
    {
        if (fingerTracker.GetTwoFingers() == true) ProcessFingerSpellbook();
    }

    public void ActivateCaster()
    {
        TurnOffCasters();

        if (casterID == 1)
        {
            lightCaster.enabled = true;
            castPosition = lightCaster.transform.position;
        }
        else if (casterID == 2)
        {
            fireCaster.enabled = true;
            castPosition = fireCaster.transform.position;
        }
        else if (casterID == 3)
        {
            waterCaster.enabled = true;
            castPosition = waterCaster.transform.position;
        }
        else if (casterID == 4)
        {
            windCaster.enabled = true;
            castPosition = windCaster.transform.position;
        }
        else if (casterID == 5)
        {
            earthCaster.enabled = true;
            castPosition = earthCaster.transform.position;
        }
        else return;
    }

    private void ProcessFingerSpellbook()
    {
        float spellSlotSize = spellbookDistThresh / numOfSpells;

        float fingerDist = fingerTracker.GetDistIndexes();

        if (fingerDist > 0 && fingerDist <= spellbookDistThresh)
        {
            if (fingerDist > 0 && fingerDist <= spellbookDistThresh - spellSlotSize * 4)
            {
                casterID = 5;
            }
            else if (fingerDist > spellbookDistThresh - spellSlotSize * 4 && fingerDist <= spellbookDistThresh - spellSlotSize * 3)
            {
                casterID = 4;
            }
            else if (fingerDist > spellbookDistThresh - spellSlotSize * 3 && fingerDist <= spellbookDistThresh - spellSlotSize * 2)
            {
                casterID = 3;
            }
            else if (fingerDist > spellbookDistThresh - spellSlotSize * 2 && fingerDist <= spellbookDistThresh - spellSlotSize)
            {
                casterID = 2;
            }
            else
            {
                casterID = 1;
            }
        }
        else return;
    }


    public void CastSpell()
    {
        if (ableToCast && !palmMenuVisuals.activeInHierarchy)
        {
            if (casterID == 1)
            {
                Instantiate(lightSpell, castPosition, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 2)
            {
                Instantiate(fireSpell, castPosition, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 3)
            {
                Instantiate(waterSpell, castPosition, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 4)
            {
                Instantiate(windSpell, castPosition, Camera.main.transform.rotation);
                StartCoroutine("CastDelay");
            }
            else if (casterID == 5)
            {
                Instantiate(earthSpell, castPosition, Camera.main.transform.rotation);
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

