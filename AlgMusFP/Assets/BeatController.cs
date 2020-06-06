using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatController : MonoBehaviour
{

    public Image square;
    public Image glow;


    Color baseColor;
    // Start is called before the first frame update

    float state = 0;

    float flashTimer;

    public void SetState(float input) {
        state = input;
        
    }

    public void SetBaseColor(Color color) {
        baseColor = color;
    }

    public float GetState() {
        return state;
    }

    public void Flash() {
        flashTimer = 1f;
    }

    void Start()
    {
        SetState(0);
        glow.color = Color.clear;
        
    }

    // Update is called once per frame
    void Update()
    {
        square.color = baseColor * Mathf.Clamp01(state);
        glow.color = Color.white * 0.75f * flashTimer;
        flashTimer = Mathf.Max(0, flashTimer - Time.deltaTime);
    }
}
