using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField]private float _timer = 0.0f;
    [SerializeField][Range(0,5)]private const int LIFESPAN = 4;
    [SerializeField]private bool _closed;
    [SerializeField]private Animator _animator;
    private Queue tooltipQueue = new Queue();

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
        _closed = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (tooltipQueue.Count > 0 || !_closed) IncreaseTimer();
    }

    private void IncreaseTimer()
    {
        if (_closed && _timer > 0.5) OpenTooltip();

        if (_timer <= LIFESPAN) //if timer not at threshold, increase and return.
        {
            _timer += Time.deltaTime;
            Debug.LogError(_timer);

            return;
        }
        _timer = 0; //reset the timer and close the tooltip
        CloseTooltip();
    }

    public void EnqueueTooltip(String tooltipText)
    {
        //if (tooltipQueue.Count > 0)
        //    _timer = LIFESPAN;
        tooltipQueue.Enqueue(tooltipText);
    }

    public void OpenTooltip()
    {
        _toolTipText.text = (String) tooltipQueue.Peek();
        _animator.SetTrigger(_switchHash);
        _closed = false;
    }

    private void CloseTooltip()
    {
        tooltipQueue.Dequeue();
        _animator.SetTrigger(_switchHash);
        _closed = true;
    }
}
