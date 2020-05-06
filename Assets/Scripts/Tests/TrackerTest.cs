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

    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    void Update()
    {
         if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out palm1) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out palm2))
        {
            if (palmUp)
            {
                Debug.Log("leftUp/CamFor: " + Vector3.Angle(palm1.Up, Camera.main.transform.forward));
                Debug.Log("rightUp/CamFor: " + Vector3.Angle(palm2.Up, Camera.main.transform.forward));
            }

            if (palmRight)
            {
                Debug.Log("leftRight/CamFor: " + Vector3.Angle(palm1.Right, Camera.main.transform.forward));
                Debug.Log("rightRight/CamFor: " + Vector3.Angle(palm2.Right, Camera.main.transform.forward));
            }

            if (palmForward)
            {
                Debug.Log("leftFor/CamFor: " + Vector3.Angle(palm1.Forward, Camera.main.transform.forward));
                Debug.Log("rightFor/CamFor: " + Vector3.Angle(palm2.Forward, Camera.main.transform.forward));
            }

            if (palmToPalm)
            {
                Debug.Log("palm to palm angle: " + Vector3.Angle(palm1.Up, palm2.Up));
            }
        }
    }
}
