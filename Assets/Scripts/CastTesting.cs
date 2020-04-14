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

    Transform castPosition;
    GameObject spellToCast;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForActiveCaster();
    }

    private void CheckForActiveCaster()
    {
        if (lightCaster.activeInHierarchy)
        {
            spellToCast = lightSpell;
            castPosition = lightCaster.transform;
        }
        else if (fireCaster.activeInHierarchy)
        {
            spellToCast = fireSpell;
            castPosition = fireCaster.transform;
        }
        else if (waterCaster.activeInHierarchy)
        {
            spellToCast = waterSpell;
            castPosition = waterCaster.transform;
        }
        else if (windCaster.activeInHierarchy)
        {
            spellToCast = windSpell;
            castPosition = windCaster.transform;
        }
        else if(earthCaster.activeInHierarchy)
        {
            spellToCast = earthSpell;
            castPosition = earthCaster.transform;
        }
    }

    public void CastTestSpell()
    {
        if (!ableToCast) return;
        if (ableToCast)
        {
            Instantiate(spellToCast, castPosition.position, Camera.main.transform.rotation);
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
