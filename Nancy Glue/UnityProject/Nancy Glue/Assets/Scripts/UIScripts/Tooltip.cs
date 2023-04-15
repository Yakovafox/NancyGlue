using System;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]private float _timer;
    [SerializeField][Range(0,5)]private const int LIFESPAN = 5;
    [SerializeField]private bool _open;
    [SerializeField]private Animator _animator;

    private int _switchHash;
    [SerializeField] private TextMeshProUGUI _toolTipText;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _toolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        _switchHash = Animator.StringToHash("Switch");
        _timer = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_open)
            IncreaseTimer();
    }

    private void IncreaseTimer()
    {
        if (!(_timer >= LIFESPAN)) //if timer not at threshold, increase and return.
        {
            _timer += 1f * Time.deltaTime;
            return;
        }
        _timer = 0; //reset the timer and close the tooltip
        CloseTooltip();
    }
    public void OpenTooltip(String tooltipText)
    {
        _toolTipText.text = tooltipText;
        _animator.SetTrigger(_switchHash);
        _open = true;
    }

    private void CloseTooltip()
    {
        _animator.SetTrigger(_switchHash);
        _open = false;
    }
}
