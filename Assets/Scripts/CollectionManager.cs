using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{

    private List<GameObject> Collectibles;
    public Text coinsLeft;

    // Start is called before the first frame update
    void Start()
    {
        Collectibles = new List<GameObject>();

        coinsLeft = GameObject.FindGameObjectWithTag("Collectible UI").GetComponent<Text>();

        Transform[] allCollectibles = gameObject.GetComponentsInChildren<Transform>();
        int i = 0;

        foreach (Transform col in allCollectibles)
        {
            if(col.gameObject.tag == "Collectible")
            {
                Collectibles.Add(col.gameObject);
                i++;
            }
        }
    }

    private void Update()
    {
        try
        {
            UpdateText();
        }
        catch (NullReferenceException e)
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                Debug.Log("No Reference to Collectible UI Avaiable. Is the player in the scene?");
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
        }
    }

    public void UpdateCollectibleList(GameObject coinObj)
    {
        for(int i = 0; i < Collectibles.Count; i++)
        {
            if(Collectibles[i].name == coinObj.name)
            {
                Collectibles.RemoveAt(i);
                Destroy(coinObj);
            }
        }
    }

    private void UpdateText()
    {
        coinsLeft.text = "Collectibles: " + Collectibles.Count;
    }

    public int CollectiblesLeft()
    {
        return Collectibles.Count;
    }

    public string[] GetAllCollectibleID()
    {
        string[] collectibleIDs = new string[Collectibles.Count];
        int counter = 0;
        foreach (GameObject g in Collectibles)
        {
            collectibleIDs[counter] = g.name;
            counter++;
        }
        return collectibleIDs;
    }
}
