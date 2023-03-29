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
    public NPCTracker NPCTrack;
    private void Awake()
    {
        inv= FindObjectOfType<Inventory>(); 
        CameraTracker=FindObjectOfType<CameraTrack>();
        NPCTrack=FindObjectOfType<NPCTracker>();
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

        NPCTrack.OnSaveGame();


        for (int i = 0; i < NPCsaveData.diaData.Count; i++)
        {
            save.dialogueIteratorsToSave[i] = NPCsaveData.diaData.ElementAt(i).Value;
            save.NPCnames[i] = NPCsaveData.diaData.ElementAt(i).Key;
            
        }

        
        
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

            //this needs to be loaded into inv/ui after this

            
            
            
            Debug.Log("Game Loaded");

            
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
    
}





