using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    const float trackVelocity = 7.5f;
    const float trackAccelRate = 0.5f;

    public Camera camera;
    public GameObject playerAura;
    public Rigidbody2D rigidBody;


    Vector2 targetVelocity;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update() {
        var viewpointRadius = Mathf.Min(camera.pixelHeight, camera.pixelWidth) * 0.25f;
        Vector2 playerScreenPos = camera.WorldToScreenPoint(playerAura.transform.position);

        var center = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);

        if ((playerScreenPos - center).magnitude > viewpointRadius) {
            targetVelocity = (Vector3)((playerScreenPos - center).normalized * trackVelocity);
        } else {
            targetVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate() {

        var velocityDelta = targetVelocity - rigidBody.velocity;
        rigidBody.velocity = rigidBody.velocity + velocityDelta.normalized * Mathf.Min(trackAccelRate, velocityDelta.magnitude);


    }
}
