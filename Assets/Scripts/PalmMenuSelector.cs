using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmMenuSelector : MonoBehaviour
{
    [Header("PICK ONLY ONE!")]
    [SerializeField] bool light;
    [SerializeField] bool fire;
    [SerializeField] bool water;
    [SerializeField] bool wind;
    [SerializeField] bool earth;

    HandTrackerTest hand;

    // Start is called before the first frame update
    void Start()
    {
        hand = FindObjectOfType<HandTrackerTest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectSpell()
    {
        if (light) hand.CastLight();
        if (fire) hand.CastFire();
        if (water) hand.CastWater();
        if (wind) hand.CastWind();
        if (earth) hand.CastEarth();
    }
}
