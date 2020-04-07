using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [SerializeField] float handPosition = 0f;
    [SerializeField] float castThreshold = 0.5f;
    public bool handTracking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rightHandIndex = HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out _);
        var leftHandIndex = HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out _);


        if (rightHandIndex)
        {
            handTracking = true;
            Debug.Log("tracking right hand");
            

        }
        else if (leftHandIndex)
        {
            handTracking = true;
            Debug.Log("tracking left hand");
        }
        else
        {
            handTracking = false;
        }

        MixedRealityPose pose;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose))
        {
            // This runs if your "try get joint pose" succeeds. If you get into this block, then pose will be defined
            // Take a look at this
            // https://microsoft.github.io/MixedRealityToolkit-Unity/api/Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose.html#Microsoft_MixedReality_Toolkit_Utilities_MixedRealityPose_Position

            Debug.Log(pose.Position);
            Debug.Log(pose.Up);

            Vector3 lastHandPos = pose.Position;
            var handPosDistance = (pose.Position - lastHandPos);
            var handVelocity = handPosDistance.magnitude / Time.deltaTime;

            Debug.Log(handVelocity);

            
        }
    }
}
