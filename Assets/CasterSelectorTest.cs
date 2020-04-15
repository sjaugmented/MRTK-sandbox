using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterSelectorTest : MonoBehaviour
{
    [SerializeField] bool lightSelector;
    [SerializeField] bool fireSelector;
    [SerializeField] bool waterSelector;
    [SerializeField] bool windSelector;
    [SerializeField] bool earthSelector;

    public GameObject trueCaster;
    GameObject lightCaster, fireCaster, waterCaster, windCaster, earthCaster;

    public void SelectCaster()
    {
        if (lightSelector) trueCaster = lightCaster;
        if (fireSelector) trueCaster = fireCaster;
        if (waterSelector) trueCaster = waterCaster;
        if (windSelector) trueCaster = windCaster;
        if (earthSelector) trueCaster = earthCaster;
    }

    public GameObject GetTrueCaster()
    {
        return trueCaster;
    }
}
