using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceVisibility : MonoBehaviour
{
    public Transform cameraTransform;
    [SerializeField] private GameObject[] gameObjects;
    public float visibilityDistance = 27f; // Adjust the distance at which the object becomes visible

    void Start()
    {
        // Set the initial visibility state based on distance
        SetVisibility();
    }

    void Update()
    {
        // Update visibility based on distance every frame
        SetVisibility();
    }

    private void SetVisibility()
    {
        if (cameraTransform != null)
        {
            float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);

            // If the distance is within the visibility range, make the object visible
            if (distanceToCamera <= visibilityDistance)
            {
                foreach (var go in gameObjects)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                foreach (var go in gameObjects)
                {
                    go.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("No target assigned for DistanceVisibility script on " + gameObject.name);
        }
    }
}
