using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;

public class ShutterScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _shutterHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _shutterHash = Animator.StringToHash("Shutter");
    }

    // Update is called once per frame
    public void TriggerShutter()
    {
        _animator.SetTrigger(_shutterHash);
    }
}
