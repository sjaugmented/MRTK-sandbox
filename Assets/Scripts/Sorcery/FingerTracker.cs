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
    MixedRealityPose indexRight, indexLeft; 
    float castFingerUp = 0.3f;
    bool indexPresent = false;
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
        // if 2 indexes
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out indexRight) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out indexLeft))
        {
            indexPresent = true;
            twoFingers = true;

            distIndexes = indexRight.Position.x - indexLeft.Position.x;

            if (indexRight.Up.y >= castFingerUp)
            {
                ProcessIndexVelocity();
            }
        }
        // if right hand index only
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out indexRight))
        {
            indexPresent = true;
            twoFingers = false;

            if (indexRight.Up.y >= castFingerUp)
            {
                ProcessIndexVelocity();
            }
        }
        // if no indexes
        else
        {
            indexPresent = false;
            twoFingers = false;
        }
    }

    private void ProcessIndexVelocity()
    {
        float awayVelocity;
        Vector3 cameraPos = Camera.main.transform.position;

        float handCamDist = Mathf.Abs(Vector3.Distance(cameraPos, indexRight.Position));
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Mathf.Abs(Vector3.Distance(cameraPos, indexRight.Position));

        if (awayVelocity >= minVelocity && awayVelocity <= maxVelocity)
        {
            caster.CastSpell();
        }
    }

    public bool GetTwoFingers()
    {
        return twoFingers;
    }

    public float GetDistIndexes()
    {
        return distIndexes;
    }

    public bool GetCastFingerUp()
    {
        return indexPresent;
    }
}
