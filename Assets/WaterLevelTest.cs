using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelTest : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float waterLevel = 0f;

    LiquidVolumeAnimator liquidAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        liquidAnimator = GetComponentInChildren<LiquidVolumeAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        liquidAnimator.level = waterLevel;
    }
}
