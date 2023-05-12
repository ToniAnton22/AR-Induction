using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ReadNumber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject Outline;
    [SerializeField]
    private ARTrackedImageManager _trackImagesManager;

    private Dictionary<string, GameObject> _instantiatedNumber = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _trackImagesManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        _trackImagesManager.trackedImagesChanged += OnTrackedImagedChanges;
    }

    private void OnDisable()
    {
        _trackImagesManager.trackedImagesChanged -= OnTrackedImagedChanges;
    }

    private void OnTrackedImagedChanges(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Loop through all new tracked images that have been detected
        foreach (var trackedImage in eventArgs.added)
        {
            // Get the name of the reference image
            var imageName = trackedImage.referenceImage.name;
            // Now loop over the array of prefabs
            Outline.SetActive(true);
            Debug.Log("These are the numbers spotted" + imageName.ToString());


        }
        // For all prefabs that have been created so far, set them active or not depending on it's presence
        foreach (var trackedImage in eventArgs.updated)
        {
            _instantiatedNumber[trackedImage.referenceImage.name]
                .SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        // If the AR subsystem gives up on looking for the image

        foreach (var trackedImage in eventArgs.removed)
        {
            // Destroy the prefab
            Destroy(_instantiatedNumber[trackedImage.referenceImage.name]);
            Outline.SetActive(false);
            // Also remove the instance in the array
            _instantiatedNumber.Remove(trackedImage.referenceImage.name);
            // You can also set the prefab to inactive instead.
            // _instantiatedPrefabs[trackedImage.referenceImage.name].setActive(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
