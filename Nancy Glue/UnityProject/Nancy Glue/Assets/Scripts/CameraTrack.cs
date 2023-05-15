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
        _cameraScripts = FindObjectsOfType<CameraSwitch>(true);
    }

    public void OnLoadGame(string targetName)
    {
        //Load last camera, switch off all cameras and turn on the loaded one
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
        //Pull what camera is currently active for saving

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
