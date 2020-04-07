using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [SerializeField] float handPosition = 0f;
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
    }
}
