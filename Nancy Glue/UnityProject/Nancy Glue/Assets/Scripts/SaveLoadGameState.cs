using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class SaveLoadGameState : MonoBehaviour
{
    
    
    public Inventory inv;
    public CameraTrack CameraTracker;
    public NPCTracker[] NPCTrackers;
    public GameManager1 gameManager;
    public SuspectPage suspectPage;
    private void Awake()
    {
        inv= FindObjectOfType<Inventory>(); 
        CameraTracker=FindObjectOfType<CameraTrack>();
        NPCTrackers = FindObjectsOfType<NPCTracker>();
        gameManager = FindObjectOfType<GameManager1>();
        suspectPage = FindObjectOfType<SuspectPage>();
    }





    public void SaveGame()
    {
        inv.SaveInv();
        suspectPage.saveNames();
        
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
                      
        
        
    }




    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        save.inventoryTS = inv.savedIDs;
        CameraTracker.OnSaveGame();
        save.SavedCameraName = CameraTracker.CameraName;
        
        int[] tempDiaIterators = new int[NPCTrackers.Length];
        int[] tempIntIterators = new int[NPCTrackers.Length];
        string[][] tempNotes = new string[NPCTrackers.Length][];
        string[] tempNames = new string[NPCTrackers.Length];

        
        for (int i = 0; i < NPCTrackers.Length; i++)
        {
            tempDiaIterators[i] = NPCTrackers[i].dialogueIterator;
            tempIntIterators[i] = NPCTrackers[i].interrogationIterator;
            tempNotes[i] = NPCTrackers[i].Notes.ToArray();
            tempNames[i] = NPCTrackers[i].attachedNPC;


        }

        save.dialogueDiaIteratorsToSave = tempDiaIterators;
        save.dialogueIntIteratorsToSave = tempIntIterators;
        save.notes = tempNotes;

        save.NPCnames = tempNames;
        save.gameStage = (int)gameManager._gameState;
        save.suspectsTS = suspectPage.SuspectNames;
        save.gameStageArray = gameManager.stateTracker;
        return save;
    }
    
    public void Load()
    {
        
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            
            inv.savedIDs = save.inventoryTS;

            gameManager._gameState = (GameManager1.GameState)save.gameStage;
            gameManager.stateTracker = save.gameStageArray;
            suspectPage.SuspectNames = save.suspectsTS;

            FindObjectOfType<invUI>().LoadInvUI();
            FindObjectOfType<GameManager1>().LoadProgress();
            FindObjectOfType<SuspectPage>().LoadSuspectPage();
            FindObjectOfType<Inventory>().LoadInventory();
            CameraTracker.OnLoadGame(save.SavedCameraName);
            
            //load dialogue data
            for (int i = 0; i < NPCTrackers.Length; i++)
            {
                GameObject.Find(save.NPCnames[i]).GetComponent<NPCTracker>().onLoadGame(save.NPCnames[i], save.dialogueDiaIteratorsToSave[i], save.dialogueIntIteratorsToSave[i], save.notes[i]);
            }

             
        }
        
    }
    private void OnApplicationQuit()
    {
        

        //call the save
        SaveGame();
        
        //save settings to file 

    }
}





