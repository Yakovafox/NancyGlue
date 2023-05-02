using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class SuspectPage : MonoBehaviour
{
    public GameObject SuspectPrefab;
    public string[] SuspectNames;
    public SaveLoadGameState SLGS;
    public List<string> suspectList = new List<string>();
    public NPCTracker tracker;

    private void Awake()
    {
        SLGS = GameObject.FindObjectOfType<SaveLoadGameState>();
    }

    private void Start()
    {
        if (NewOrLoad.isLoad)
        {
            //for


            //check if there are items in the saved inv 
            SLGS.Load();
            if (IsNullOrEmpty(SLGS.suspectPage.SuspectNames))
            {
                //if empty then we're good
                Debug.Log("loaded sus list is empty");
            }
            else
            {
                Debug.Log("loaded sus list is not empty");
                var susEmptyText = GameObject.Find("BlankTextSuspect");
                if (susEmptyText != null && susEmptyText.activeSelf)
                    susEmptyText.SetActive(false);


                //SuspectNames=SLGS.
                //load suspects into UI
                for (int i = 0; i < SLGS.suspectPage.SuspectNames.Length; i++)
                {
                    tracker = GameObject.Find(SLGS.suspectPage.SuspectNames[i]).GetComponent<NPCTracker>();

                    var Suspect = Instantiate(SuspectPrefab, transform.GetChild(0));
                    Suspect.GetComponent<SuspectMugshot>().SetData(tracker.CharName, tracker.CharacterSprite);
                    Suspect.name = Suspect.GetComponent<SuspectMugshot>().Name;
                    Suspect.GetComponent<SuspectMugshot>()._npcTracker = tracker;
                    tracker.SpokenTo = true;
                    suspectList.Add(SLGS.suspectPage.SuspectNames[i]);
                    Debug.Log("loaded suspect " + SLGS.suspectPage.SuspectNames[i]);
                }



            }
        }
    }
    public static bool IsNullOrEmpty(Array array)
    {
        return (array == null || array.Length == 0);
    }



    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void saveNames()
    {
        SuspectNames = suspectList.Select(x => x).ToArray();
        Debug.Log("saved suspects");
    }



}
