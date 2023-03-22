using Dialogue.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class npcScript : MonoBehaviour, IEvidenceCheck
{
    [SerializeField] private List<DialogueContainerSO> _dialogueContainers;
    public List<DialogueContainerSO> DialogueContainers => _dialogueContainers;
    public int ActiveContainer = 0;

    [SerializeField] private List<ItemScriptableObject> _itemSOs;
    [SerializeField] private bool _canBeQuestioned;
    public bool CanBeQuestioned => _canBeQuestioned;
    public int normalDialogueLimit;

    [SerializeField] private bool _spokenTo;
    public void ChangeActiveContainer()
    {
        //ActiveContainer = ActiveContainer > DialogueContainers.Count ? ActiveContainer++ : 0;
        ActiveContainer++;
        if (!_canBeQuestioned)
        {
            if (ActiveContainer > _dialogueContainers.Count - normalDialogueLimit)
                ActiveContainer = _dialogueContainers.Count - normalDialogueLimit;
        }
        else
        {
            if(ActiveContainer > _dialogueContainers.Count - 1)
            {
                ActiveContainer = _dialogueContainers.Count - 1;
            }
        }


        /*
         * if canbequestioned false
         *      check if the active container is > the count - limit for normal dialogue
         *      set to the limit of normal dialogue
         * else
         *      check if the active container is > than the count - 1
         *      set to limit of dialogue count
         */

        //Debug.Log("Current Active Container: " + ActiveContainer);
    }

    public void EvidenceCheck(Inventory inventory)
    {
        if (_canBeQuestioned) return;
        Debug.Log("Checking Evidence");
        foreach (var item in inventory.characterItems) //3 nests deep. Remember and change this.
        {
            Debug.Log(item.title);
            for (var i = 0; i < _itemSOs.Count; i++)
            {
                if (item.id == _itemSOs[i].ItemID)
                {
                    _itemSOs.RemoveAt(i);
                    break;
                }
            }
        }

        _canBeQuestioned = _itemSOs.Count == 0 ? true : false;
    }

    public void SpeakToPlayer()
    {
        _spokenTo = true;
        if(!_canBeQuestioned)
            ChangeActiveContainer();
    }
}
