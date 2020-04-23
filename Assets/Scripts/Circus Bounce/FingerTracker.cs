using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTracker : MonoBehaviour
{
    [Header("Casting thresholds")]
    [Tooltip("Min Velocity at which spells are cast")] 
    [SerializeField] float minVelocity = 2f;
    [Tooltip("Max Velocity at which spells are cast")] 
    [SerializeField] float maxVelocity = 10f;

    // used for index tracking & velocity
    MixedRealityPose firstIndex, secondIndex; 
    float castFingerUp = 0.3f;
    public bool oneFinger = false;
    public bool twoFingers = false;
    float distIndexes;
    float prevHandCamDist;

    SpellManager caster;

    // Start is called before the first frame update
    void Start()
    {
        caster = FindObjectOfType<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIndexes();
    }

    private void ProcessIndexes()
    {
        // if right index THEN left index
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out firstIndex)) 
        {
            oneFinger = true;
            twoFingers = false;

            if (firstIndex.Up.y >= castFingerUp)
            {
                ProcessIndexVelocity();
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out secondIndex))
            {
                oneFinger = true;
                twoFingers = true;

                distIndexes = firstIndex.Position.x - secondIndex.Position.x;
            }
        }
        // if left index THEN right index
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out firstIndex))
        {
            oneFinger = true;
            twoFingers = false;

            if (firstIndex.Up.y >= castFingerUp)
            {
                ProcessIndexVelocity();
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out secondIndex))
            {
                oneFinger = true;
                twoFingers = true;

                distIndexes = firstIndex.Position.x - secondIndex.Position.x;
            }
        }
        // if one index only
        /*else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out firstIndex))
        {
            oneFinger = true;
            twoFingers = false;

            if (firstIndex.Up.y >= castFingerUp)
            {
                ProcessIndexVelocity();
            }
        }*/
        // if no indexes
        else
        {
            oneFinger = false;
            twoFingers = false;
        }
    }

    private void ProcessIndexVelocity()
    {
        if (caster.GetCurrForm() == SpellManager.Form.particle || caster.GetCurrForm() == SpellManager.Form.orb)
        {
            float awayVelocity;
            Vector3 cameraPos = Camera.main.transform.position;

            float handCamDist = Mathf.Abs(Vector3.Distance(cameraPos, firstIndex.Position));
            awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
            prevHandCamDist = Mathf.Abs(Vector3.Distance(cameraPos, firstIndex.Position));

            if (awayVelocity >= minVelocity && awayVelocity <= maxVelocity)
            {
                caster.CastSpell();
            }
        }
        else if (caster.GetCurrForm() == SpellManager.Form.stream)
        {
            Debug.Log("index rotation: " + firstIndex.Rotation);
            
        }
        else return;
    }

    public bool GetCastFingerUp()
    {
        return oneFinger;
    }
    public bool GetTwoFingers()
    {
        return twoFingers;
    }

    public float GetDistIndexes()
    {
        return distIndexes;
    }
}
