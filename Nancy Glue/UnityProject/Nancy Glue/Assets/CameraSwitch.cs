using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private bool _activeCam;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private MeshRenderer _myMeshRenderer;
    [SerializeField] private bool _isRoot;
    public bool IsRoot { get => _isRoot; }
    [SerializeField] private Transform _rootCameraTransform; //add root camera in Unity Editor.
    public Transform RootCamera { get => _rootCameraTransform; }
    [SerializeField] private List<Transform> _switchableCameras; //add cameras in through Unity Editor.
    [SerializeField] private bool _canSwitch;
    public bool CanSwitch { get => _canSwitch; }

    private void Awake()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
        _cameraTransform = transform.GetChild(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        _isRoot = _rootCameraTransform != null ? false: true;
        _canSwitch = _switchableCameras.Count != 0 ? true : false;
        EnableDisableCam();
    }

    public void SwitchActiveCam()
    {
        _activeCam = !_activeCam;
        EnableDisableCam();
    }

    private void EnableDisableCam()
    {
        _cameraTransform.gameObject.SetActive(_activeCam);
        _myMeshRenderer.enabled = !_activeCam;
    }
}
