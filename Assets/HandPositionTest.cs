using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositionTest : MonoBehaviour, IMixedRealityHandJointService
{
    public string Name => throw new System.NotImplementedException();

    public uint Priority => throw new System.NotImplementedException();

    public BaseMixedRealityProfile ConfigurationProfile => throw new System.NotImplementedException();

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }

    public void Disable()
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void Enable()
    {
        throw new System.NotImplementedException();
    }

    public IMixedRealityController[] GetActiveControllers()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public bool IsHandTracked(Handedness handedness)
    {
        throw new System.NotImplementedException();
    }

    public void LateUpdate()
    {
        throw new System.NotImplementedException();
    }

    public Transform RequestJointTransform(TrackedHandJoint joint, Handedness handedness)
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform jointTransform = RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
        Debug.Log(jointTransform);

    }

    void IMixedRealityService.Update()
    {
        throw new System.NotImplementedException();
    }
}
