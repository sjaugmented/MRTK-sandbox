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

    bool conjuring = false;

    // coordinates for conjuring
    Vector3 midpoint;
    Vector3 palm1Pos;
    Vector3 palm2Pos;
    float palmDist;

    // used to create rate of fire for spells
    bool ableToCast = true;

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
        ConfigOctas();
        
        if (!palmMenuVisuals.activeInHierarchy)
        {
            bool twoPalms = handTracking.GetTwoPalms();
            bool palmsIn = handTracking.GetPalmsIn();
            bool palmsOut = handTracking.GetPalmsOut();

            CalcPalmPositions();

            if (twoPalms)
            {
                if (palmsIn)
                {
                    //conjuring = true;
                    OrbSelector();

                    conjureValueOSC = palmDist / formMenuThresh;

                    if (conjureValueOSC < 0) conjureValueOSC = 0;
                    if (conjureValueOSC > 1) conjureValueOSC = 1;
                    SendOSCMessage(conjureOSCMessages[elementID], conjureValueOSC);

                }
                else
                {
                    //conjuring = false;
                    DisableOrbDummies();
                }

                if (palmsOut) CastOrb();

                /*conjureValueOSC = palmDist / formMenuThresh;
                if (conjuring)
                {
                    if (conjureValueOSC >= 0 && conjureValueOSC <= 1)
                    {
                        SendOSCMessage(conjureOSCMessages[elementID], conjureValueOSC);
                    }
                }
                else return;*/
            }
            else
            {
                //conjuring = false;
                DisableOrbDummies();
            }
        } 
        else
        {
            DisableOrbDummies();
            //conjuring = false;

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

    private void CalcPalmPositions()
    {
        palmDist = handTracking.GetPalmDist();
        palm1Pos = handTracking.GetPalm1Pos();
        palm2Pos = handTracking.GetPalm2Pos();

        midpoint = Vector3.Lerp(palm1Pos, palm2Pos, 0.5f);

        if (palmDist < formMenuThresh) spellScale = palmDist * scaleMultiplier;
        if (palmDist >= formMenuThresh) spellScale = formMenuThresh * scaleMultiplier;
    }

    private void OrbSelector()
    {
        currForm = Form.orb;

        for (int i = 0; i < spellBook.orbDummies.Count; i++)
        {
            if (i == elementID) spellBook.orbDummies[i].SetActive(true);
            else spellBook.orbDummies[i].SetActive(false);
        }

        spellBook.orbDummies[elementID].transform.position = midpoint;

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
            GameObject spell = Instantiate(spellBook.orbSpells[elementID], midpoint, Camera.main.transform.rotation) as GameObject;
            spell.transform.localScale = new Vector3(spellScale, spellScale, spellScale);
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
