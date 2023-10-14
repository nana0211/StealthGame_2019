using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // If Time Limit is 0 it means it is unlimited time
    public float TimeLimit = 0;
    public bool RandomizeSceneOrder = true;
    public int NumOfRepetitions = 10;

    // This Variable is mostly used to keep consistency for the questionnaire section.
    public int RandomSeed;

    [HideInInspector] public float CurrentTime = 0;
    [HideInInspector] public int CurrentIteration = 0;

    public SceneReference BaselineScene;
    public SceneReference QuestionnaireScene;
    public SceneReference EndScene;
    public List<SceneReference> SceneOrder;

    [HideInInspector] public bool SwitchScene = false;
    [HideInInspector] public int ScenarioFlag = 0; // This is the Scenario Flag where: 0 = Running Baseline; 1 = Runnning Level; 2 = Running Questionnaire.
    [HideInInspector] public int ScenarioCounter = 0;

    private JSONLogger loggingSystem; 

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        CurrentTime = TimeLimit;

        ShuffleSceneOrder();
        RandomSeed = Random.Range(0, 9);

        loggingSystem = GetComponent<JSONLogger>();

        // Load the Baseline
        SceneManager.LoadScene(BaselineScene);

    }

    // Update is called once per frame
    void Update()
    {
        // Level Has Finished -- Time to Switch the Scene.
        if (SwitchScene)
        {
            SwitchScene = false;
            CurrentTime = TimeLimit;

            // If the Player is currently in the baseline
            if (ScenarioFlag == 0)
            {
                // Are there more Scenarios?
                if (ScenarioCounter < SceneOrder.Count)
                {
                    // Load the Following Scenario
                    SceneManager.LoadScene(SceneOrder[ScenarioCounter]);
                    ScenarioCounter++;
                    ScenarioFlag = 1;
                }
                else
                {
                    // It is the End of the Experiment! -- Ideally the code will never reach this point, but it is better safe than sorry!
                    SceneManager.LoadScene(EndScene);
                }
            }
            // If the Player is currently playing a level switch to Questionnaire
            else if (ScenarioFlag == 1)
            {
                SceneManager.LoadScene(QuestionnaireScene);
                ScenarioFlag = 2;
            }
            // If the Player is currently answering a Questionnaire
            else if (ScenarioFlag == 2)
            {
                // First Check if there are more Scenarios -- If Not we don't run the Baseline
                if (ScenarioCounter >= SceneOrder.Count)
                {
                    if(CurrentIteration < NumOfRepetitions)
                    {
                        CurrentIteration++;
                        ShuffleSceneOrder(); // Shuffle the Scenes
                        
                        // Switch to the Baseline Scenario
                        SceneManager.LoadScene(BaselineScene);
                        ScenarioFlag = 0;
                        ScenarioCounter = 0;
                    }
                    else
                    {
                        // It is the End of the Experiment!
                        SceneManager.LoadScene(EndScene);
                    }
                }
                else
                {
                    // Switch to the Baseline Scenario
                    SceneManager.LoadScene(BaselineScene);
                    ScenarioFlag = 0;
                }
            }
        }

        if(ScenarioFlag == 1 && TimeLimit > 0)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime < 0)
            {
                SwitchScene = true;
            }
        }
    }

    void ShuffleSceneOrder()
    {
        if (RandomizeSceneOrder && SceneOrder.Count > 1)
        {
            int NumberOfSwaps = 20;
            while (NumberOfSwaps > 0)
            {
                int firstNum = Random.Range(0, SceneOrder.Count);
                int secondNum = Random.Range(0, SceneOrder.Count);
                while (firstNum == secondNum)
                {
                    secondNum = Random.Range(0, SceneOrder.Count);
                }
                SwapItems(firstNum, secondNum, SceneOrder);
                NumberOfSwaps--;
            }
        }
    }

    private void SwapItems(int m, int n, List<SceneReference> list)
    {
        SceneReference tmp = list[m];
        list[m] = list[n];
        list[n] = tmp;
    }
}
