using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject _evidenceItem;
    public ItemScriptableObject EvidenceItem => _evidenceItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
