using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites = new Sprite[2];
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update() 
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.transform == transform)
                _spriteRenderer.sprite = _sprites[0];
        }
        _spriteRenderer.sprite = _sprites[1];
           
    }
}
