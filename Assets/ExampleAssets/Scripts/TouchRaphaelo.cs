using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchRaphaelo : MonoBehaviour
{
    // Start is called before the first frame update
    static bool state = false;
    public GameObject ArPrehabs;


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch happened");
            if (state == false)
            {
                Debug.Log("Raphelo touched Touch activated");
                ArPrehabs.SetActive(true);
            }
            ArPrehabs.SetActive(false);
        }
    }
}
