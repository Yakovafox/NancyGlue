using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefcaseScript : MonoBehaviour, IClickable
{
    [SerializeField] private Sprite[] _sprites = new Sprite[2];
    public SpriteRenderer spriteRenderer;
    public bool Clicked;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable() //display the default sprite
    {
        spriteRenderer.sprite = _sprites[0];
    }

    public void Clickable() //set the object as clicked
    {
        Clicked = true;
    }
}
