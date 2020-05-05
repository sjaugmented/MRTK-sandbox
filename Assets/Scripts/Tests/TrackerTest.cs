using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour, IMixedRealityHand
{
    [SerializeField] bool palmUp = false;
    [SerializeField] bool palmRight = false;
    [SerializeField] bool palmForward = false;
    [SerializeField] bool palmPosition = false;
    [SerializeField] bool palmRotation = false;

    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    public bool Enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public TrackingState TrackingState => throw new System.NotImplementedException();

    public Handedness ControllerHandedness => throw new System.NotImplementedException();

    public IMixedRealityInputSource InputSource => throw new System.NotImplementedException();

    public IMixedRealityControllerVisualizer Visualizer => throw new System.NotImplementedException();

    public bool IsPositionAvailable => throw new System.NotImplementedException();

    public bool IsPositionApproximate => throw new System.NotImplementedException();

    public bool IsRotationAvailable => throw new System.NotImplementedException();

    public MixedRealityInteractionMapping[] Interactions => throw new System.NotImplementedException();

    public Vector3 AngularVelocity => throw new System.NotImplementedException();

    public Vector3 Velocity => throw new System.NotImplementedException();

    public bool IsInPointingPose => throw new System.NotImplementedException();

    public bool TryGetJoint(TrackedHandJoint joint, out MixedRealityPose pose)
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
         if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Any, out palm1))
        {
            if (palmUp) Debug.Log("palm1.Up: " + palm1.Up);
            if (palmRight) Debug.Log("palm1.Right: " + palm1.Right);
            if (palmForward) Debug.Log("palm1.Forward: " + palm1.Forward);
            if (palmPosition) Debug.Log("palm.Position: " + palm1.Position);
            if (palmRotation) Debug.Log("palm.Rotation: " + palm1.Rotation);
        }
    }
}
