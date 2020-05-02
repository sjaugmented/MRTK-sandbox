using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour, IMixedRealityHand
{
    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    bool IMixedRealityController.Enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    TrackingState IMixedRealityController.TrackingState => throw new System.NotImplementedException();

    Handedness IMixedRealityController.ControllerHandedness => throw new System.NotImplementedException();

    IMixedRealityInputSource IMixedRealityController.InputSource => throw new System.NotImplementedException();

    IMixedRealityControllerVisualizer IMixedRealityController.Visualizer => throw new System.NotImplementedException();

    bool IMixedRealityController.IsPositionAvailable => throw new System.NotImplementedException();

    bool IMixedRealityController.IsPositionApproximate => throw new System.NotImplementedException();

    bool IMixedRealityController.IsRotationAvailable => throw new System.NotImplementedException();

    MixedRealityInteractionMapping[] IMixedRealityController.Interactions => throw new System.NotImplementedException();

    Vector3 IMixedRealityController.AngularVelocity => throw new System.NotImplementedException();

    Vector3 IMixedRealityController.Velocity => throw new System.NotImplementedException();

    bool IMixedRealityController.IsInPointingPose => throw new System.NotImplementedException();

    bool IMixedRealityHand.TryGetJoint(TrackedHandJoint joint, out MixedRealityPose pose)
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
         
    }
}
