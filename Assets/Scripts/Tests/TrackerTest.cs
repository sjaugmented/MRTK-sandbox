using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    MixedRealityPose indexTip1, indexTip2, palm1, palm2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out indexTip1))
        {
            Debug.Log(indexTip1.Forward.z);
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Any, out palm1))
        {
            Debug.Log(palm1);
        }
    }
}
