using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lerpSpeed = 10f;
    private Vector3 targetPosition;
    private bool moving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click sets the target
        {

        }
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z; // Keep the camera's Z position
        moving = true;

        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

            // Stop moving when close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                moving = false;
            }
        }
    }
}
