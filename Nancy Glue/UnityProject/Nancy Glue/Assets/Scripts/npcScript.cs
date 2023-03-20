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
    public void ChangeActiveContainer()
    {
        //ActiveContainer = ActiveContainer > DialogueContainers.Count ? ActiveContainer++ : 0;
        ActiveContainer++;
        if (ActiveContainer > _dialogueContainers.Count - 1)
            ActiveContainer = 0;
        Debug.Log("Current Active Container: " + ActiveContainer);
    }

    public void EvidenceCheck(Inventory inventory)
    {
        if (_canBeQuestioned) return;
        foreach (var item in inventory.characterItems) //3 nests deep. Remember and change this.
        {
            for (var i = 0; i < _itemSOs.Count; i++)
            {
                if (item.id == _itemSOs[i].ItemID)
                {
                    _itemSOs.RemoveAt(i);
                }
            }
        }

        _canBeQuestioned = _itemSOs.Count > 0;
    }
}
