using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTracker : MonoBehaviour
{
    [Tooltip("Min Velocity at which spells are cast")] [SerializeField] float minVelocity = 2f;
    [Tooltip("Max Velocity at which spells are cast")] [SerializeField] float maxVelocity = 5f;

    // used to index tracking & velocity
    MixedRealityPose index; 
    float fingerUp = 0.3f;
    float prevHandCamDist;
    float awayVelocity;

    // used to disable index tracking when menu is open
    bool menuOpen = false;

    CastTesting caster;

    // Start is called before the first frame update
    void Start()
    {
        caster = FindObjectOfType<CastTesting>();
    }

    // Update is called once per frame
    void Update()
    {
        // only tracks index if menu is closed
        if (!menuOpen)
        {
            TrackIndexFinger();
        }
        else return;
    }

    private void TrackIndexFinger()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index))
        {
            if (index.Up.y >= fingerUp)
            {
                ProcessIndexVelocity();
            }
            else return;
        }
    }

    private void ProcessIndexVelocity()
    {
        Vector3 cameraPos = Camera.main.transform.position;

        float handCamDist = Vector3.Distance(cameraPos, index.Position);
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Vector3.Distance(cameraPos, index.Position);

        if (awayVelocity >= minVelocity && awayVelocity <= maxVelocity)
        {
            Debug.Log("casting");
            caster.CastTestSpell();
        }
    }
}
