using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] ParticleSystem spellParticle;

    Rigidbody rigidBody;

    [SerializeField] Transform targetRef;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float castThreshold = 1f;
        float currVelocity = rigidBody.velocity.magnitude;

        Debug.Log(currVelocity); //todo remove

        if (currVelocity >= castThreshold)
        {
            Debug.Log("Spell Cast"); //todo remove
            CastSpell();
        }
    }

    private void CastSpell()
    {
        spellParticle.Play();
    }
}
