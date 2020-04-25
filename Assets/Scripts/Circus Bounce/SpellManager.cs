using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

    public void DisableStreams()
    {
        foreach (ParticleSystem stream in spellBook.streamSpells)
        {
            var emission = stream.emission;
            emission.enabled = false;
            foreach (Transform child in stream.transform)
            {
                var childEmission = child.GetComponent<ParticleSystem>().emission;
                childEmission.enabled = false;
            }
        }
    }

    private void EnableStream(int index)
    {
        var emission = spellBook.streamSpells[index].emission;
        emission.enabled = true;

        foreach (Transform child in spellBook.streamSpells[index].transform)
        {
            var childEmission = child.GetComponent<ParticleSystem>().emission;
            childEmission.enabled = true; 
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
        else
        {
            DisableCasters();
            DisableStreams();
        }
    }

    private void LookForCastFinger()
    {
        if (fingerTracker.GetCastFingerUp() == true) DisplayCaster();
        else
        {
            DisableCasters();
            DisableStreams();
        }
    }

    private void LookForFormSelector()
    {
        if (fingerTracker.GetTwoFingers() == true)
        {
            SelectCurrForm();
            DisableStreams();
        }
    }

    private void ActivateCurrForm(int index)
    {
        if (currForm == Form.particle)
        {
            spellBook.particleCasters[index].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.particleCasters[index].transform)
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
            spellBook.orbCasters[index].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.orbCasters[index].transform)
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
            spellBook.streamCasters[index].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.streamCasters[index].transform)
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

    public void DisplayCaster()
    {
        DisableCasters();

        if (currEl == Element.light)
        {
            ActivateCurrForm(0);
        }
        else if (currEl == Element.fire)
        {
            ActivateCurrForm(1);
        }
        else if (currEl == Element.water)
        {
            ActivateCurrForm(2);
        }
        else if (currEl == Element.ice)
        {
            ActivateCurrForm(3);
        }
        else return;
    }

    private void SelectCurrForm()
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
                InstantiateForm(0);
            }
            else if (currEl == Element.fire)
            {
                InstantiateForm(1);
            }
            else if (currEl == Element.water)
            {
                InstantiateForm(2);

            }
            else if (currEl == Element.ice)
            {
                InstantiateForm(3);

            }
            else return;
        }
    }

    private void InstantiateForm(int index)
    {
        if (currForm == Form.particle)
        {
            Instantiate(spellBook.particleSpells[index], castingObj.position, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
        }
        else if (currForm == Form.orb)
        {
            Instantiate(spellBook.orbSpells[index], castingObj.position, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
        }
        else if (currForm == Form.stream)
        {
            EnableStream(index);
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

