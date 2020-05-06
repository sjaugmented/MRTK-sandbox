using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFingerTracker : MonoBehaviour
{
    [Header("Thresholds")]
    [Tooltip("Min Velocity at which spells are cast")]
    [SerializeField] float minVelocity = 2f;
    [Tooltip("Max Velocity at which spells are cast")]
    [SerializeField] float maxVelocity = 10f;
    [Tooltip("How far forward the finger must point before casting can happen")]
    [SerializeField] float fingerForwardThreshold = 0.7f;
    [Tooltip("Margin between hero angles of 0 and 180")]
    [SerializeField] float angleMargin = 20f;
    //[SerializeField] bool fingerCasting = true;

    // used for index tracking & velocity
    MixedRealityPose indexTip, rightPalm, leftPalm;
    float castFingerUpThresh = 0.3f;
    bool castFingerOut = false;
    //public bool oneFinger = false;
    public bool touchDown = false;
    public bool palmsForward = false;
    public bool palmsIn = false;
    public bool twoPalms = false;
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
        Transform cam = Camera.main.transform;
        
        // look for two palms
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out rightPalm) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out leftPalm))
        {
            twoPalms = true;
            palmDist = Mathf.Abs(Vector3.Distance(rightPalm.Position, leftPalm.Position));

            // get angle of palms to cam.Forward
            float rtPalmUpAng = Vector3.Angle(rightPalm.Up, cam.forward);
            float ltPalmUpAng = Vector3.Angle(leftPalm.Up, cam.forward);
            float rtPalmForAng = Vector3.Angle(rightPalm.Forward, cam.forward);
            float ltPalmForAng = Vector3.Angle(leftPalm.Forward, cam.forward);
            float rtPalmRtAng = Vector3.Angle(leftPalm.Right, cam.forward);
            float ltPalmRtAng = Vector3.Angle(leftPalm.Right, cam.forward);

            // look for touchDown pose
            if (IsWithinRange(rtPalmUpAng, 90) && IsWithinRange(ltPalmUpAng, 90) && IsWithinRange(rtPalmForAng, 90) && IsWithinRange(ltPalmForAng, 90))
            {
                touchDown = true;
                palmsIn = false;
                palmsForward = false;
            }

            // look for palmsIn pose
            if (IsWithinRange(rtPalmUpAng, 90) && IsWithinRange(ltPalmUpAng, 90) && IsWithinRange(rtPalmForAng, 0) && IsWithinRange(ltPalmForAng, 0) && IsWithinRange(rtPalmRtAng, 90) && IsWithinRange(ltPalmRtAng, 90))
            {
                touchDown = false;
                palmsIn = true;
                palmsForward = false;
            }

            // look for palmsOut pose
            if (IsWithinRange(rtPalmUpAng, 180) && IsWithinRange(ltPalmUpAng, 180) && IsWithinRange(rtPalmForAng, 90) && IsWithinRange(ltPalmForAng, 90) && IsWithinRange(rtPalmRtAng, 90) && IsWithinRange(ltPalmRtAng, 90))
            {
                touchDown = false;
                palmsIn = false;
                palmsForward = true;
            }
        }
        // if no hands
        else
        {
            twoPalms = false;
            touchDown = false;
            palmsIn = false;
            palmsForward = false;
        }
    }

    private bool IsWithinRange(float testVal, float target)
    {
        bool withinRange = false;

        if (target == 0)
        {
            if (testVal <= target + angleMargin) withinRange = true;
        }
        else if (target == 180)
        {
            if (testVal >= 180 - angleMargin) withinRange = true;
        }
        else if (target > 0 && target < 180)
        {
            if (testVal >= target - angleMargin && testVal <= target + angleMargin) withinRange = true;
        }
        else withinRange = false;

        return withinRange;
    }

    /* private void ProcessIndexVelocity()
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
     }*/

    /* public bool GetCastFingerUp()
     {
         return oneFinger;
     }*/

    public Vector3 GetPalm1Pos()
    {
        return rightPalm.Position;
    }

    public Vector3 GetPalm2Pos()
    {
        return leftPalm.Position;
    }

    public bool GetPalmsForward()
    {
        return palmsForward;
    }

    public bool GetTouchdown()
    {
        return touchDown;
    }

    public bool GetPalmsIn()
    {
        return palmsIn;
    }

    public float GetPalmDist()
    {
        return palmDist;
    }

    public bool GetTwoPalms()
    {
        return twoPalms;
    }
}
