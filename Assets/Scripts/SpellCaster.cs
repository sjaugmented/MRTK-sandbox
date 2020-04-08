using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] ParticleSystem spellParticle;

    [SerializeField] Transform targetRef;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell()
    {
        spellParticle.Play();
    }
}
