using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePositionTester : MonoBehaviour
{
    [SerializeField] Transform indexTip;
    [SerializeField] Transform indexKnuckle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlaceAtMidpoint();
        ResizeTransform();
        RotateTransform();
    }

    private void PlaceAtMidpoint()
    {
        Vector3 midpoint = Vector3.Lerp(indexTip.position, indexKnuckle.position, 0.5f);
        transform.position = midpoint;
    }

    private void ResizeTransform()
    {
        float distance = Vector3.Distance(indexTip.position, indexKnuckle.position);
        transform.localScale = new Vector3(0.05f, distance, 0.05f);
    }

    private void RotateTransform()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, indexKnuckle.position - indexTip.position);
    }
}
