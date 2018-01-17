using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Rigidbody2D playerRigidbody2D;
    public float distance;
    public float smoothTime;

    private Vector2 currentVelocity;

    void FixedUpdate()
    {
        var direction = playerRigidbody2D.velocity.normalized;
        var targetPosition = playerRigidbody2D.position + direction * distance;

        transform.position = new Vector3(
            Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref currentVelocity.y, smoothTime),
            transform.position.z);
    }
}
