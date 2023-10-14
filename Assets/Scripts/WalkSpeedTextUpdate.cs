using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkSpeedTextUpdate : MonoBehaviour
{

    public Slider slider;
    private Text walkspeedtext;

    // Start is called before the first frame update
    void Start()
    {
        walkspeedtext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        walkspeedtext.text = "Walk Speed: " + slider.value;
    }
}
