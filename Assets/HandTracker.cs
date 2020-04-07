using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class HandTracker : MonoBehaviour
{
    MixedRealityPose hands;
    public enum TrackedHandJoint
    {
        IndexDistalJoint,
        IndexTip
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        var hand = eventData.Controller as IMixedRealityHand;
        if (hand != null)
        {
            if (hand.TryGetJoint(TrackedHandJoint.IndexTip, out MixedRealityPose jointPose)
            {
                // ...
            }
        }
    }
}
