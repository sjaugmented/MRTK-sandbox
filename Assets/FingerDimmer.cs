using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDimmer : MonoBehaviour
{
    [SerializeField] private HandInteractionPanZoom panInputSource;
    [SerializeField] GameObject lightObj;

    Light workLight;

    private void OnEnable()
    {
        if (panInputSource == null)
        {
            panInputSource = GetComponent<HandInteractionPanZoom>();
        }
        if (panInputSource == null)
        {
            Debug.LogError("MoveWithPan did not find a HandInteractionPanZoom to listen to, the component will not work", gameObject);
        }
        else
        {
            panInputSource.PanUpdated.AddListener(OnPanning);
        }
    }

    private void OnDisable()
    {
        if (panInputSource != null)
        {
            panInputSource.PanUpdated.RemoveListener(OnPanning);
        }
    }

    public void OnPanning(HandPanEventData eventData)
    {
        Vector3 panningPosition = new Vector3(eventData.PanDelta.x, eventData.PanDelta.y * -1, 0.0f);
        Debug.Log(panningPosition); //todo remove
    }

    // Start is called before the first frame update
    void Start()
    {
        workLight = lightObj.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
