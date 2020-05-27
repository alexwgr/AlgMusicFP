using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Resources;

public class CannonBlast : MonoBehaviour
{
    public PlanetResources resource;
    public Rigidbody2D rigidBody;

    public float Speed;

    Vector2 direction;

    float lifeTimer = 3;

    
     

    // Start is called before the first frame update
    void Start() 
    {        
     


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidBody.velocity = transform.rotation * Vector2.right * Speed;
        lifeTimer -= Time.fixedDeltaTime;
        if (lifeTimer < 0) {
            Destroy(this);
        }
    }
}
