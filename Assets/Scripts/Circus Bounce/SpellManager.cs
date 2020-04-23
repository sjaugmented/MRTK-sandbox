using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FingerTracker))]  
public class SpellManager : MonoBehaviour
{
    [SerializeField] float delayBetweenCasts = 0.2f;
    [SerializeField] Transform castingObj;
    [SerializeField] SortingLayer activeLayer;
    [SerializeField] SortingLayer inactiveLayer;

    [Header("Palm Spellbook")]
    [Tooltip("Parent gameObject for the Palm Menu")]
    [SerializeField] GameObject palmMenuParent;
    [Tooltip("Parent gamObject for the Palm Menu visuals")]
    [SerializeField] GameObject palmMenuVisuals;

    [Header("Two Finger Spellbook")]
    [Tooltip("Distance between index fingers that activates Spellbook")]
    [SerializeField] float spellbookDistThresh = 0.3f;
    [SerializeField] int numOfForms = 3;

    // used to create rate of fire for spells
    bool ableToCast = true;

    public enum Element { light, fire, water, ice };
    public enum Form { particle, orb, stream };
    public Element currEl = Element.light;
    public Form currForm = Form.particle;

    FingerTracker fingerTracker;
    SpellBook spellBook;

    // Start is called before the first frame update
    void Start()
    {
        fingerTracker = FindObjectOfType<FingerTracker>();
        spellBook = GetComponent<SpellBook>();

        DisableCasters();
    }

    private void DisableCasters()
    {
        foreach (GameObject caster in spellBook.particleCasters)
        {
            caster.layer = LayerMask.NameToLayer("Disabled");
            foreach (Transform child in caster.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Disabled");
                foreach (Transform child1 in child)
                {
                    child1.gameObject.layer = LayerMask.NameToLayer("Disabled");
                }
            }

        }
        foreach (GameObject caster in spellBook.orbCasters)
        {
            caster.layer = LayerMask.NameToLayer("Disabled");
            foreach (Transform child in caster.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Disabled");
                foreach (Transform child1 in child)
                {
                    child1.gameObject.layer = LayerMask.NameToLayer("Disabled");
                }
            }
        }
        foreach (GameObject caster in spellBook.streamCasters)
        {
            caster.layer = LayerMask.NameToLayer("Disabled");
            foreach (Transform child in caster.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Disabled");
                foreach (Transform child1 in child)
                {
                    child1.gameObject.layer = LayerMask.NameToLayer("Disabled");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!palmMenuVisuals.activeInHierarchy)
        {
            LookForCastFinger();
            LookForFormSelector();
        }
        else DisableCasters();
    }

    private void LookForCastFinger()
    {
        if (fingerTracker.GetCastFingerUp() == true) ActivateCaster();
        else DisableCasters();
    }

    private void LookForFormSelector()
    {
        if (fingerTracker.GetTwoFingers() == true) SelectForm();
    }

    public void ActivateCaster()
    {
        DisableCasters();

        if (currEl == Element.light)
        {
            if (currForm == Form.particle)
            {
                spellBook.particleCasters[0].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.particleCasters[0].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.orb)
            {
                spellBook.orbCasters[0].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.orbCasters[0].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.stream)
            {
                spellBook.streamCasters[0].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.streamCasters[0].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else return;
        }
        else if (currEl == Element.fire)
        {
            if (currForm == Form.particle)
            {
                spellBook.particleCasters[1].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.particleCasters[1].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.orb)
            {
                spellBook.orbCasters[1].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.orbCasters[1].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.stream)
            {
                spellBook.streamCasters[1].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.streamCasters[1].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else return;
        }
        else if (currEl == Element.water)
        {
            if (currForm == Form.particle)
            {
                spellBook.particleCasters[2].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.particleCasters[2].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.orb)
            {
                spellBook.orbCasters[2].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.orbCasters[2].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.stream)
            {
                spellBook.streamCasters[2].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.streamCasters[2].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else return;
        }
        else if (currEl == Element.ice)
        {
            if (currForm == Form.particle)
            {
                spellBook.particleCasters[3].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.particleCasters[3].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.orb)
            {
                spellBook.orbCasters[3].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.orbCasters[3].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else if (currForm == Form.stream)
            {
                spellBook.streamCasters[3].layer = LayerMask.NameToLayer("Default");
                foreach (Transform child in spellBook.streamCasters[3].transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                    foreach (Transform nextChild in child)
                    {
                        nextChild.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                }
            }
            else return;
        }
        else return;
    }

    private void SelectForm()
    {
        float slotSize = spellbookDistThresh / numOfForms;

        float fingerDist = fingerTracker.GetDistIndexes();

        if (fingerDist > 0 && fingerDist <= spellbookDistThresh)
        {
            if (fingerDist > 0 && fingerDist <= spellbookDistThresh - slotSize * 2)
            {
                currForm = Form.particle;
            }
            else if (fingerDist > spellbookDistThresh - slotSize * 2 && fingerDist <= spellbookDistThresh - slotSize)
            {
                currForm = Form.orb;
            }
            else
            {
                currForm = Form.stream;
            }
        }
        else return;
    }


    public void CastSpell()
    {
        if (ableToCast && !palmMenuVisuals.activeInHierarchy)
        {
            if (currEl == Element.light)
            {
                if (currForm == Form.particle)
                {
                    Instantiate(spellBook.particleSpells[0], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.orb)
                {
                    Instantiate(spellBook.orbSpells[0], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.stream)
                {
                    Instantiate(spellBook.streamSpells[0], castingObj.position, castingObj.rotation);
                }
            }
            else if (currEl == Element.fire)
            {
                if (currForm == Form.particle)
                {
                    Instantiate(spellBook.particleSpells[1], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.orb)
                {
                    Instantiate(spellBook.orbSpells[1], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.stream)
                {
                    Instantiate(spellBook.streamSpells[1], castingObj.position, castingObj.rotation);
                }
            }
            else if (currEl == Element.water)
            {
                if (currForm == Form.particle)
                {
                    Instantiate(spellBook.particleSpells[2], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.orb)
                {
                    Instantiate(spellBook.orbSpells[2], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.stream)
                {
                    Instantiate(spellBook.streamSpells[2], castingObj.position, castingObj.rotation);
                }
            }
            else if (currEl == Element.ice)
            {
                if (currForm == Form.particle)
                {
                    Instantiate(spellBook.particleSpells[3], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.orb)
                {
                    Instantiate(spellBook.orbSpells[3], castingObj.position, Camera.main.transform.rotation);
                    StartCoroutine("CastDelay");
                }
                else if (currForm == Form.stream)
                {
                    Instantiate(spellBook.streamSpells[3], castingObj.position, castingObj.rotation);
                }
            }
            else return;

            
        }
    }

    IEnumerator CastDelay()
    {
        ableToCast = false;
        yield return new WaitForSeconds(delayBetweenCasts);
        ableToCast = true;
    }

    public void SetLight()
    {
        currEl = Element.light;
    }

    public void SetFire()
    {
        currEl = Element.fire;
    }

    public void SetWater()
    {
        currEl = Element.water;
    }

    public void SetIce()
    {
        currEl = Element.ice;
    }

    public Form GetCurrForm()
    {
        return currForm;
    }
}

