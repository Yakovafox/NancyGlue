using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("Introduction")]
    [SerializeField] private CameraSwitch _officeCam;
    public CameraSwitch OfficeCam => _officeCam;

    [Header("Alley")]
    [SerializeField] private CameraSwitch _alleyCam;
    public CameraSwitch AlleyCam => _alleyCam;
    [field: SerializeField] public bool AlleyUnlocked { get; set; }

    [Header("Evidence Board")]
    [SerializeField] private CameraSwitch _evidenceBoardCam;
    public CameraSwitch EvidenceBoardCam => _evidenceBoardCam;
    [field: SerializeField] public bool BoardUnlocked { get; set; }

    [Header("Basement")]
    [SerializeField] private CameraSwitch _basementCam;
    public CameraSwitch BasementCam => _basementCam;

    [Header("Drive-In")]
    [SerializeField] private CameraSwitch _driveInCam;
    public CameraSwitch DriveInCam => _driveInCam;
    [field: SerializeField]public bool DriveInUnlocked { get; set; }

    [Header("Drive-In Investigation")]
    [SerializeField] private bool _spokeToTed;
    public bool SpokeToTed { get => _spokeToTed; set => _spokeToTed = value; }
    [SerializeField] private bool _confrontedTed;
    public bool ConfrontedTed { get => _confrontedTed; set => _confrontedTed = value; }

    public void SpeakToNPC(string name)
    {
        switch (name)
        {
            /*
             * Speak to ted when bool is false
             *          turn on the camera for projector room
             *          set bool to true
             */
            case "TedGrizzly" when !_spokeToTed:
                _driveInCam.SwitchableCameras[1].gameObject.SetActive(true);
                _driveInCam.SwitchableCameras[1].GetComponent<CameraSwitch>().SwitchableCameras[0].gameObject.SetActive(true);
                _spokeToTed = true;
                break;
        }
    }
}
