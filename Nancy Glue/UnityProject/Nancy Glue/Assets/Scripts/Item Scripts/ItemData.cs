using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    //Stores the Item scriptable objects
    [SerializeField] private ItemScriptableObject _evidenceItem;
    public ItemScriptableObject EvidenceItem => _evidenceItem;
}
