using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    [SerializeField] bool palmUp = false;
    [SerializeField] bool palmRight = false;
    [SerializeField] bool palmForward = false;
    [SerializeField] bool palmPosition = false;
    [SerializeField] bool palmRotation = false;

    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    void Update()
    {
         if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out palm1) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out palm2))
        {
            Debug.Log(Vector3.Angle(palm1.Up, palm2.Up));

            if (palmUp) Debug.Log("palm1.Up: " + palm1.Up);
            if (palmRight) Debug.Log("palm1.Right: " + palm1.Right);
            if (palmForward) Debug.Log("palm1.Forward: " + palm1.Forward);
            if (palmPosition) Debug.Log("palm.Position: " + palm1.Position);
            if (palmRotation) Debug.Log("palm.Rotation: " + palm1.Rotation);
        }
    }
}
