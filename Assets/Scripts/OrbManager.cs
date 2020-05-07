using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] float delayBetweenCasts = 0.2f;

    [Tooltip("Parent object of the palm menu visuals")]
    [SerializeField] GameObject palmMenuVisuals;
    [SerializeField] List<OctaRotater> elementOctas;

    [Header("Palm Conjure")]
    [Tooltip("Max distance between palms for conjuring")]
    [SerializeField] float formMenuThresh = 0.3f;
    [SerializeField] float scaleMultiplier = 1f;

    [Header("OSC controller")]
    public List<String> conjureOSCMessages;
    
    float conjureValueOSC = 0;

    [Header("Misc")]
    public float spellScale = 1;
    public enum Element { light, fire, water, ice };
    public enum Form { particle, orb, stream };
    public Element currEl = Element.light;
    public Form currForm = Form.orb; // in case we reintroduce different forms - ie, particles, streams
    int elementID = 0;

    // coordinates for conjuring
    Vector3 midpointPalms;
    Vector3 midpointRockOn;
    Vector3 palm1Pos;
    Vector3 palm2Pos;
    float palmDist;
    Vector3 rtIndexPos;
    Vector3 rtPinkyPos;

    // used to create rate of fire for spells
    bool ableToCast = true;
    bool fromElSelector = false;
    bool fromOrbScaler = false;

    OrbFingerTracker handTracking;
    SpellBook spellBook;
    OSC osc;

    private void ConvertElementToID() // allows for quick selection in inspector for testing various elements and forms
    {
        if (currEl == Element.light) elementID = 0;
        if (currEl == Element.fire) elementID = 1;
        if (currEl == Element.water) elementID = 2;
        if (currEl == Element.ice) elementID = 3;
    }

    private void ConfigOctas()
    {
        for(int i = 0; i < elementOctas.Count; i++)
        {
            if (i == elementID) elementOctas[i].isSelected = true;
            else elementOctas[i].isSelected = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        handTracking = FindObjectOfType<OrbFingerTracker>();
        spellBook = GetComponent<SpellBook>();
        osc = FindObjectOfType<OSC>();

        DisableOrbDummies();
        DisableStreams();
    }

    private void DisableOrbDummies()
    {
        foreach (GameObject dummy in spellBook.orbDummies)
        {
            dummy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ConvertElementToID();
        //ConfigOctas();

        bool twoPalms = handTracking.GetTwoPalms();
        bool touchDown = handTracking.GetTouchdown();
        bool palmsForward = handTracking.GetPalmsForward();
        bool palmsIn = handTracking.GetPalmsIn();
        bool rockOn = handTracking.GetRockOn();
        bool fingerGun = handTracking.GetFingerGun();

        CalcHandPositions();

        if (twoPalms)
        {
            if (palmsForward)
            {
                CastOrb();
                DisableOrbDummies();
            }
            else if (palmsIn)
            {
                ElementSelector();
            }
            else if (touchDown)
            {
                OrbScaler();

                conjureValueOSC = palmDist / formMenuThresh;

                if (conjureValueOSC < 0) conjureValueOSC = 0;
                if (conjureValueOSC > 1) conjureValueOSC = 1;
                SendOSCMessage(conjureOSCMessages[elementID], conjureValueOSC);

            }
            else DisableOrbDummies();

            if (fingerGun)
            {
                CastParticle();
                DisableStreams();
            }
            else if (rockOn)
            {
                EnableStream();
            }
            else
            {
                DisableStreams();
            }
        }
        else
        {
            DisableOrbDummies();
            DisableStreams();
        }
    }

    private void SendOSCMessage(string address, float value)
    {
        OscMessage message = new OscMessage();
        message.address = address;
        message.values.Add(value);
        osc.Send(message);
        //Debug.Log("Sending OSC: " + address + " " + value); // todo remove
    }

    private void CalcHandPositions()
    {
        palmDist = handTracking.GetPalmDist();
        palm1Pos = handTracking.GetPalm1Pos();
        palm2Pos = handTracking.GetPalm2Pos();
        rtIndexPos = handTracking.GetRtIndexPos();
        rtPinkyPos = handTracking.GetRtPinkyPos();

        midpointPalms = Vector3.Lerp(palm1Pos, palm2Pos, 0.5f);
        midpointRockOn = Vector3.Lerp(rtIndexPos, rtPinkyPos, 0.5f);

        if (palmDist < formMenuThresh) spellScale = palmDist * scaleMultiplier;
        if (palmDist >= formMenuThresh) spellScale = formMenuThresh * scaleMultiplier;
    }

    private void ElementSelector()
    {
        fromOrbScaler = false;
        float elSlotSize = formMenuThresh / spellBook.orbDummies.Count;

        if (palmDist > 0 && palmDist <= formMenuThresh - elSlotSize * 3)
        {
            currEl = Element.light;
            for (int i = 0; i < spellBook.orbDummies.Count; i++)
            {
                if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                else spellBook.orbDummies[i].SetActive(false);
            }
        }
        else if (palmDist > formMenuThresh - elSlotSize * 3 && palmDist <= formMenuThresh - elSlotSize * 2)
        {
            currEl = Element.fire;
            for (int i = 0; i < spellBook.orbDummies.Count; i++)
            {
                if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                else spellBook.orbDummies[i].SetActive(false);
            }
        }
        else if (palmDist > formMenuThresh - elSlotSize * 2 && palmDist <= formMenuThresh - elSlotSize)
        {
            currEl = Element.water;
            for (int i = 0; i < spellBook.orbDummies.Count; i++)
            {
                if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                else spellBook.orbDummies[i].SetActive(false);
            }
        }
        else if (palmDist > formMenuThresh - elSlotSize && palmDist <= formMenuThresh)
        {
            currEl = Element.ice;
            for (int i = 0; i < spellBook.orbDummies.Count; i++)
            {
                if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                else spellBook.orbDummies[i].SetActive(false);
            }
        }

        spellBook.orbDummies[elementID].transform.position = midpointPalms;
        spellBook.orbDummies[elementID].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }

    private void OrbScaler()
    {
        fromOrbScaler = true;

        for (int i = 0; i < spellBook.orbDummies.Count; i++)
        {
            if (i == elementID) spellBook.orbDummies[i].SetActive(true);
            else spellBook.orbDummies[i].SetActive(false);
        }

        spellBook.orbDummies[elementID].transform.position = midpointPalms;

        if (palmDist < formMenuThresh)
        {
            spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist * scaleMultiplier, palmDist * scaleMultiplier, palmDist * scaleMultiplier);
        }
        else if (palmDist >= formMenuThresh)
        {
            spellBook.orbDummies[elementID].transform.localScale = new Vector3(formMenuThresh * scaleMultiplier, formMenuThresh * scaleMultiplier, formMenuThresh * scaleMultiplier);
        }
    }

    private void CastOrb()
    {
        if (ableToCast)
        {
            GameObject spellOrb = Instantiate(spellBook.orbSpells[elementID], midpointPalms, Camera.main.transform.rotation) as GameObject;
            if (fromOrbScaler) spellOrb.transform.localScale = new Vector3(spellScale, spellScale, spellScale);
            else spellOrb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            StartCoroutine("CastDelay");
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
        for(int i = 0; i < spellBook.streamSpells.Count; i++)
        {
            if (i == elementID)
            {
                var emission = spellBook.streamSpells[i].emission;
                emission.enabled = true;
                Transform streamParent = spellBook.streamSpells[elementID].transform.parent;
                streamParent.position = midpointRockOn;

                foreach (Transform child in spellBook.streamSpells[elementID].transform)
                {
                    var childEmission = child.GetComponent<ParticleSystem>().emission;
                    childEmission.enabled = true;
                }
            }
            else
            {
                var emission = spellBook.streamSpells[i].emission;
                emission.enabled = false;
                Transform streamParent = spellBook.streamSpells[i].transform.parent;
                streamParent.position = midpointRockOn;

                foreach (Transform child in spellBook.streamSpells[i].transform)
                {
                    var childEmission = child.GetComponent<ParticleSystem>().emission;
                    childEmission.enabled = false;
                }
            }
        }
        
        
    }

    private void CastParticle()
    {
        if (ableToCast)
        {
            GameObject spellParticle = Instantiate(spellBook.particleSpells[elementID], rtIndexPos, Camera.main.transform.rotation);
            StartCoroutine("CastDelay");
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
}
