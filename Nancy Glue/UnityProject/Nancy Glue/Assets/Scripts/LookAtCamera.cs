using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool _lockXRot, _lockYRot, _lockZRot;
    [SerializeField] private float _xRot, _yRot, _zRot;

    private void FixedUpdate()
    {
        _xRot = _lockXRot ? 0 : Camera.main.transform.rotation.eulerAngles.x;
        _yRot = _lockYRot ? 0 : Camera.main.transform.rotation.eulerAngles.y;
        _zRot = _lockZRot ? 0 : Camera.main.transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(_xRot,_yRot,_zRot);
    }
}
