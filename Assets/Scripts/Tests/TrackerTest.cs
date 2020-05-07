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
    [SerializeField] bool palmToPalm = false;

    MixedRealityPose rightIndex, rightThumb, leftPalm, rightPalm;

    void Update()
    {
         if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out leftPalm) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out rightPalm) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out rightIndex) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Right, out rightThumb))
        {
            Debug.Log("thumb: " + Vector3.Angle(rightThumb.Forward, rightPalm.Forward));
            Debug.Log("index: " + Vector3.Angle(rightIndex.Forward, rightPalm.Forward));
            
            if (palmUp)
            {
                Debug.Log("leftUp/CamFor: " + Vector3.Angle(leftPalm.Up, Camera.main.transform.forward));
                Debug.Log("rightUp/CamFor: " + Vector3.Angle(rightPalm.Up, Camera.main.transform.forward));
            }

            if (palmRight)
            {
                Debug.Log("leftRight/CamFor: " + Vector3.Angle(leftPalm.Right, Camera.main.transform.forward));
                Debug.Log("rightRight/CamFor: " + Vector3.Angle(rightPalm.Right, Camera.main.transform.forward));
            }

            if (palmForward)
            {
                Debug.Log("leftFor/CamFor: " + Vector3.Angle(leftPalm.Forward, Camera.main.transform.forward));
                Debug.Log("rightFor/CamFor: " + Vector3.Angle(rightPalm.Forward, Camera.main.transform.forward));
            }

            if (palmToPalm)
            {
                Debug.Log("palm to palm angle: " + Vector3.Angle(leftPalm.Up, rightPalm.Up));
            }
        }
    }
}
