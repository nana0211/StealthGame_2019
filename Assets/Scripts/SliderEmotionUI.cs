using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SliderEmotionUI : MonoBehaviour
{

    public string UserResponseLocation = "LogFolder/";
    public string UserResponseFileName = "Response";

    private Color SelectedColor = Color.yellow;
    private Color UnselectedColor = Color.white;

    [HideInInspector]
    public GameObject[] EmotionOrder;
    
    [HideInInspector]
    public List<bool> EmotionSelected;
    
    [HideInInspector]
    public int CurrentEmotionSelection;

    private bool FlagButtonPressRelease = true;
    public float CooldownButtonInterval = .2f;


    public int bufferLeftFlagMax = 5;
    private int bufferLeftFlagCounter = 0;

    public float SliderIncrementValue = 0.10f;

    private IEnumerator SliderCoroutine;

    private GameObject LevelManager;

    public int RandomSeed = 1234;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.Find("LevelManagerObject");

        GameObject[] EmotionObjects = GameObject.FindGameObjectsWithTag("SliderUIButton");
        EmotionOrder = new GameObject[10];
        EmotionSelected = new List<bool>();

        foreach (GameObject ob in EmotionObjects)
        {
            int index = EmotionLocation(ob.name);
            if(index >= 0)
            {
                EmotionOrder[index] = ob;
                EmotionSelected.Add(false);
            }
            else
            {
                Debug.LogError("Bug in the Slider Interface Emotion!");
            }
        }

        // Check to see if there is a Level Manager
        GameObject lvlManager = GameObject.FindGameObjectWithTag("LevelManager");

        if (lvlManager != null)
        {
            ShiftEmotionElements(lvlManager.GetComponent<LevelManager>().RandomSeed);
        }
        else
        {
            ShiftEmotionElements(RandomSeed);
        }

        CurrentEmotionSelection = 0;
        LightButton(EmotionOrder[CurrentEmotionSelection], "No");

    }

    // Update is called once per frame
    void Update()
    {
        CheckKeys();
    }

    private void CheckKeys()
    {
        if (FlagButtonPressRelease)
        {
            if (SliderCoroutine == null)
            {
                // Test the Condition For Horizontal
                if (CrossPlatformInputManager.GetButtonDown("RightOption"))
                {
                    SliderCoroutine = SliderUIRight();
                    StartCoroutine(SliderCoroutine);
                }
                else if (CrossPlatformInputManager.GetButtonDown("LeftOption"))
                {
                    SliderCoroutine = SliderUILeft();
                    StartCoroutine(SliderCoroutine);
                }
                else if (CrossPlatformInputManager.GetButtonDown("UpOption"))
                {
                    SliderCoroutine = SliderUIUp();
                    StartCoroutine(SliderCoroutine);
                }
                else if (CrossPlatformInputManager.GetButtonDown("DownOption"))
                {
                    SliderCoroutine = SliderUIDown();
                    StartCoroutine(SliderCoroutine);
                }
                else if (CrossPlatformInputManager.GetButtonDown("Submit"))
                {
                    //Debug.Log(SubmitEmotionData());
                    LogUserResponse();

                    // Change Scene Script
                    FinishScene();
                }
            }
        }
    }

    private int EmotionLocation(string name)
    {
        if(name == "Interest")
        {
            return 0;
        }
        else if(name == "Pleasure")
        {
            return 1;
        }
        else if(name == "Pride")
        {
            return 2;
        }
        else if(name == "Love")
        {
            return 3;
        }
        else if(name == "Relief")
        {
            return 4;
        }
        else if(name == "Regret")
        {
            return 5;
        }
        else if(name == "Anger")
        {
            return 6;
        }
        else if(name == "Disgust")
        {
            return 7;
        }
        else if(name == "Sadness")
        {
            return 8;
        }
        else if(name == "Disappointment")
        {
            return 9;
        }
        else
        {
            return -1;
        }
    }

    private void LightButton(GameObject ob, string val)
    {
        Image[] images = ob.GetComponentsInChildren<Image>();
        foreach(Image img in images)
        {
            if(img.name == val)
            {
                img.color = SelectedColor;
            }
        }
    }

    private void UnlightButton(GameObject ob, string val)
    {
        Image[] images = ob.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.name == val)
            {
                img.color = UnselectedColor;
            }
        }
    }

    private bool IsSliderZero(GameObject go)
    {
        Slider slider = go.GetComponentInChildren<Slider>();

        if(slider.value == 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void ManipulateSlider(GameObject go, float val)
    {
        Slider slider = go.GetComponentInChildren<Slider>();
        slider.value += val;
    }

    private float SliderValue(GameObject go)
    {
        Slider slider = go.GetComponentInChildren<Slider>();
        return slider.value;
    }

    private void DeselectEmotionColor()
    {
        if (EmotionSelected[CurrentEmotionSelection])
        {
            UnlightButton(EmotionOrder[CurrentEmotionSelection], "Yes");
        }
        else
        {
            UnlightButton(EmotionOrder[CurrentEmotionSelection], "No");
        }
    }

    private void SelectEmotionColor()
    {
        if (EmotionSelected[CurrentEmotionSelection])
        {
            // Make Sure the Options are Correctly Selected
            LightButton(EmotionOrder[CurrentEmotionSelection], "Yes");
            UnlightButton(EmotionOrder[CurrentEmotionSelection], "No");
        }
        else
        {
            // Make Sure the Options are Correctly Selected
            LightButton(EmotionOrder[CurrentEmotionSelection], "No");
            UnlightButton(EmotionOrder[CurrentEmotionSelection], "Yes");
        }
    }

    IEnumerator SliderUIRight()
    {
        FlagButtonPressRelease = false;
        while (CrossPlatformInputManager.GetButton("RightOption"))
        {
            if (EmotionSelected[CurrentEmotionSelection])
            {
                // Manipulate the Slider
                ManipulateSlider(EmotionOrder[CurrentEmotionSelection], SliderIncrementValue);
            }
            else
            {
                // Pass the Button from No to Yes
                EmotionSelected[CurrentEmotionSelection] = true;
                LightButton(EmotionOrder[CurrentEmotionSelection], "Yes");
                UnlightButton(EmotionOrder[CurrentEmotionSelection], "No");

                // Randomize Slider Value
                ManipulateSlider(EmotionOrder[CurrentEmotionSelection], UnityEngine.Random.Range(0f,1f));
            }
            yield return new WaitForSeconds(CooldownButtonInterval);
        }
        FlagButtonPressRelease = true;
        SliderCoroutine = null;
        yield return null;
    }

    IEnumerator SliderUILeft()
    {
        FlagButtonPressRelease = false;
        while (CrossPlatformInputManager.GetButton("LeftOption"))
        {
            if (EmotionSelected[CurrentEmotionSelection])
            {
                // Manipulate Slider
                if (IsSliderZero(EmotionOrder[CurrentEmotionSelection]))
                {
                    if (bufferLeftFlagCounter > bufferLeftFlagMax)
                    {
                        // Deselect this Option
                        LightButton(EmotionOrder[CurrentEmotionSelection], "No");
                        UnlightButton(EmotionOrder[CurrentEmotionSelection], "Yes");
                        EmotionSelected[CurrentEmotionSelection] = false;
                    }
                    else
                    {
                        // Buffer Allowed until Actually Deselecting
                        bufferLeftFlagCounter++;
                    }
                }
                else
                {
                    ManipulateSlider(EmotionOrder[CurrentEmotionSelection], -SliderIncrementValue);
                }
            }
            else
            {
                // Do Nothing
            }
            yield return new WaitForSeconds(CooldownButtonInterval);
        }
        FlagButtonPressRelease = true;
        SliderCoroutine = null;
        yield return null;
    }

    IEnumerator SliderUIUp()
    {
        FlagButtonPressRelease = false;
        while (CrossPlatformInputManager.GetButton("UpOption"))
        {

            // De-Select the Current Option
            DeselectEmotionColor();

            if (CurrentEmotionSelection > 0)
            {
                // Select the Emotion Option on Top
                CurrentEmotionSelection--;
            }
            else
            {
                // Do Nothing, just insure that the right options are selected and we don't go over the index limit.
                CurrentEmotionSelection = 0;
            }

            // Make Sure the Correct Options are Selected
            SelectEmotionColor();

            yield return new WaitForSeconds(CooldownButtonInterval);
        }
        FlagButtonPressRelease = true;
        SliderCoroutine = null;
        yield return null;
    }

    IEnumerator SliderUIDown()
    {
        FlagButtonPressRelease = false;
        while (CrossPlatformInputManager.GetButton("DownOption"))
        {

            // Remove the Color Encoded Selection
            DeselectEmotionColor();

            // Select New Option
            if (CurrentEmotionSelection < EmotionSelected.Count-1)
            {
                // Select the Emotion Option Under
                CurrentEmotionSelection++;
            }
            else
            {
                // Do Nothing, just insure that the right options are selected and we don't go over the index limit.
                CurrentEmotionSelection = EmotionSelected.Count - 1;
            }

            // Add teh Color of the Current New Selection
            SelectEmotionColor();

            yield return new WaitForSeconds(CooldownButtonInterval);
        }
        FlagButtonPressRelease = true;
        SliderCoroutine = null;
        yield return null;
    }

    private string SubmitEmotionData()
    {
        string emotionVals = "";
        for(int i = 0; i < EmotionOrder.Length; i++)
        {
            if (EmotionSelected[i])
            {
                // Emotion was selected!
                GameObject go = EmotionOrder[i];
                emotionVals += go.name;
                emotionVals += "_" + SliderValue(go);
                emotionVals += "\n";
            }
            else
            {
                // Emotion was not selected!
                GameObject go = EmotionOrder[i];
                emotionVals += go.name;
                emotionVals += "_-1";
                emotionVals += "\n";
            }
        }
        return emotionVals;
    }

    private void FinishScene()
    {
        if (LevelManager == null)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            LevelManager.GetComponent<LevelManager>().SwitchScene = true;
        }
    }

    private void ShiftEmotionElements(int ShiftTimes)
    {
        List<GameObject> AuxOrder = new List<GameObject>(EmotionOrder);

        // Function to Shift the a List
        ListShift(AuxOrder, ShiftTimes);

        List<Vector3> vectorList = new List<Vector3>();

        // Get Positions before changing them
        for(int i = 0; i < EmotionOrder.Length; i++)
        {
            vectorList.Add(EmotionOrder[i].transform.position);
        }

        for(int i = 0; i < AuxOrder.Count; i++)
        {
            AuxOrder[i].transform.position = vectorList[i];
        }


        EmotionOrder = AuxOrder.ToArray();

        Debug.Log("Interface Complete");
    }

    public static void ListShift(List<GameObject> list, int n)
    {
        for(int i = 0; i < n; i++)
        {
            GameObject aux = list[list.Count - 1];
            for(int j = list.Count-1; j > 0; j--)
            {
                list[j] = list[j - 1];
            }
            list[0] = aux;
        }
    }


    public void LogUserResponse()
    {
        UserResponseFileName += "_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
        string finalpath = Path.Combine(UserResponseLocation, UserResponseFileName);
        Directory.CreateDirectory(UserResponseLocation);

        using (StreamWriter sw = new StreamWriter(finalpath))
        {
            sw.WriteLine(JsonConvert.SerializeObject(new QuestConfirmValues(EmotionOrder)));
        }
    }

    class QuestConfirmValues
    {
        public EmotionValue[] items;

        public QuestConfirmValues(GameObject[] values)
        {
            List<EmotionValue> final_values = new List<EmotionValue>();
            for (int i = 0; i < values.Length; i++)
            {
                float slider_value = values[i].GetComponentInChildren<Slider>().value;
                final_values.Add(new EmotionValue(values[i].name, slider_value));
            }
            items = final_values.ToArray();
        }
    }

    class EmotionValue
    {
        public string EmotionName;
        public float SliderValue;

        public EmotionValue(string EmotionName, float SliderValue)
        {
            this.EmotionName = EmotionName;
            this.SliderValue = SliderValue;
        }
    }

}
