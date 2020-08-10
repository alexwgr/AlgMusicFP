using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public SpriteRenderer Red, Yellow, Blue;

    bool redActive, yellowActive, blueActive;
    float redOpacity = 0, yellowOpacity = 0, blueOpacity = 0;

    public void SetRedBackground() {
        redActive = true;
        yellowActive = blueActive = false;
    }

    public void SetYellowBackground() {
        yellowActive = true;
        redActive = blueActive = false;
    }

    public void SetBlueBackground() {
        blueActive = true;
        redActive = yellowActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        redOpacity = (redActive ? Mathf.Min(1, redOpacity + Time.deltaTime) : Mathf.Max(0, redOpacity - Time.deltaTime));
        yellowOpacity = (yellowActive ? Mathf.Min(1, yellowOpacity + Time.deltaTime) : Mathf.Max(0, yellowOpacity - Time.deltaTime));
        blueOpacity = (blueActive ? Mathf.Min(1, blueOpacity + Time.deltaTime) : Mathf.Max(0, blueOpacity - Time.deltaTime));

        Red.color = redOpacity * Color.white;
        Yellow.color = yellowOpacity * Color.white;
        Blue.color = blueOpacity * Color.white;
    }
}
