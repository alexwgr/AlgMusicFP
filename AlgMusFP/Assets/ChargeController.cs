using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Resources;

public class ChargeController : MonoBehaviour
{

    public Driver driver;

    public float ChargeTime;
    public int DepleteShots;

    float chargeStrength = 0;   
    
    public float ChargeStrength { get { return chargeStrength; } }
    public GameObject ShotPrefab;
    public PlanetResources Resource;
    public SpriteRenderer sprite;


    Vector2 fireAxis;

    float resetTimer = 0;


    public void Replenish() {
        chargeStrength = Mathf.Min(1, chargeStrength + 1 / (ChargeTime / Time.fixedDeltaTime));
    }

    public void Fire() {
        var angle = driver.GetComponent<Rigidbody2D>().rotation;
        if (chargeStrength > 0) {
            chargeStrength = Mathf.Max(0, chargeStrength - 1f / DepleteShots);

           
        }
        var shot = Instantiate(ShotPrefab, transform.position, driver.transform.rotation);
        shot.transform.localPosition += new Vector3(0, Random.Range(-0.35f, 0.35f));

    }


    // Start is called before the first frame update
    void Start()
    {
        chargeStrength = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, chargeStrength);


        fireAxis = new Vector2(Input.GetAxis("FireX"), Input.GetAxis("FireY"));
       

        


    }
}
