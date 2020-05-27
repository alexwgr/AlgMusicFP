using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Driver : MonoBehaviour
{

    public float MaxVelocity, BoostForce, Deceleration;

    float directionAngle;
    public float DirectionAngle { get { return directionAngle; } }


    Transform auraTarget = null;
    Vector2 auraEntryVelocity, auraPrevDirection;
    float auraTimer = 0f;

    bool boostLock = false;
    public Rigidbody2D rigidBody;

    public void pushVertical(float verticalDirection) {        
        directionAngle = directionAngle + verticalDirection;
    }

    public void pushToDirection(Vector2 target) {
        var displacement = Vector2.Angle(Vector2.right, target) - directionAngle;
        directionAngle += Mathf.Sign(displacement) * Mathf.Min(50f, Mathf.Abs(displacement));

    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            auraTarget = collision.gameObject.transform;
            auraEntryVelocity = rigidBody.velocity;
            auraPrevDirection = auraEntryVelocity.normalized;
            boostLock = true;
            auraTimer = 1f;


            
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            auraTarget = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        directionAngle = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = new Vector2(Mathf.Cos(directionAngle * Mathf.Deg2Rad), Mathf.Sin(directionAngle * Mathf.Deg2Rad));
        var boostRequest = Input.GetButton("Jump");
        if (boostRequest && !boostLock) {
            rigidBody.AddForce(direction * BoostForce);
        } else if (!boostRequest && boostLock) {
            boostLock = false;
        }

        var inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        if (auraTarget != null) {

            if ((boostLock || !boostRequest)) {
                var displacement = (Vector2)(auraTarget.position - transform.position);
                var displacementNorm = displacement.normalized;
                var problemComponent = Vector2.Dot(rigidBody.velocity, displacementNorm);
                var targetVelocity = (rigidBody.velocity - problemComponent * displacementNorm).normalized * MaxVelocity * 0.75f;

                var idealRadius = auraTarget.GetComponent<CircleCollider2D>().radius * 0.75f;
                var downComponent = displacementNorm * Mathf.Max(0, displacement.magnitude - idealRadius) / idealRadius;

                targetVelocity += downComponent;

                //var forceDirection = rigidBody.velocity -  * displacement;
                rigidBody.velocity = auraTimer * auraEntryVelocity + (1 - auraTimer) * targetVelocity;
                auraTimer = Mathf.Max(0, auraTimer - 0.1f);

                directionAngle = Vector2.SignedAngle(Vector2.right, targetVelocity);

                auraPrevDirection = rigidBody.velocity.normalized;
                //pushToDirection(targetVelocity);
                //rigidBody.AddForce(-0.5f * problemComponent * displacement);
            }

        } else {
            if (!boostRequest || boostLock) {
                rigidBody.velocity = rigidBody.velocity.normalized * (Mathf.Max(0, rigidBody.velocity.magnitude - Deceleration));
            }
            directionAngle = Vector2.SignedAngle(Vector2.right, inputVector);


            //direction = new Vector2(Mathf.Cos(rigidBody.rotation * Mathf.Deg2Rad), Mathf.Sin(rigidBody.rotation * Mathf.Deg2Rad));
           
        }
        rigidBody.SetRotation(directionAngle);

        //pushVertical(verticalInput * (boostRequest ? 2.5f : 6f));






        if (rigidBody.velocity.magnitude > MaxVelocity) {
            rigidBody.velocity = rigidBody.velocity.normalized * MaxVelocity;
        }




    }
}
