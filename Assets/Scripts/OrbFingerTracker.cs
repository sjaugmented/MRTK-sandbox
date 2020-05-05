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
    [Tooltip("How far in the palms have to face to trigger Selector menu")]
    [SerializeField] float palmInThresh = 0.3f;
    [SerializeField] float palmOutThresh = 0.5f;
    [SerializeField] float palmOutThresh2 = -0.5f;
    //[SerializeField] bool fingerCasting = true;

    // used for index tracking & velocity
    MixedRealityPose indexTip, rightPalm, leftPalm;
    float castFingerUpThresh = 0.3f;
    bool castFingerOut = false;
    //public bool oneFinger = false;
    public bool palmsIn = false;
    public bool palmsOut = false;
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
        // right then left
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out rightPalm))
        {
            // look for two palms
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out leftPalm))
            {
                //oneFinger = false;
                // look for palmsIn for form selector
                if (rightPalm.Right.x <= palmInThresh && leftPalm.Right.x <= palmInThresh)
                {
                    palmsIn = true;
                    palmsOut = false;
                    palmDist = Mathf.Abs(Vector3.Distance(rightPalm.Position, leftPalm.Position));
                }
                // look for palmsOut for casting
                else if (rightPalm.Up.y <= palmOutThresh && rightPalm.Up.x <= palmOutThresh && leftPalm.Up.y <= palmOutThresh && leftPalm.Up.x >= palmOutThresh2)
                {
                    palmsIn = false;
                    palmsOut = true;
                    palmDist = Mathf.Abs(Vector3.Distance(rightPalm.Position, leftPalm.Position));

                }
                else
                {
                    palmsIn = false;
                    palmsOut = false;
                }
            }
            /*else if (fingerCasting)
            {
                // look for single index tip
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out indexTip))
                {
                    oneFinger = true;

                    if (indexTip.Up.y >= castFingerUpThresh)
                    {
                        ProcessIndexVelocity();
                    }
                }
                else oneFinger = false;
            }*/
        }
        // left then right
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out leftPalm))
        {
            // look for two palms
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out rightPalm))
            {
                //oneFinger = false;
                // look for palmsIn for form selector
                if (rightPalm.Right.x <= palmInThresh && leftPalm.Right.x <= palmInThresh)
                {
                    palmsIn = true;
                    palmsOut = false;
                    palmDist = Mathf.Abs(Vector3.Distance(rightPalm.Position, leftPalm.Position));
                }
                // look for palmsOut for casting
                else if (rightPalm.Up.y <= palmOutThresh && rightPalm.Up.x <= palmOutThresh && leftPalm.Up.y <= palmOutThresh && leftPalm.Up.x >= palmOutThresh2)
                {
                    palmsIn = false;
                    palmsOut = true;
                    palmDist = Mathf.Abs(Vector3.Distance(rightPalm.Position, leftPalm.Position));
                }
                else
                {
                    palmsIn = false;
                    palmsOut = false;
                }
            }
            /*else if (fingerCasting)
            {
                // look for single index tip
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out indexTip))
                {
                    oneFinger = true;

                    if (indexTip.Up.y >= castFingerUpThresh)
                    {
                        ProcessIndexVelocity();
                    }
                }
                else oneFinger = false;
            }*/
        }
        // if no hands
        else
        {
            //oneFinger = false;
            palmsIn = false;
        }
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

    public bool GetPalmsOut()
    {
        return palmsOut;
    }

    public bool GetPalmsIn()
    {
        return palmsIn;
    }

    public float GetPalmDist()
    {
        return palmDist;
    }
}
