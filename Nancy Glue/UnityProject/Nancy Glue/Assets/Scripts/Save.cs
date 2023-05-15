using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//save data class
[System.Serializable]
public class Save
{
    public int[] inventoryTS;
    public string SavedCameraName;
    public int[] dialogueDiaIteratorsToSave;
    public int[] dialogueIntIteratorsToSave;
    public string[] NPCnames;
    public int gameStage;
    public string[] suspectsTS;
    public int[] gameStageArray;
    public string[][] notes;
}
