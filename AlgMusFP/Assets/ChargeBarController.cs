using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarController : MonoBehaviour
{

    public ChargesController chargesController;

    public GameObject BeatBarParent;
    List<BeatController> BeatBar;

    public GameObject BlueBarParent;
    List<BeatController> BlueBar;

    public GameObject YellowBarParent;
    List<BeatController> YellowBar;


    public ChargeController RedCharge;
    public ChargeController YellowCharge;
    public ChargeController BlueCharge;

    int bluePrevBeat = 0, yellowPrevBeat = 0;

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler.Instance.Init();
        Application.runInBackground = true;

        BlueBar = new List<BeatController>();
        BeatBar = new List<BeatController>();
        YellowBar = new List<BeatController>();
        BeatBarParent.GetComponentsInChildren(BeatBar);
        BlueBarParent.GetComponentsInChildren(BlueBar);
        YellowBarParent.GetComponentsInChildren(YellowBar);

        for (int i = 0; i < 32; i++) {
            BlueBar[i].SetBaseColor(Color.blue);
            YellowBar[i].SetBaseColor(Color.yellow);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire")) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/fireState", 1);
        } else if (Input.GetButtonUp("Fire")) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/fireState", 0);
        }


        OSCHandler.Instance.UpdateLogs();
        Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
        servers = OSCHandler.Instance.Servers;

        foreach (KeyValuePair<string, ServerLog> item in servers) {
            // If we have received at least one packet,
            // show the last received from the log in the Debug console
            if (item.Value.log.Count > 0) {
                int lastPacketIndex = item.Value.packets.Count - 1;
                var addr = item.Value.packets[lastPacketIndex].Address.ToString();

                if (addr == "/metroSignal") {
                    var lastPacket = item.Value.packets[lastPacketIndex];
                    var beatCount = Convert.ToInt32(lastPacket.Data[0].ToString());

                    var beat = beatCount - 1;
                    BeatBar[beat].Flash();


                    var blueState = Convert.ToInt32(lastPacket.Data[1].ToString());
                    var yellowState = Convert.ToInt32(lastPacket.Data[2].ToString());

                    if (chargesController.ChargingBlue && blueState != 0) {
                        BlueBar[beat].SetState(1);                       
                    }

                    if (chargesController.ChargingYellow && yellowState != 0) {
                        YellowBar[beat].SetState(1);
                    }

                    if (BlueBar[beat].GetState() > 0) {
                        if (beat != bluePrevBeat) {
                            OSCHandler.Instance.SendMessageToClient("pd", "/unity/blueFireSignal", beat);
                            bluePrevBeat = beat;


                            if (Input.GetButton("Fire")) {

                                var currState = BlueBar[beat].GetState();
                                if (currState > 0) {                                    
                                    BlueCharge.Fire();
                                    BlueBar[beat].SetState(currState - 1f);
                                    BlueBar[beat].Flash();
                                }
                            }

                        }                        
                    }

                    if (YellowBar[beat].GetState() > 0) {
                        if (beat != yellowPrevBeat) {
                            OSCHandler.Instance.SendMessageToClient("pd", "/unity/yellowFireSignal", beat);
                            yellowPrevBeat = beat;

                        }

                        if (Input.GetButton("Fire")) {

                            var currState = YellowBar[beat].GetState();
                            if (currState > 0) {
                                YellowCharge.Fire();
                                YellowBar[beat].SetState(currState - 1f);
                                YellowBar[beat].Flash();
                            }
                        }
                    }
                    


                    
                }



            }
        }

    }
}
