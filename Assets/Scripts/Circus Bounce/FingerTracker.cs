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
    [Tooltip("")]
    [SerializeField] float fingerForwardThreshold = 0.7f;

    // used for index tracking & velocity
    MixedRealityPose firstIndex, secondIndex; 
    float castFingerUpThresh = 0.3f;
    bool castFingerOut = false;
    bool oneFinger = false;
    bool twoFingers = false;
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

            if (firstIndex.Up.y >= castFingerUpThresh)
            {
                ProcessIndexVelocity();
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out secondIndex))
            {
                oneFinger = true;
                twoFingers = true;

                distIndexes = Mathf.Abs(firstIndex.Position.x - secondIndex.Position.x);
            }
        }
        // if left index THEN right index
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out firstIndex))
        {
            oneFinger = true;
            twoFingers = false;

            if (firstIndex.Up.y >= castFingerUpThresh)
            {
                ProcessIndexVelocity();
            }

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out secondIndex))
            {
                oneFinger = true;
                twoFingers = true;

                distIndexes = Mathf.Abs(firstIndex.Position.x - secondIndex.Position.x);
            }
        }
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
                if (firstIndex.Forward.z >= fingerForwardThreshold)
                {
                    caster.CastSpell();
                }
                else return;
            }
            else return;
        }
        else if (caster.GetCurrForm() == SpellManager.Form.stream)
        {
            if (firstIndex.Forward.z >= fingerForwardThreshold)
            {
                caster.CastSpell();
            }
            else caster.DisableStreams();
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
