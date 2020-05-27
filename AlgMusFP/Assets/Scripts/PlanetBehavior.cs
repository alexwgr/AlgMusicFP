using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Resources;

public class PlanetBehavior : MonoBehaviour
{

    public PlanetResources Resource;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, -5f));       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
