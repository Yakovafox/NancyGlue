using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    [SerializeField] private CameraSwitch[] _cameraScripts;
    public string CameraName;
    // Start is called before the first frame update
    private void Awake()
    {
        _cameraScripts = FindObjectsOfType<CameraSwitch>();
    }

    public void OnLoadGame(string targetName)
    {
        //Load from file.
        foreach (var camera in _cameraScripts)
        {
            if (camera.ActiveCam)
            {
                camera.SwitchActiveCam();
                break;
            }
        }
        var target = _cameraScripts.FirstOrDefault(obj => obj.name == targetName);
        target.SwitchActiveCam();
    }
    public void OnSaveGame()
    {
        //Pull what camera is currently active.

        foreach (var camera in _cameraScripts)
        {
            if(camera.ActiveCam)
            {
               CameraName = camera.name;
               break;
            }
        }  
    }
}
