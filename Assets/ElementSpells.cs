using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpells : MonoBehaviour
{
    [SerializeField] SpellSlot[] elementalForms;
    
    [System.Serializable]
    private class SpellSlot
    {
        public Elementals elementType;
        public GameObject particle;
        public GameObject orb;
        public GameObject thrower;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
