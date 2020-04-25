using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    [SerializeField] bool logIndex = false;
    [SerializeField] bool logPalm = false;

    [SerializeField] GameObject testObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out indexTip1))
        {
            if (logIndex)
            {
                Debug.Log("indextip.forward: " + indexTip1.Forward);
                Debug.Log("indextip.up: " + indexTip1.Up);
            }
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Any, out palm1))
        {
            if (logPalm)
            {
                Debug.Log("palm.forward: " + palm1.Forward);
                Debug.Log("palm.up: " + palm1.Up);

            }

            /*if (palm1.Up.y < 0)
            {
                testObject.SetActive(true);
            }
            else testObject.SetActive(false);*/
        }
    }
}
