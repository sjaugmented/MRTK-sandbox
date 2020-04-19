using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDimmerScaler : MonoBehaviour
{
    [Range(-0.1f, 0.1f)]
    [SerializeField] float xPosOffset = 0;
    [Range(-0.1f, 0.1f)]
    [SerializeField] float yPosOffset = 0;
    [Range(-0.1f, 0.1f)]
    [SerializeField] float zPosOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScaleToFinger();
    }

    private void ScaleToFinger()
    {
        MixedRealityPose indexTip, indexKnuckle;

        // get fingertip and knuckle positions
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out indexTip) && HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Left, out indexKnuckle))
        {
            Vector3 rawMidpoint = Vector3.Lerp(indexTip.Position, indexKnuckle.Position, 0.5f);
            Vector3 adjustedMidpoint = new Vector3(xPosOffset, yPosOffset, zPosOffset);
            float distance = Vector3.Distance(indexTip.Position, indexKnuckle.Position);
            var rotation = Quaternion.FromToRotation(Vector3.up, indexKnuckle.Position - indexTip.Position);

            // set dimmer position to midpoint
            transform.position = rawMidpoint;
            transform.localPosition = adjustedMidpoint;
            // set dimmer size to pose distance
            transform.localScale = new Vector3(0.01f, distance, 1f);
            // set dimmer rotation
            transform.rotation = rotation;
            
        }
        else return;

        

        

        

        // set dimmer rotation
    }
}
