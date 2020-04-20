using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDimmerScaler : MonoBehaviour
{
    [SerializeField]
    public enum Finger
    {
        Index,
        Middle,
        Ring,
        Pinky
    }

    public Finger finger;

    /*[Range(-0.1f, 0.1f)]
    [SerializeField] float xPosOffset = 0;
    [Range(-0.1f, 0.1f)]
    [SerializeField] float yPosOffset = 0;
    [Range(-0.1f, 0.1f)]
    [SerializeField] float zPosOffset = 0;*/

    Vector3 tipPos, knucklePos;
    float fingerLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScaleToFinger();
    }

    private void ScaleToFinger()
    {
        MixedRealityPose fingerTip, knuckle;

        // get fingertip and knuckle positions

        if (finger == Finger.Index)
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out fingerTip) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Left, out knuckle))
            {
                tipPos = fingerTip.Position;
                knucklePos = knuckle.Position;
                fingerLength = Vector3.Distance(fingerTip.Position, knuckle.Position);

                Vector3 rawMidpoint = Vector3.Lerp(fingerTip.Position, knuckle.Position, 0.5f);
                //Vector3 adjustedMidpoint = new Vector3(xPosOffset, yPosOffset, zPosOffset);

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knuckle.Position - fingerTip.Position);

                // set dimmer position to midpoint
                transform.position = rawMidpoint;
                //transform.localPosition = adjustedMidpoint;
                // set dimmer size to pose distance
                transform.localScale = new Vector3(0.01f, fingerLength, 1f);
                // set dimmer rotation
                transform.rotation = rotation;

            }
            else return;
        }
        else if (finger == Finger.Middle)
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Left, out fingerTip) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleKnuckle, Handedness.Left, out knuckle))
            {
                tipPos = fingerTip.Position;
                knucklePos = knuckle.Position;
                fingerLength = Vector3.Distance(fingerTip.Position, knuckle.Position);

                Vector3 rawMidpoint = Vector3.Lerp(fingerTip.Position, knuckle.Position, 0.5f);
                //Vector3 adjustedMidpoint = new Vector3(xPosOffset, yPosOffset, zPosOffset);

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knuckle.Position - fingerTip.Position);

                // set dimmer position to midpoint
                transform.position = rawMidpoint;
                //transform.localPosition = adjustedMidpoint;
                // set dimmer size to pose distance
                transform.localScale = new Vector3(0.01f, fingerLength, 1f);
                // set dimmer rotation
                transform.rotation = rotation;

            }
            else return;
        }
        else if (finger == Finger.Ring)
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Left, out fingerTip) && HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, Handedness.Left, out knuckle))
            {
                tipPos = fingerTip.Position;
                knucklePos = knuckle.Position;
                fingerLength = Vector3.Distance(fingerTip.Position, knuckle.Position);

                Vector3 rawMidpoint = Vector3.Lerp(fingerTip.Position, knuckle.Position, 0.5f);
                //Vector3 adjustedMidpoint = new Vector3(xPosOffset, yPosOffset, zPosOffset);

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knuckle.Position - fingerTip.Position);

                // set dimmer position to midpoint
                transform.position = rawMidpoint;
                //transform.localPosition = adjustedMidpoint;
                // set dimmer size to pose distance
                transform.localScale = new Vector3(0.01f, fingerLength, 1f);
                // set dimmer rotation
                transform.rotation = rotation;

            }
            else return;
        }
        else if (finger == Finger.Pinky)
        {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Left, out fingerTip) && HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, Handedness.Left, out knuckle))
            {
                tipPos = fingerTip.Position;
                knucklePos = knuckle.Position;
                fingerLength = Vector3.Distance(fingerTip.Position, knuckle.Position);

                Vector3 rawMidpoint = Vector3.Lerp(fingerTip.Position, knuckle.Position, 0.5f);
                //Vector3 adjustedMidpoint = new Vector3(xPosOffset, yPosOffset, zPosOffset);

                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knuckle.Position - fingerTip.Position);

                // set dimmer position to midpoint
                transform.position = rawMidpoint;
                //transform.localPosition = adjustedMidpoint;
                // set dimmer size to pose distance
                transform.localScale = new Vector3(0.01f, fingerLength, 1f);
                // set dimmer rotation
                transform.rotation = rotation;

            }
            else return;
        }
        else return;
    }

    public Vector3 GetIndexTip() 
    {
        return tipPos;
    }

    public Vector3 GetIndexKnuckle()
    {
        return knucklePos;
    }

    public float GetFingerLength()
    {
        return fingerLength;
    }
}
