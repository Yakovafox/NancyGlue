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

    private void FixedUpdate() //updates the colour of the button based on the IsCurrentLocation variable.
    {
        var colors = _button.colors;
        colors.normalColor = IsCurrentLocation ? _selectedColor : _unSelectedColor;
        _button.colors = colors;
    }

    
    public void ButtonClick()
    {
        /*
         * when clicked the player is moved to the appropriate starting camera for the location
         * e.g. bearly pictures moves the player to the main camera between the drive in and the projector room.
         * marks the location as the active location switching the colour
         */
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
    }

    //public call to check the current location of the player and update the location buttons appropriately
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
