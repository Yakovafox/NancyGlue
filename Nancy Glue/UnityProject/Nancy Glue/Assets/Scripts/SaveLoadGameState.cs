using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class SaveLoadGameState : MonoBehaviour
{
    
    //string gameStageToSave;
    //int camToSave;
    public Inventory inv;
    public CameraTrack CameraTracker;
    public NPCTracker[] NPCTrackers;
    
    
    private void Awake()
    {
        inv= FindObjectOfType<Inventory>(); 
        CameraTracker=FindObjectOfType<CameraTrack>();
        NPCTrackers = FindObjectsOfType<NPCTracker>();


    }





    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
                      
        
        Debug.Log("Game Saved");
    }




    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        save.inventoryTS = inv.savedIDs;
        CameraTracker.OnSaveGame();
        save.SavedCameraName = CameraTracker.CameraName;
        Debug.Log("attempting to save dialogue data");
        //NPCTrack.OnSaveGame();
        int[] tempIterators=new int[NPCTrackers.Length];
        string[] tempNames = new string[NPCTrackers.Length];

        
        for (int i = 0; i < NPCTrackers.Length; i++)
        {
            
            Debug.Log(NPCTrackers[i].attachedNPC);
            Debug.Log(NPCTrackers[i].dialogueIterator);
            tempIterators[i] = NPCTrackers[i].dialogueIterator;
            tempNames[i] = NPCTrackers[i].attachedNPC;


        }

        save.dialogueIteratorsToSave = tempIterators;
        save.NPCnames = tempNames;
        

        return save;
    }
    public void Load()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            
            
            //reset inv etc if not already done. 

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            
            inv.savedIDs = save.inventoryTS;
            CameraTracker.OnLoadGame(save.SavedCameraName);
            
            //load dialogue data
            for (int i = 0; i < NPCTrackers.Length; i++)
            {
                NPCTrackers[i].onLoadGame(save.NPCnames[i], save.dialogueIteratorsToSave[i]);
            }

            




            Debug.Log("Game Loaded");

            
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
    
}





