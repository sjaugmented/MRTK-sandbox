using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackerTest : MonoBehaviour
{
    [Tooltip("Min Velocity at which spells are cast")] [SerializeField] float castThreshold = 2f;
    [Tooltip("Max Velocity at which spells are cast")] [SerializeField] float castThresholdCap = 5f;
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
    [Header("Left Palm menu object")]
    [SerializeField] bool palmMenuForSpellbook = false;

    GameObject spellFromPalm;
    
    bool ableToCast = true;
    public bool spellbookIsOpen = false;

    float prevHandCamDist;
    float awayVelocity;

    MixedRealityPose index, middle, ring, pinky, thumb;
    float fingerUp = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        SetCasters(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (palmMenuForSpellbook)
        {
            ProcessPalmSpellbook();
        } else
        {
            ProcessFingerSpellbook();
        }
    }
    private void ProcessPalmSpellbook()
    {
        if (spellbookIsOpen) ableToCast = false;
        else
        {
            ableToCast = true;

            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index))
            {
                if (index.Up.y >= fingerUp)
                {
                    if (spellFromPalm = null) return;
                    else
                    {
                        TrackHandVelocity(index, index, spellFromPalm);
                    }
                }
                else return;
            }
            else return;
        }
    }

    private void ProcessFingerSpellbook()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out index) && HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Any, out middle) && HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Any, out ring) && HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Any, out pinky) && HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Any, out thumb))
        {
            if (index.Up.y > fingerUp && middle.Up.y > fingerUp && ring.Up.y > fingerUp && pinky.Up.y > fingerUp && thumb.Up.x < 0)
            {
                SetCasters(earthCaster);
                TrackHandVelocity(index, thumb, earthSpell);
            }
            else if (index.Up.y >= fingerUp && middle.Up.y >= fingerUp && ring.Up.y >= fingerUp && pinky.Up.y >= fingerUp)
            {
                SetCasters(windCaster);
                TrackHandVelocity(index, pinky, windSpell);
            }
            else if (index.Up.y >= fingerUp && middle.Up.y >= fingerUp && ring.Up.y >= fingerUp)
            {
                SetCasters(waterCaster);
                TrackHandVelocity(index, ring, waterSpell);
            }
            else if (index.Up.y >= fingerUp && middle.Up.y >= fingerUp)
            {
                SetCasters(fireCaster);
                TrackHandVelocity(index, middle, fireSpell);
            }
            else if (index.Up.y >= fingerUp)
            {
                SetCasters(lightCaster);
                TrackHandVelocity(index, index, lightSpell);
            }
            else
            {
                ableToCast = false;
                SetCasters(null);
            }
        }
        else
        {
            ableToCast = true;
            SetCasters(null);

        }

    }

    private void SetCasters(GameObject trueCaster)
    {
        lightCaster.SetActive(false);
        fireCaster.SetActive(false);
        waterCaster.SetActive(false);
        windCaster.SetActive(false);
        earthCaster.SetActive(false);

        if (trueCaster == null) return;
        else trueCaster.SetActive(true);
    }

    private void TrackHandVelocity(MixedRealityPose castFinger, MixedRealityPose velFinger, GameObject spellToCast)
    {
        // tracks velocity of joint away from camera; if greater than castThreshold then cast spell

        Vector3 cameraPos = Camera.main.transform.position;

        float handCamDist = Vector3.Distance(cameraPos, velFinger.Position);
        awayVelocity = (handCamDist - prevHandCamDist) / Time.deltaTime;
        prevHandCamDist = Vector3.Distance(cameraPos, velFinger.Position);

        //Debug.Log(awayVelocity); // todo remove

        if (awayVelocity >= castThreshold && awayVelocity <= castThresholdCap && ableToCast)
        {
            CastSpell(spellToCast, castFinger.Position, Camera.main.transform.rotation, awayVelocity);
            //Debug.Log("casting"); //todo remove
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
        ableToCast = false;
        yield return new WaitForSeconds(castFreq);
        ableToCast = true;
    }

    public void CastLight()
    {
        SetCasters(lightCaster);
        spellFromPalm = lightSpell;
    }
    public void CastFire()
    {
        SetCasters(fireCaster);
        spellFromPalm = fireSpell;
    }
    public void CastWater()
    {
        SetCasters(waterCaster);
        spellFromPalm = waterSpell;
    }
    public void CastWind()
    {
        SetCasters(windCaster);
        spellFromPalm = windSpell;
    }
    public void CastEarth()
    {
        SetCasters(earthCaster);
        spellFromPalm = earthSpell;
    }
}
