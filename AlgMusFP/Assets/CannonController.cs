using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Resources;


public class CannonInfo {
    public PlanetResources resource;
    public float remaining;
}


public class CannonController : MonoBehaviour
{

    public float ChargeTime;
    public int DepleteShots;

    public List<CannonBlast> CannonTypes;

    Dictionary<PlanetResources, float> CannonCharges = new Dictionary<PlanetResources, float>();

    bool charging = false;
    PlanetResources chargeResource;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            chargeResource = collision.gameObject.transform.parent.GetComponent<PlanetBehavior>().Resource;
            charging = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            charging = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {     
        foreach (var cb in CannonTypes) {
            CannonCharges[cb.resource] = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (charging) {
            CannonCharges[chargeResource] = Mathf.Min(1, CannonCharges[chargeResource] + ChargeTime / Time.fixedDeltaTime);
        }
    }


}
