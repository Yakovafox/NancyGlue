using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScript : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private CameraSwitch _currentCamera;
    [SerializeField] private mouseTrack _mouseTrack;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _mouseTrack = GetComponent<mouseTrack>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
