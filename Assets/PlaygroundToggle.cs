using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundToggle : MonoBehaviour
{
    [SerializeField] GameObject visuals;

    
    
    // Start is called before the first frame update
    void Start()
    {
        visuals.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePlayground()
    {
        if (!visuals.activeInHierarchy)
        {
            visuals.SetActive(true);
        }
        else
        {
            visuals.SetActive(false);
        }
    }
}
