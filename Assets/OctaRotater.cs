using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaRotater : MonoBehaviour
{
    public bool isSelected = false;
    [Header("Standby values")]
    [SerializeField] float standbyXRot = .2f;
    [SerializeField] float standbyYRot = .2f;
    [SerializeField] float standbyZRot = .2f;
    [Header("Selected values")]
    [SerializeField] float selectedXRot = 1f;
    [SerializeField] float selectedYRot = 1f;
    [SerializeField] float selectedZRot = 1f;
    [SerializeField] float scaleMagnifier = 1.3f;

    float baseScale;

    void Start()
    {
        baseScale = transform.localScale.y;
    }

    private void StandbyBehaviour()
    {
        transform.Rotate(standbyXRot, standbyYRot, standbyZRot);
        transform.localScale = new Vector3(baseScale, baseScale, baseScale);
    }

    public void SelectedBehaviour()
    {
        transform.Rotate(selectedXRot, selectedYRot, selectedZRot);
        transform.localScale = new Vector3(baseScale * scaleMagnifier, baseScale * scaleMagnifier, baseScale * scaleMagnifier);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) SelectedBehaviour();
        else StandbyBehaviour();
    }

}
