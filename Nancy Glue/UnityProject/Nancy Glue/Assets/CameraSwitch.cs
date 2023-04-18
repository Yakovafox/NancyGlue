using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private bool _activeCam;
    public bool ActiveCam => _activeCam;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private MeshRenderer _myMeshRenderer;
    [SerializeField] private Collider _myCollider;
    [SerializeField] private bool _isRoot;
    public bool IsRoot { get => _isRoot; }
    [SerializeField] private Transform _rootCameraTransform; //add root camera in Unity Editor.
    public Transform RootCamera { get => _rootCameraTransform; }
    [SerializeField] private List<Transform> _switchableCameras; //add cameras in through Unity Editor.
    public List<Transform> SwitchableCameras => _switchableCameras;
    [SerializeField] private bool _canSwitch;
    public bool CanSwitch { get => _canSwitch; }

    public GameObject _returnTooltip;

    private void Awake()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
        _myCollider = GetComponent<Collider>();
        _cameraTransform = transform.GetChild(0);
        _returnTooltip = GameObject.Find("CameraReturnPrompt");
    }
    void Start()
    {
        _isRoot = _rootCameraTransform != null ? false: true; //Used to set Root camera in hierarchy
        _canSwitch = _switchableCameras.Count != 0 ? true : false; //assigning switchable cameras in the list sets this to true, false if empty
        EnableDisableCam();
    }

    public void SwitchActiveCam()
    {
        _activeCam = !_activeCam;
        EnableDisableCam();
        EnableReturnTooltip();
    }

    public void EnableDisableCam()
    {
        _cameraTransform.gameObject.SetActive(_activeCam);
        _myMeshRenderer.enabled = !_activeCam;
        _myCollider.enabled = !_activeCam;
    }

    public void EnableReturnTooltip()
    {
        _returnTooltip.SetActive(!_isRoot); //Enable the tooltip if the camera is not a root camera.
    }


}
