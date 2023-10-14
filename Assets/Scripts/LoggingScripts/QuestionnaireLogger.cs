using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireLogger : LoggerGeneric
{
    [HideInInspector]
    public SliderEmotionUI sliderQ;

    // Start is called before the first frame update
    void Start()
    {
        sliderQ = GetComponent<SliderEmotionUI>();
    }

    public override LogInfo GetCurrentObjectInfo()
    {
        string name = gameObject.name;
        string tag = gameObject.tag;
        QuestionnaireInfo info = new QuestionnaireInfo(name, tag, sliderQ);
        return info;
    }


    class QuestionnaireInfo : LogInfo
    {
        public string name;
        public string tag;


        public QuestionItem[] Items;

        public QuestionnaireInfo(string name, string tag, SliderEmotionUI sliderQ)
        {
            List<QuestionItem> items = new List<QuestionItem>();

            for(int i = 0; i < sliderQ.EmotionOrder.Length; i++)
            {
                float value = sliderQ.EmotionOrder[i].GetComponentInChildren<Slider>().value;
                items.Add(new QuestionItem(sliderQ.EmotionOrder[i].name, value, i == sliderQ.CurrentEmotionSelection));
            }

            this.name = name;
            this.tag = tag;
            Items = items.ToArray();
        }
    }


    class QuestionItem
    {
        public string Name;
        public float SliderValue;
        public bool IsChosen;

        public QuestionItem(string name, float value, bool IsChosen)
        {
            this.Name = name;
            this.SliderValue = value;
            this.IsChosen = IsChosen;
        }

    }
}
