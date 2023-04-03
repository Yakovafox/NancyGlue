using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    //    private CameraMovement CamMovementScript;

    //    public Transform ActiveCam;

    //    // Start is called before the first frame update
    //    void Awake()
    //    {
    //        CamMovementScript = GameObject.Find("DriveInMainCamera").GetComponent<CameraMovement>();
    //    }

    //    // Update is called once per frame
    //    void LateUpdate()
    //    {
    //        for(int i = 0; i < CamMovementScript._switchableCameras.Count; i++)
    //        {
    //            if (CamMovementScript._switchableCameras[i].gameObject.activeSelf)
    //            {
    //                ActiveCam = CamMovementScript._switchableCameras[i];
    //            }
    //            if (!CamMovementScript._switchableCameras[i].gameObject.activeSelf)
    //            {
    //                continue;
    //            }
    //        }
    //        transform.LookAt(ActiveCam);


    //        //transform.rotation = Quaternion.Euler(90f, 0f, transform.rotation.eulerAngles.z);
    //        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, 0f);
    //    }
    //}
    private Transform _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = this.transform.position;
        var camPos = _camera.position;

        pos = LockXZaxis(pos);
        camPos = LockXZaxis(camPos);

        this.transform.rotation = Quaternion.LookRotation(camPos - pos);
    }

    private Vector3 LockXZaxis(Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }
}