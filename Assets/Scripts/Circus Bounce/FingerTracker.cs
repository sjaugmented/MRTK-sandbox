using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTracker : MonoBehaviour
{
    [Header("Thresholds")]
    [Tooltip("Min Velocity at which spells are cast")] 
    [SerializeField] float minVelocity = 2f;
    [Tooltip("Max Velocity at which spells are cast")] 
    [SerializeField] float maxVelocity = 10f;
    [Tooltip("How far forward the finger must point before casting can happen")]
    [SerializeField] float fingerForwardThreshold = 0.7f;
    [Tooltip("How far in the palms have to face to trigger Selector menu")]
    [SerializeField] float palmInThresh = 0.3f;


    // used for index tracking & velocity
    MixedRealityPose indexTip, palm1, palm2;
    float castFingerUpThresh = 0.3f;
    bool castFingerOut = false;
    public bool oneFinger = false;
    public bool palmsIn = false;
    float palmDist;
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
        ProcessHands();
    }

    private void ProcessHands()
    {
        Debug.Log("palm1.right.x: " + palm1.Right.x);
        Debug.Log("palm2.right.x: " + palm2.Right.x);

        // look for palms in
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out palm1) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out palm2))
        {
            oneFinger = false;

            if (palm1.Right.x <= palmInThresh && palm2.Right.x <= palmInThresh)
            {
                palmsIn = true;
                palmDist = Mathf.Abs(Vector3.Distance(palm1.Position, palm2.Position));
            }
            else
            {
                palmsIn = false;
            }
        }
        // look for casting finger
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out indexTip))
        {
            oneFinger = true;

            if (indexTip.Up.y >= castFingerUpThresh)
            {
                ProcessIndexVelocity();
            }
        }
        // if no hands
        else
        {
            oneFinger = false;
            palmsIn = false;
        }
    }

    private void ProcessIndexVelocity()
    {
        if (caster.GetCurrForm() == SpellManager.Form.particle || caster.GetCurrForm() == SpellManager.Form.orb)
        {
            float awayVelocity;
            Vector3 cameraPos = Camera.main.transform.position;

            float handCamDist = Mathf.Abs(Vector3.Distance(cameraPos, indexTip.Position));
            awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
            prevHandCamDist = Mathf.Abs(Vector3.Distance(cameraPos, indexTip.Position));

            if (awayVelocity >= minVelocity && awayVelocity <= maxVelocity)
            {
                if (indexTip.Forward.z >= fingerForwardThreshold)
                {
                    caster.CastSpell();
                }
                else return;
            }
            else return;
        }
        else if (caster.GetCurrForm() == SpellManager.Form.stream)
        {
            if (indexTip.Forward.z >= fingerForwardThreshold)
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

    public bool GetPalmsIn()
    {
        return palmsIn;
    }

    public float GetPalmDist()
    {
        return palmDist;
    }

    public Vector3 GetPalm1Pos()
    {
        return palm1.Position;
    }

    public Vector3 GetPalm2Pos()
    {
        return palm2.Position;
    }
}
