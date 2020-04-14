using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterSelectorTest : MonoBehaviour
{
    [SerializeField] bool light;
    [SerializeField] bool fire;
    [SerializeField] bool water;
    [SerializeField] bool wind;
    [SerializeField] bool earth;
    [Tooltip("Visual representation of Light spell")] [SerializeField] GameObject lightCaster;
    [Tooltip("Visual representation of Fire spell")] [SerializeField] GameObject fireCaster;
    [Tooltip("Visual representation of Water spell")] [SerializeField] GameObject waterCaster;
    [Tooltip("Visual representation of Wind spell")] [SerializeField] GameObject windCaster;
    [Tooltip("Visual representation of Earth spell")] [SerializeField] GameObject earthCaster;

    void Awake()
    {
        lightCaster.SetActive(false);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCaster()
    {   
        lightCaster.SetActive(false);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);

        if (light) lightCaster.SetActive(true);
        if (fire) fireCaster.SetActive(true);
        if (water) waterCaster.SetActive(true);
        if (wind) windCaster.SetActive(true);
        if (earth) earthCaster.SetActive(true);


    }
}
