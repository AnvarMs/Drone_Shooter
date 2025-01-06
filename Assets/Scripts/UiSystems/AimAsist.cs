using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAssist : MonoBehaviour
{
    public Image crossAire;              // Crosshair UI image
    public float aimDistance = 100f;     // Maximum distance of sphere cast
    public float baseAimRadius = 1f;     // Starting radius of the sphere cast
    public float fovExpansionRate = 0.1f; // Rate at which radius expands with distance
    public Camera aimDirection;          // Camera used for aiming

    void Update()
    {
        // Calculate dynamic aim radius based on distance to simulate FOV effect
        float dynamicRadius = baseAimRadius + aimDistance * fovExpansionRate;

        // Perform the sphere cast with the dynamic radius
        if (Physics.SphereCast(transform.position, baseAimRadius, aimDirection.transform.forward, out RaycastHit hit, aimDistance))
        {
            // Hit logic here
            if (hit.transform.GetComponent<DroneController>())
            {
                crossAire.enabled = true;
                AimCrossaire(hit.transform.gameObject);
            }
            else
            {
                // Optionally, hide the crosshair if the enemy is out of view
                crossAire.enabled = false;
            }
        }
    }
    void AimCrossaire(GameObject enemy)
    {


        if (enemy != null && crossAire != null && aimDirection != null)
        {
            // Convert the enemy's world position to a screen position
            Vector3 screenPosition = aimDirection.WorldToScreenPoint(enemy.transform.position);

            // Check if the enemy is within the camera's view
                screenPosition.z=0f;
            // Set the crosshair position to the enemy's screen position

            crossAire.transform.position = screenPosition;


        }


    }

    // void OnDrawGizmos()
    // {
    //     if (aimDirection != null)
    //     {
    //         // Calculate dynamic radius for visualization
    //         float dynamicRadius = baseAimRadius + aimDistance * fovExpansionRate;

    //         Gizmos.color = Color.red;
    //         // Draw the start of the sphere cast (small radius)
    //         Gizmos.DrawWireSphere(transform.position, baseAimRadius);
    //         // Draw the end of the sphere cast (expanded radius)
    //         Gizmos.DrawWireSphere(transform.position + aimDirection.transform.forward * aimDistance, dynamicRadius);
    //         // Draw a cone-like line connecting the start and end points of the sphere cast
    //         Gizmos.DrawLine(transform.position, transform.position + aimDirection.transform.forward * aimDistance);
    //     }
    // }
}
