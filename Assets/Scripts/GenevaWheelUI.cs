using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GenevaWheelUI : MonoBehaviour
{
    public Button[,] ButtonMatrix;
    public Button Neutral;

    private bool FlagButtonPressRelease = true;

    private int TierCounter = -1;
    private int EmotionTypeCounter = -1;

    public float CooldownButtonInterval = 1;
    private float CurrentCooldown = 0;

    private GameObject LevelManager;

    // Start is called before the first frame update
    void Start()
    {
        CreateButtonMatrix(GameObject.FindGameObjectsWithTag("GenevaUIButton"));
        Neutral.GetComponent<Image>().color = Neutral.colors.highlightedColor;

        LevelManager = GameObject.Find("LevelManagerObject");
    }


    // Update is called once per frame
    void Update()
    {
        float yAxis = CrossPlatformInputManager.GetAxis("Vertical");
        float xAxis = CrossPlatformInputManager.GetAxis("Horizontal");

        if (FlagButtonPressRelease)
        {
            if(yAxis > 0)
            {
                FlagButtonPressRelease = false;

                // Going Up!              
                ResetColor();
                if(TierCounter == -1)
                {
                    TierCounter = 0;
                    EmotionTypeCounter = 0;
                }
                else
                {
                    TierCounter++;
                    if(TierCounter >= ButtonMatrix.GetLength(1))
                    {
                        TierCounter = ButtonMatrix.GetLength(1) - 1;
                    }
                }
            }
            else if(yAxis < 0)
            {
                FlagButtonPressRelease = false;

                // Going Down!
                ResetColor();
                if (TierCounter == 0)
                {
                    TierCounter = -1;
                    EmotionTypeCounter = -1;
                }
                else
                {
                    if(TierCounter != -1)
                        TierCounter--;
                }

            }

            if(xAxis > 0)
            {
                FlagButtonPressRelease = false;
                // Going Right
                if (EmotionTypeCounter != -1)
                {
                    ResetColor();
                    EmotionTypeCounter++;
                    if(EmotionTypeCounter >= ButtonMatrix.GetLength(0))
                    {
                        EmotionTypeCounter = 0;
                    }
                    else if (TierCounter == -1 && EmotionTypeCounter == -1)
                    {
                        // Do Nothing
                    }
                }
            }
            else if(xAxis < 0)
            {
                FlagButtonPressRelease = false;
                // Going Left
                if (EmotionTypeCounter != -1)
                {
                    ResetColor();
                    EmotionTypeCounter--;
                    if (EmotionTypeCounter < 0)
                    {
                        EmotionTypeCounter = ButtonMatrix.GetLength(0) - 1;
                    }
                    else if (TierCounter == -1 && EmotionTypeCounter == -1)
                    {
                        // Do Nothing
                    }
                }
            }
        }
        else
        {
            if(CurrentCooldown > CooldownButtonInterval)  //if(yAxis == 0 && xAxis == 0)
            {
                FlagButtonPressRelease = true;
                CurrentCooldown = 0;
            }
            else
            {
                yAxis = 0;
                xAxis = 0;
                CurrentCooldown += Time.deltaTime;
            }
        }

        ColorButton();
        // Debug.Log(ButtonMatrix.GetLength(0) + " " + ButtonMatrix.GetLength(1));
        // Debug.Log("Matrix Space - Tier: " + TierCounter + " EmotionCounter: " + EmotionTypeCounter);

        if (CrossPlatformInputManager.GetButtonDown("Submit"))
        {
            if(EmotionTypeCounter != -1 && TierCounter != -1)
            {
                Button ChosenButton = ButtonMatrix[EmotionTypeCounter, TierCounter];
                ChosenButton.GetComponent<Image>().color = ChosenButton.colors.selectedColor;
                Debug.Log("Chosen Tier = " + ChosenButton.name + " // Chosen Emotion = " + ChosenButton.transform.parent.name);
                FinishScene();
            }
            else
            {
                Debug.Log("Can't Select Neutral Position!");
            }
        }
    }


    private void CreateButtonMatrix(GameObject[] EmotionTypes)
    {
        ButtonMatrix = new Button[10, 4];
        foreach(GameObject go in EmotionTypes)
        {
            int EmotionIndex = EmotionIndexer(go.name);
            foreach(Button bu in go.GetComponentsInChildren<Button>())
            {
                if(bu.name == "Tier 1")
                {
                    ButtonMatrix[EmotionIndex, 0] = bu;
                }
                else if (bu.name == "Tier 2")
                {
                    ButtonMatrix[EmotionIndex, 1] = bu;
                }
                else if (bu.name == "Tier 3")
                {
                    ButtonMatrix[EmotionIndex, 2] = bu;
                }
                else if (bu.name == "Tier 4")
                {
                    ButtonMatrix[EmotionIndex, 3] = bu;
                }
            }
        }
    }


    private int EmotionIndexer(string emotiolabel)
    {
        if (emotiolabel == "Pleasure")
        {
            return 0;
        }
        else if (emotiolabel == "Love")
        {
            return 1;
        }
        else if (emotiolabel == "Interest")
        {
            return 2;
        }
        else if (emotiolabel == "Pride")
        {
            return 3;
        }
        else if (emotiolabel == "Anger")
        {
            return 4;
        }
        else if (emotiolabel == "Disgust")
        {
            return 5;
        }
        else if (emotiolabel == "Dissapointment")
        {
            return 6;
        }
        else if (emotiolabel == "Regret")
        {
            return 7;
        }
        else if (emotiolabel == "Sadness")
        {
            return 8;
        }
        else if (emotiolabel == "Relief")
        {
            return 9;
        }
        else
        {
            return -1; // ERROR
        }
    }


    private void ColorButton()
    {
        if(TierCounter == -1 && EmotionTypeCounter == -1)
        {
            Neutral.GetComponent<Image>().color = Neutral.colors.highlightedColor;
        }
        else
        {
            ButtonMatrix[EmotionTypeCounter, TierCounter].GetComponent<Image>().color = ButtonMatrix[EmotionTypeCounter, TierCounter].colors.highlightedColor;
        }
    }


    private void ResetColor()
    {
        if (TierCounter == -1 && EmotionTypeCounter == -1)
        {
            Neutral.GetComponent<Image>().color = Neutral.colors.normalColor;
        }
        else
        {
            ButtonMatrix[EmotionTypeCounter, TierCounter].GetComponent<Image>().color = ButtonMatrix[EmotionTypeCounter, TierCounter].colors.normalColor;
        }
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
}
