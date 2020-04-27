using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    MixedRealityPose indexTip1, indexTip2, palm1, palm2;

    [SerializeField] float palmInThresh = 0.3f;
    [SerializeField] float palmSelectorSize = 0.4f;

    [SerializeField] bool logIndex = false;
    [SerializeField] bool logPalm = false;

    [SerializeField] GameObject particles;
    [SerializeField] GameObject orbs;
    [SerializeField] GameObject streams;

    float palmDist;
    
    // Start is called before the first frame update
    void Start()
    {
        DisableShapes();
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
                Debug.Log("palm.right: " + palm1.Right);
                Debug.Log("palm.up: " + palm1.Up);

            }
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out palm1) && HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out palm2))
        {
            if (palm1.Right.x <= palmInThresh && palm1.Right.x >= 0 && palm2.Right.x <= palmInThresh && palm2.Right.x >= 0)
            {
                palmDist = Mathf.Abs(Vector3.Distance(palm1.Position, palm2.Position));
                ActivatePalmForms();
            }
            else
            {
                DisableShapes();
            }
        }
        else DisableShapes();
    }

    private void ActivatePalmForms()
    {
        float slotSize = palmSelectorSize / 3;

        if (palmDist > 0 && palmDist <= palmSelectorSize)
        {
            if (palmDist > 0 && palmDist <= palmSelectorSize - slotSize * 2)
            {
                particles.SetActive(true);
                orbs.SetActive(false);
                streams.SetActive(false);

                particles.transform.position = GetShapePos();
                particles.transform.localScale = new Vector3(palmDist, palmDist, palmDist);
            }
            else if (palmDist > palmSelectorSize - slotSize * 2 && palmDist <= palmSelectorSize - slotSize)
            {
                particles.SetActive(false);
                orbs.SetActive(true);
                streams.SetActive(false);

                orbs.transform.position = GetShapePos();
                orbs.transform.localScale = new Vector3(palmDist, palmDist, palmDist);
            }
            else
            {
                particles.SetActive(false);
                orbs.SetActive(false);
                streams.SetActive(true);

                streams.transform.position = GetShapePos();
                streams.transform.localScale = new Vector3(palmDist, palmDist, palmDist);
            }
        }
        else
        {
            DisableShapes();
        }
    }

    private Vector3 GetShapePos()
    {
        var rightPalmPos = palm1.Position;
        var leftPalmPos = palm2.Position;

        var midpoint = Vector3.Lerp(rightPalmPos, leftPalmPos, 0.5f);

        return midpoint;
    }

    private void DisableShapes()
    {
        particles.SetActive(false);
        orbs.SetActive(false);
        streams.SetActive(false);
    }
}
