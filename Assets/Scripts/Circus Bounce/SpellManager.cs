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
    [SerializeField] bool usePalmMenu = true;
    [Tooltip("Parent gameObject for the Palm Menu")]
    [SerializeField] GameObject palmMenuParent;
    [Tooltip("Parent gamObject for the Palm Menu visuals")]
    [SerializeField] GameObject palmMenuVisuals;

    [Header("Two Finger Spellbook")]
    [Tooltip("Distance between index fingers that activates Spellbook")]
    [SerializeField] float formMenuThresh = 0.3f;
    [SerializeField] int numOfForms = 3;

    [Header("OSC controller")]
    [SerializeField] string lightMessageOSC = "light message here";
    [SerializeField] float lightValueOSC = 1;
    [SerializeField] string fireMessageOSC = "fire message here";
    [SerializeField] float fireValueOSC = 1;
    [SerializeField] string iceMessageOSC = "ice message here";
    [SerializeField] float iceValueOSC = 1;

    // used to create rate of fire for spells
    bool ableToCast = true;

    public enum Element { light, fire, water, ice };
    public enum Form { particle, orb, stream };
    public Element currEl = Element.light;
    public Form currForm = Form.particle;
    int elementID = 0;
    int formID = 0;

    bool formSelector = false;

    FingerTracker fingerTracker;
    SpellBook spellBook;
    OSC osc;

    private void ConvertElementToID() // allows for quick selection in inspector for testing various elements and forms
    {
        if (currEl == Element.light) elementID = 0;
        if (currEl == Element.fire) elementID = 1;
        if (currEl == Element.water) elementID = 2;
        if (currEl == Element.ice) elementID = 3;
    }


    // Start is called before the first frame update
    void Start()
    {
        fingerTracker = FindObjectOfType<FingerTracker>();
        spellBook = GetComponent<SpellBook>();
        osc = FindObjectOfType<OSC>();

        DisableCasters();
        DisableAllDummies();
        DisableStreams();
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

    private void DisableAllDummies()
    {
        DisableParticleDummies();
        DisableOrbDummies();
        DisableStreamDummies();
    }

    private void DisableParticleDummies()
    {
        foreach (GameObject dummy in spellBook.particleDummies)
        {
            dummy.SetActive(false);
        }
    }

    private void DisableOrbDummies()
    {
        foreach (GameObject dummy in spellBook.orbDummies)
        {
            dummy.SetActive(false);
        }
    }

    private void DisableStreamDummies()
    {
        foreach (GameObject dummy in spellBook.streamDummies)
        {
            dummy.SetActive(false);
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

    private void EnableStream()
    {
        var emission = spellBook.streamSpells[elementID].emission;
        emission.enabled = true;

        foreach (Transform child in spellBook.streamSpells[elementID].transform)
        {
            var childEmission = child.GetComponent<ParticleSystem>().emission;
            childEmission.enabled = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        ConvertElementToID();

        if (usePalmMenu)
        {
            if (!palmMenuVisuals.activeInHierarchy)
            {
                LookForFormSelector();
                if (!formSelector) LookForCastFinger();
            }
            else
            {
                DisableCasters();
                DisableAllDummies();
                DisableStreams();
            }
        }
        else
        {
            CalcPalmPositions();

            bool palmsIn = fingerTracker.GetPalmsIn();
            bool palmsOut = fingerTracker.GetPalmsOut();

            if (palmsIn) OrbSelector();
            else DisableAllDummies();

            if (palmsOut) CastOrb();
            
        }
        
    }

    Vector3 midpoint;
    Vector3 palm1Pos;
    Vector3 palm2Pos;
    float palmDist;

    private void CalcPalmPositions()
    {
        palmDist = fingerTracker.GetPalmDist();
        palm1Pos = fingerTracker.GetPalm1Pos();
        palm2Pos = fingerTracker.GetPalm2Pos();

        midpoint = Vector3.Lerp(palm1Pos, palm2Pos, 0.5f);
    }

    private void OrbSelector()
    {
        currForm = Form.orb;

        float slotSize = formMenuThresh / numOfForms;

        

        if (palmDist > 0 && palmDist <= formMenuThresh)
        {
            if (palmDist > 0 && palmDist <= formMenuThresh - slotSize * 2)
            {
                DisableParticleDummies();
                DisableStreamDummies();
                elementID = 0;

                for (int i = 0; i < spellBook.orbDummies.Count; i++)
                {
                    if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                    else spellBook.orbDummies[i].SetActive(false);
                }

                spellBook.orbDummies[elementID].transform.position = midpoint;
                spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);

                OscMessage message = new OscMessage();
                message.address = lightMessageOSC;
                message.values.Add(lightValueOSC);
                osc.Send(message);
                Debug.Log("sending OSC: " + lightMessageOSC + lightValueOSC); //todo remove


            }
            else if (palmDist > formMenuThresh - slotSize * 2 && palmDist <= formMenuThresh - slotSize)
            {
                DisableParticleDummies();
                DisableStreamDummies();
                elementID = 1;

                for (int i = 0; i < spellBook.orbDummies.Count; i++)
                {
                    if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                    else spellBook.orbDummies[i].SetActive(false);
                }

                spellBook.orbDummies[elementID].transform.position = midpoint;
                spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);

                OscMessage message = new OscMessage();
                message.address = fireMessageOSC;
                message.values.Add(fireValueOSC);
                osc.Send(message);
                Debug.Log("sending OSC: " + fireMessageOSC + fireValueOSC); //todo remove

            }
            else
            {
                DisableParticleDummies();
                DisableStreamDummies();
                elementID = 3;

                for (int i = 0; i < spellBook.orbDummies.Count; i++)
                {
                    if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                    else spellBook.orbDummies[i].SetActive(false);
                }

                spellBook.orbDummies[elementID].transform.position = midpoint;
                spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);

                OscMessage message = new OscMessage();
                message.address = iceMessageOSC;
                message.values.Add(iceValueOSC);
                osc.Send(message);
                Debug.Log("sending OSC: " + iceMessageOSC + iceValueOSC); //todo remove

            }
        }
        else DisableAllDummies();
    }

    private void CastOrb()
    {
        if (ableToCast)
        {
            Debug.Log("casting " + currEl + " orb"); //todo remove
            Instantiate(spellBook.orbSpells[elementID], midpoint, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
        }
        

    }

    private void LookForFormSelector()
    {
        if (fingerTracker.GetPalmsIn() == true)
        {
            formSelector = true;
            DisableCasters();
            DisableStreams();
            SelectCurrForm();
        }
        else formSelector = false;
    }

    private void LookForCastFinger()
    {
        DisableAllDummies();
        if (fingerTracker.GetCastFingerUp() == true)
        {
            DisplayCaster();
        }
        else
        {
            DisableCasters();
            DisableStreams();
        }
    }
    private void SelectCurrForm()
    {
        float slotSize = formMenuThresh / numOfForms;

        float palmDist = fingerTracker.GetPalmDist();
        Vector3 palm1Pos = fingerTracker.GetPalm1Pos();
        Vector3 palm2Pos = fingerTracker.GetPalm2Pos();

        var midpoint = Vector3.Lerp(palm1Pos, palm2Pos, 0.5f);

        if (palmDist > 0 && palmDist <= formMenuThresh)
        {
            if (palmDist > 0 && palmDist <= formMenuThresh - slotSize * 2)
            {
                currForm = Form.particle;
                DisableOrbDummies();
                DisableStreamDummies();

                for (int i = 0; i < spellBook.particleDummies.Count; i++)
                {
                    if (i == elementID) spellBook.particleDummies[i].SetActive(true);
                    else spellBook.particleDummies[i].SetActive(false);
                }

                spellBook.particleDummies[elementID].transform.position = midpoint;
                spellBook.particleDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);

            }
            else if (palmDist > formMenuThresh - slotSize * 2 && palmDist <= formMenuThresh - slotSize)
            {
                currForm = Form.orb;
                DisableParticleDummies();
                DisableStreamDummies();

                for (int i = 0; i < spellBook.orbDummies.Count; i++)
                {
                    if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                    else spellBook.orbDummies[i].SetActive(false);
                }

                spellBook.orbDummies[elementID].transform.position = midpoint;
                spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);
            }
            else
            {
                currForm = Form.stream;
                DisableParticleDummies();
                DisableOrbDummies();

                for (int i = 0; i < spellBook.streamDummies.Count; i++)
                {
                    if (i == elementID) spellBook.streamDummies[i].SetActive(true);
                    else spellBook.streamDummies[i].SetActive(false);
                }

                spellBook.streamDummies[elementID].transform.position = midpoint;
                spellBook.streamDummies[elementID].transform.localScale = new Vector3(palmDist, palmDist, palmDist);
            }
        }
        else DisableAllDummies();
    }

    public void DisplayCaster()
    {
        DisableCasters();

        if (currForm == Form.particle)
        {
            spellBook.particleCasters[elementID].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.particleCasters[elementID].transform)
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
            spellBook.orbCasters[elementID].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.orbCasters[elementID].transform)
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
            spellBook.streamCasters[elementID].layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in spellBook.streamCasters[elementID].transform)
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


    public void CastSpell()
    {
        if (ableToCast && !palmMenuVisuals.activeInHierarchy)
        {
            InstantiateForm();
        }
        else return;
    }

    private void InstantiateForm()
    {
        if (currForm == Form.particle)
        {
            Instantiate(spellBook.particleSpells[elementID], castingObj.position, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
        }
        else if (currForm == Form.orb)
        {
            Instantiate(spellBook.orbSpells[elementID], castingObj.position, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
        }
        else if (currForm == Form.stream)
        {
            EnableStream();
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

    public Form GetCurrForm() //used by FingerTracker to know when and how to cast particles/orbs vs streams
    {
        return currForm;
    }
}

