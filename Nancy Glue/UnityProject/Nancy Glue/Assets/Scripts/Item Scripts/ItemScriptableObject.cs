using UnityEngine;

[CreateAssetMenu(fileName = "New Item",menuName = "Evidence Item")]
public class ItemScriptableObject : ScriptableObject
{
    [SerializeField] private int _itemID;
    public int ItemID => _itemID;
    [SerializeField] private string _title;
    public string Title => _title;
    [SerializeField] private string _description;
    public string Description => _description;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;

}
