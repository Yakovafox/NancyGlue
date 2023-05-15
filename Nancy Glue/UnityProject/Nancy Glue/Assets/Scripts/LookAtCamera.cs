using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _lockXRot, _lockYRot, _lockZRot;
    [SerializeField] private float _xRot, _yRot, _zRot;
    [SerializeField] private float _yStart;

    //Stores the initial y rotation for the object
    private void Awake()
    {
        _yStart = transform.rotation.y;   
    }

    //Updates the sprite to look in the direction of the camera. Locking axis will set objects to either the camera rotation or 0 for X/Z and initial Y rotation.
    private void FixedUpdate()
    {
        _xRot = _lockXRot ? 0 : Camera.main.transform.rotation.eulerAngles.x;
        _yRot = _lockYRot ? _yStart : Camera.main.transform.rotation.eulerAngles.y;
        _zRot = _lockZRot ? 0 : Camera.main.transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(_xRot,_yRot,_zRot);
    }
}
