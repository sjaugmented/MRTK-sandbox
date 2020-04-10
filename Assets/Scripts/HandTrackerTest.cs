﻿using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [Tooltip("Velocity at which spells are cast")] [SerializeField] float castThreshold = 0.5f;
    [Tooltip("Interval between spells cast")] [SerializeField] float castFreq = 0.5f;
    [Header("Casting and Spell prefabs")]
    [Tooltip("Visual representation of Light spell")] [SerializeField] GameObject lightCaster;
    [Tooltip("Visual representation of Fire spell")] [SerializeField] GameObject fireCaster;
    [Tooltip("Visual representation of Water spell")] [SerializeField] GameObject waterCaster;
    [Tooltip("Visual representation of Wind spell")] [SerializeField] GameObject windCaster;
    [Tooltip("Visual representation of Earth spell")] [SerializeField] GameObject earthCaster;
    [Tooltip("Light spell prefab to cast")] [SerializeField] GameObject lightSpell;
    [Tooltip("Fire spell prefab to cast")] [SerializeField] GameObject fireSpell;
    [Tooltip("Water spell prefab to cast")] [SerializeField] GameObject waterSpell;
    [Tooltip("Wind spell prefab to cast")] [SerializeField] GameObject windSpell;
    [Tooltip("Earth spell prefab to cast")] [SerializeField] GameObject earthSpell;

    bool castIsActive = false;

    float prevHandCamDist;
    float awayVelocity;

    // Start is called before the first frame update
    void Start()
    {
        lightCaster.SetActive(false);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ListenForFingers();

        MixedRealityPose pose;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Any, out pose))
        {

        }
    }

    private void ListenForFingers()
    {
        MixedRealityPose index, middle, ring, pinky, thumb;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index))
        {
            SetCasters(lightCaster);
            TrackHandVelocity(index, lightSpell);
        }
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Any, out middle))
        {
            SetCasters(fireCaster);
            TrackHandVelocity(index, fireSpell);
        }
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Any, out middle) && HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Any, out ring))
        {
            SetCasters(waterCaster);
            TrackHandVelocity(index, waterSpell);
        }
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Any, out middle) && HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Any, out ring) && HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Any, out pinky))
        {
            SetCasters(windCaster);
            TrackHandVelocity(index, windSpell);
        }
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Any, out middle) && HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Any, out ring) && HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Any, out pinky) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Any, out thumb))
        {
            SetCasters(earthCaster);
            TrackHandVelocity(index, earthSpell);
        }

        /*else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Both, out rightPose))
        {
            Debug.Log("tracking two index fingers");
            castIsActive = false;
            lightCaster.SetActive(false);

        }*/
        else
        {
            castIsActive = false;
            SetCasters(null);

        }
    }

    private void SetCasters(GameObject trueCaster)
    {
        lightCaster.SetActive(true);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);

        if (trueCaster == null) return;
        else trueCaster.SetActive(true);
    }

    private void TrackHandVelocity(MixedRealityPose pose, GameObject spellToCast)
    {
        // tracks velocity of joint away from camera; if greater than castThreshold then cast spell

        Vector3 cameraPos = Camera.main.transform.position;

        float handCamDist = Vector3.Distance(cameraPos, pose.Position);
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Vector3.Distance(cameraPos, pose.Position);

        if (awayVelocity >= castThreshold && !castIsActive)
        {
            CastSpell(spellToCast ,pose.Position, Camera.main.transform.rotation, awayVelocity);

        }
    }

    private void CastSpell(GameObject spellToCast, Vector3 pos, Quaternion rot, float forwardVel)
    {
        //spellParticle.Play();

        GameObject holo = Instantiate(spellToCast, pos, rot);
        StartCoroutine("CastDelay");
    }

    IEnumerator CastDelay()
    {
        castIsActive = true;
        yield return new WaitForSeconds(castFreq);
        castIsActive = false;
    }
}
