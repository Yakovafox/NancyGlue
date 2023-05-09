using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefcaseScript : MonoBehaviour, IClickable
{
    [SerializeField] private Sprite[] _sprites = new Sprite[2];
    public SpriteRenderer spriteRenderer;
    public bool Clicked;
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = _sprites[0];
    }

    public void Clickable()
    {
        //spriteRenderer.sprite = _sprites[1];
        Clicked = true;
    }
}
