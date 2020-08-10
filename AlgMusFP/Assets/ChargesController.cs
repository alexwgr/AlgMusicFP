using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Resources;

public class ChargesController : MonoBehaviour
{
    public BackgroundManager bckgdMgr;

    public List<ChargeController> CannonCharges = new List<ChargeController>();
    Dictionary<PlanetResources, ChargeController> indexedCharges = new Dictionary<PlanetResources, ChargeController>();

    bool charging = false;
    PlanetResources chargeResource;

    public bool ChargingBlue { get { return charging && chargeResource == PlanetResources.blue; } }
    public bool ChargingYellow { get { return charging && chargeResource == PlanetResources.yellow; } }
    public bool CharingRed { get { return charging && chargeResource == PlanetResources.red; } }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Aura")) {
            chargeResource = collision.gameObject.transform.parent.GetComponent<PlanetBehavior>().Resource;
            charging = true;

            if (chargeResource == PlanetResources.blue) {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/blueAura", 1);
                bckgdMgr.SetBlueBackground();

            } else if (chargeResource == PlanetResources.yellow) {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/yellowAura", 1);
                bckgdMgr.SetYellowBackground();
            }
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
        foreach (var cc in CannonCharges) {
            indexedCharges[cc.Resource] = cc;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (charging) {
            indexedCharges[chargeResource].Replenish();
        }
    }
}
