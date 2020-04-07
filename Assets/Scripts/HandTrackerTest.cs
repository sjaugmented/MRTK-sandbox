using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [SerializeField] float castThreshold = 0.5f;

    Vector3 cameraPos;
    Vector3 prevHandPos;
    float velocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MixedRealityPose right;
        //MixedRealityPose left;

        var rightHand = HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out right);
        //var leftHand = HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out left);


        if (rightHand)
        {
            Debug.Log("tracking single hand");

            cameraPos = Camera.main.transform.position;
            prevHandPos = right.Position;
            velocity = (right.Position - prevHandPos).magnitude;

            Debug.Log("velocity = " + velocity);

        }
        else
        {
            return;
        }
    }

    private void TrackHand(MixedRealityPose pose)
    {
        // This runs if your "try get joint pose" succeeds. If you get into this block, then pose will be defined
        // Take a look at this
        // https://microsoft.github.io/MixedRealityToolkit-Unity/api/Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose.html#Microsoft_MixedReality_Toolkit_Utilities_MixedRealityPose_Position

        //Debug.Log(pose.Position);
        //Debug.Log(pose.Up);

        
    }
}
