using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmMenuTest : MonoBehaviour
{
    [SerializeField] GameObject[] menuItems;

    bool menuOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        menuOpen = FindObjectOfType<HandTrackerTest>().spellbookIsOpen;
        DeactivateMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMenu()
    {
        menuOpen = true;

        foreach (GameObject item in menuItems)
        {
            item.SetActive(true);
        }
    }

    public void DeactivateMenu()
    {
        menuOpen = false;

        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }
    }
}
