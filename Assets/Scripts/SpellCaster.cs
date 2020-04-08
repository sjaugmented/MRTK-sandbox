using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] ParticleSystem spellParticle;
    [SerializeField] GameObject spellHolo;
    [SerializeField] Transform targetRef;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell(Vector3 position, float forwardVel)
    {
        spellParticle.Play();
        Instantiate(spellHolo, position, Quaternion.identity);
        spellHolo.GetComponent<Rigidbody>().velocity = spellHolo.transform.forward * (forwardVel * 10);
    }
}
