using Cinemachine;
using UnityEngine;

public class LocationButton : MonoBehaviour
{
    [SerializeField] private CameraSwitch _currentCamera;

    [SerializeField] private CameraSwitch _targetLocation;
    [SerializeField] private Animator _transition;

    public void ButtonClick()
    {
        CameraSwitch rootCamera;
        var cameraName = Camera.main.transform.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Name;
        var cameraSwitch = GameObject.Find(cameraName).transform.parent;
        _currentCamera = cameraSwitch.GetComponent<CameraSwitch>();
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
    }
}
