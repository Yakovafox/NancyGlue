using Dialogue.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcScript : MonoBehaviour
{
    public List<DialogueContainerSO> DialogueContainers;
    public int ActiveContainer = 0;

    public void ChangeActiveContainer()
    {
        //ActiveContainer = ActiveContainer > DialogueContainers.Count ? ActiveContainer++ : 0;
        ActiveContainer++;
        if (ActiveContainer > DialogueContainers.Count - 1)
            ActiveContainer = 0;
        Debug.Log("Current Active Container: " + ActiveContainer);
    }
}
