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

    [Header("Palm Spellbook")]
    [Tooltip("If false, turn off Palm Menu solvers")] 
    [SerializeField] bool usePalmMenu = true;
    [Tooltip("Parent object for the palm menu visuals")]
    [SerializeField] GameObject palmMenuVisuals;

    // used for index tracking & velocity
    MixedRealityPose indexRight, indexLeft; 
    float fingerUp = 0.3f;
    float distIndexes;
    float prevHandCamDist;

    bool indexPresent = false;
    bool twoFingers = false;

    // used to disable index tracking when menu is open
    bool menuOpen = false;

    SpellManager caster;

    // Start is called before the first frame update
    void Start()
    {
        caster = FindObjectOfType<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (usePalmMenu)
        {
            // only track index if menu is closed
            if (!palmMenuVisuals.activeInHierarchy) ProcessIndexes();
            else return;
        }
        else
        {
            ProcessIndexes();
        }
    }

    private void ProcessIndexes()
    {
        // if 2 indexes
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out indexRight) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out indexLeft))
        {
            indexPresent = true;
            twoFingers = true;

            distIndexes = Vector3.Distance(indexRight.Position, indexLeft.Position);

            if (indexRight.Up.y >= fingerUp)
            {
                ProcessIndexVelocity();
            }
        }
        // if right hand index only
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out indexRight))
        {
            indexPresent = true;
            twoFingers = false;

            if (indexRight.Up.y >= fingerUp)
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

        float handCamDist = Vector3.Distance(cameraPos, indexRight.Position);
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Vector3.Distance(cameraPos, indexRight.Position);

        if (awayVelocity >= minVelocity && awayVelocity <= maxVelocity)
        {
            Debug.Log("casting");
            caster.CastTestSpell();
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

    public bool GetFingerUp()
    {
        return indexPresent;
    }
}
