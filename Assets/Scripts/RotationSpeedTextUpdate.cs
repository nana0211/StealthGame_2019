using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationSpeedTextUpdate : MonoBehaviour
{

    public Slider slider;
    private Text rotationspeedtext;

    // Start is called before the first frame update
    void Start()
    {
        rotationspeedtext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        rotationspeedtext.text = "Rotation Speed: " + slider.value;
    }
}
