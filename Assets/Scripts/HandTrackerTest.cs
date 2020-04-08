using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [Tooltip("Velocity at which spells are cast")] [SerializeField] float castThreshold = 0.5f;
    //[Tooltip("GameObject that casts spells")] [SerializeField] GameObject spellCastObj;
    [SerializeField] GameObject spellHolo;


    public float prevHandCamDist; //
    public float awayVelocity;    // todo remove public

    //SpellCaster spellCaster;

    // Start is called before the first frame update
    void Start()
    {
        /*spellCaster = FindObjectOfType<SpellCaster>();
        spellCaster.gameObject.SetActive(false);*/

    }

    // Update is called once per frame
    void Update()
    {
        MixedRealityPose pose, rightPose, leftPose;
        
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose) || HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out pose))
        {
            // This runs if your "try get joint pose" succeeds. If you get into this block, then pose will be defined
            // Take a look at this
            // https://microsoft.github.io/MixedRealityToolkit-Unity/api/Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose.html#Microsoft_MixedReality_Toolkit_Utilities_MixedRealityPose_Position

            //Debug.Log(pose.Position);
            //Debug.Log(pose.Up);

            //Debug.Log("tracking single hand"); //todo remove

            //spellCaster.gameObject.SetActive(true);
            TrackHandVelocity(pose);

        }
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out rightPose) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out leftPose))
        {
            // This runs if your "try get joint pose" succeeds. If you get into this block, then pose will be defined
            // Take a look at this
            // https://microsoft.github.io/MixedRealityToolkit-Unity/api/Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose.html#Microsoft_MixedReality_Toolkit_Utilities_MixedRealityPose_Position

            //Debug.Log(pose.Position);
            //Debug.Log(pose.Up);

            Debug.Log("tracking two index fingers");
            //spellCaster.gameObject.SetActive(false);

        }
        else
        {
            //spellCaster.gameObject.SetActive(false);
        }
    }

    private void TrackHandVelocity(MixedRealityPose pose)
    {
        // tracks velocity of joint away from camera; if greater than castThreshold then cast spell
        
        Vector3 cameraPos = Camera.main.transform.position;

        float handCamDist = Vector3.Distance(cameraPos, pose.Position);
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Vector3.Distance(cameraPos, pose.Position);

        

        //Debug.Log(awayVelocity); //todo remove

        if (awayVelocity >= castThreshold)
        {
            Debug.Log("casting spell"); //todo remove
            CastSpell(pose.Position, pose.Rotation, awayVelocity);

        }
    }

    public void CastSpell(Vector3 position, Quaternion rotation, float forwardVel)
    {
        //spellParticle.Play();
        Instantiate(spellHolo, position, rotation);
        spellHolo.GetComponent<Rigidbody>().velocity = spellHolo.transform.forward * forwardVel;
    }

}
