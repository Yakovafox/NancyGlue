using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private CameraSwitch _driveInCam;
    public CameraSwitch DriveInCam => _driveInCam;
    [SerializeField] private CameraSwitch _officeCam;
    public CameraSwitch OfficeCam => _officeCam;
    [SerializeField] private CameraSwitch _alleyCam;
    public CameraSwitch AlleyCam => _alleyCam;      
    [SerializeField] private CameraSwitch _evidenceBoardCam;
    public CameraSwitch EvidenceBoardCam => _evidenceBoardCam;  
    [SerializeField] private CameraSwitch _basementCam;
    public CameraSwitch BasementCam => _basementCam;
}
