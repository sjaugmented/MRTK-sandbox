using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] float delayBetweenCasts = 0.2f;

    [Header("Two Finger Spellbook")]
    [Tooltip("Distance between index fingers that activates Spellbook")]
    [SerializeField] float formMenuThresh = 0.3f;
    [SerializeField] float scaleMultiplier = 1f;

    [Header("OSC controller")]
    [SerializeField] string conjureMessageOSC = "conjure message here";
    
    float conjureValueOSC = 0;
    public float spellScale = 1;

    // used to create rate of fire for spells
    bool ableToCast = true;

    public enum Element { light, fire, water, ice };
    public enum Form { particle, orb, stream };
    public Element currEl = Element.light;
    public Form currForm = Form.orb;
    int elementID = 0;

    bool conjuring = false;

    OrbFingerTracker fingerTracker;
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
        fingerTracker = FindObjectOfType<OrbFingerTracker>();
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
        CalcPalmPositions();
        conjureValueOSC = palmDist / formMenuThresh;

        bool palmsIn = fingerTracker.GetPalmsIn();
        bool palmsOut = fingerTracker.GetPalmsOut();

        if (palmsIn)
        {
            conjuring = true;
            OrbSelector();
        }
        else
        {
            conjuring = false;
            DisableOrbDummies();
        }

        if (palmsOut) CastOrb();

        if (conjuring) SendOSCMessage(conjureMessageOSC, conjureValueOSC);
        else return;
    }

    private void SendOSCMessage(string address, float value)
    {
        OscMessage message = new OscMessage();
        message.address = address;
        message.values.Add(value);
        osc.Send(message);
        //Debug.Log("Sending OSC: " + address + " " + value); // todo remove
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

        if (palmDist > 0 && palmDist <= formMenuThresh)
        {
            for (int i = 0; i < spellBook.orbDummies.Count; i++)
            {
                if (i == elementID) spellBook.orbDummies[i].SetActive(true);
                else spellBook.orbDummies[i].SetActive(false);
            }

            spellBook.orbDummies[elementID].transform.position = midpoint;
            spellBook.orbDummies[elementID].transform.localScale = new Vector3(palmDist * scaleMultiplier, palmDist * scaleMultiplier, palmDist * scaleMultiplier);
            
            spellScale = palmDist * scaleMultiplier;
        }
        else DisableOrbDummies();
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
}
