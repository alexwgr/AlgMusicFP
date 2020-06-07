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
    public SpriteRenderer flameButt;

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

            OSCHandler.Instance.SendMessageToClient("pd", "/unity/aurastate", 1);


        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            auraTarget = null;
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/aurastate", 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 1);

        directionAngle = 0;


    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 0);
        }
        if (Input.GetButtonDown("Jump")) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/rocket", 1);
        } else if (Input.GetButtonUp("Jump")) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/norocket", 1);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        flameButt.color = Color.clear;
        var direction = new Vector2(Mathf.Cos(directionAngle * Mathf.Deg2Rad), Mathf.Sin(directionAngle * Mathf.Deg2Rad));
        var boostRequest = Input.GetButton("Jump");
        if (boostRequest && !boostLock) {
            rigidBody.AddForce(direction * BoostForce);
            flameButt.color = Color.white;
        } else if (!boostRequest && boostLock) {
            boostLock = false;
        }

        

        var inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        if (auraTarget != null && (boostLock || !boostRequest)) {            
            var displacement = (Vector2)(auraTarget.position - transform.position);
            var displacementNorm = displacement.normalized;
            var problemComponent = Vector2.Dot(rigidBody.velocity, displacementNorm);
            var targetVelocity = (rigidBody.velocity - problemComponent * displacementNorm).normalized * MaxVelocity * 0.75f;

            var idealRadius = auraTarget.GetComponent<CircleCollider2D>().radius * 0.75f;
            var downComponent = displacementNorm * Mathf.Max(0, displacement.magnitude - idealRadius) / idealRadius;

            targetVelocity += downComponent;

            //var forceDirection = rigidBody.velocity -  * displacement;
            rigidBody.velocity = auraTimer * auraEntryVelocity + (1 - auraTimer) * targetVelocity;
            auraTimer = Mathf.Max(0, auraTimer - 0.2f);

            directionAngle = Vector2.SignedAngle(Vector2.right, targetVelocity);

            auraPrevDirection = rigidBody.velocity.normalized;
            //pushToDirection(targetVelocity);
            //rigidBody.AddForce(-0.5f * problemComponent * displacement);
            
        } else {
            if (!boostRequest || boostLock) {
                rigidBody.velocity = rigidBody.velocity.normalized * (Mathf.Max(0, rigidBody.velocity.magnitude - Deceleration));
            }

            var targetAngle = Vector2.SignedAngle(Vector2.right, inputVector);
            if (targetAngle != directionAngle) {
                var push = Mathf.Sign(targetAngle - directionAngle);
                directionAngle += Mathf.Min(10, Mathf.Abs(targetAngle - directionAngle)) * push;
            }

            var rotation = Quaternion.Slerp(transform.localRotation, Quaternion.FromToRotation(transform.localRotation * Vector3.right,
                new Vector3(inputVector.x, inputVector.y, 0)), 0.5f);

            direction = new Vector2(Mathf.Cos(rigidBody.rotation * Mathf.Deg2Rad), Mathf.Sin(rigidBody.rotation * Mathf.Deg2Rad));
            var targetDisplace = (inputVector - direction);
            direction = direction + targetDisplace.normalized * Mathf.Min(targetDisplace.magnitude, 0.2f);
            directionAngle = Vector2.SignedAngle(Vector2.right, direction);

        }





        rigidBody.SetRotation(directionAngle);

        //pushVertical(verticalInput * (boostRequest ? 2.5f : 6f));






        if (rigidBody.velocity.magnitude > MaxVelocity) {
            rigidBody.velocity = rigidBody.velocity.normalized * MaxVelocity;
        }




    }
}
