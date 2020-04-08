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
    float prevHandPosZ;
    float Zvelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MixedRealityPose pose;
        
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose) || HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out pose))
        {
            //Debug.Log("tracking single hand"); //todo remove

            cameraPos = Camera.main.transform.position;
            
            Zvelocity = pose.Position.z - prevHandPosZ;
            prevHandPosZ = pose.Position.z;

            //Debug.Log("velocity = " + velocity); //todo remove

            if (Zvelocity >= castThreshold)
            {
                Debug.Log("casting spell");
            }

        }
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose) || HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out pose))
        {
            Debug.Log("tracking two index fingers");
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
