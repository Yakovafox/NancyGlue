using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
