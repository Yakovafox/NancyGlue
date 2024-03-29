using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private LocationButton _locationButton;
    [SerializeField] private Sprite[] _cameraIcons;

    private void Awake()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
        _myCollider = GetComponent<Collider>();
        _cameraTransform = transform.GetChild(0);
    }
    void Start()
    {
        _isRoot = _rootCameraTransform != null ? false: true; //Used to set Root camera in hierarchy
        _canSwitch = _switchableCameras.Count != 0 ? true : false; //assigning switchable cameras in the list sets this to true, false if empty
        EnableDisableCam();
    }

    public void SwitchActiveCam()
    {
        /*
         * switches the active camera in the game.
         * Sets appropriate camera icon for dolly cameras and stationary cameras
         * Disables or enables the camera appropriately to match the Active camera bool
         */
        
        _activeCam = !_activeCam;

        var cameraOnDolly = _cameraTransform.GetComponent<CameraMovement>().OnDolly;
        var image = GameObject.Find("CameraIcon").GetComponent<Image>();
        if (cameraOnDolly)
        {
            image.sprite = _cameraIcons[1];
        }
        else
            image.sprite = _cameraIcons[0];
        
        if (_locationButton != null)
            _locationButton.IsCurrentLocation = _activeCam;
        EnableDisableCam();
        EnableReturnTooltip();
        if (_activeCam)
        {
            var zoneManager = FindObjectOfType<ZoneManager>();
            zoneManager.CurrentCamera = this;
        }
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
