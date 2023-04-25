using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    [SerializeField] private CameraSwitch _currentCamera;

    [SerializeField] private CameraSwitch _targetLocation;
    [SerializeField] private Animator _transition;
    [field: SerializeField] public bool IsCurrentLocation { get; set; }
    [SerializeField] public Button _button;
    [SerializeField] private Color _unSelectedColor;
    [SerializeField] private Color _selectedColor;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _unSelectedColor = _button.colors.normalColor;
        _selectedColor = _button.colors.selectedColor;
    }

    private void FixedUpdate()
    {
        var colors = _button.colors;
        colors.normalColor = IsCurrentLocation ? _selectedColor : _unSelectedColor;
        _button.colors = colors;
    }

    public void ButtonClick()
    {
        CameraSwitch rootCamera;
        var cameraName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        var cameraSwitch = GameObject.Find(cameraName).transform.parent;
        _currentCamera = cameraSwitch.GetComponent<CameraSwitch>();

        if (!IsCurrentLocation)
        {
            var locationButtons = FindObjectsOfType<LocationButton>();
            foreach (var button in locationButtons)
            {
                if(button.gameObject != gameObject)
                    button.IsCurrentLocation = false;
            }
            IsCurrentLocation = true;
            _currentCamera.SwitchActiveCam();
            _targetLocation.SwitchActiveCam();
        }

        /*
        if (_currentCamera.RootCamera != null)
        {
            rootCamera = _currentCamera.RootCamera.GetComponent<CameraSwitch>();
            if (rootCamera == _targetLocation) return;
            _currentCamera.SwitchActiveCam();
            _targetLocation.SwitchActiveCam();
        }
        else
        {
            if (_currentCamera == _targetLocation) return;
            _currentCamera.SwitchActiveCam();
            _targetLocation.SwitchActiveCam();
        }
        */
    }

    public void LocationCheck()
    {
        if (IsCurrentLocation) return;
        var locationButtons = FindObjectsOfType<LocationButton>();
        foreach (var button in locationButtons)
        {
            if (button.gameObject != gameObject)
                button.IsCurrentLocation = false;
        }
        IsCurrentLocation = true;
    }
}
