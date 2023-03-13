using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool _isRoot;
    [SerializeField] private Transform _rootCamera; //transform for the root camera.
    [SerializeField] private Transform _cameraTransform; //transform for the current camera.
    [SerializeField] private List<Transform> _switchableCameras; //transforms for cameras that can be switched to.
    [SerializeField] [Range(0,45)]private float _yRotLimit;
    [SerializeField] [Range(0,45)]private float _xRotLimit;
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private Vector2 _screenBounds;
    [SerializeField][Range(0, 100)] private int _screenBuffer;
    [SerializeField] private bool _lookLeft, _lookRight, _lookUp, _lookDown;
    [SerializeField] private Vector3 _initialRotation;
    [SerializeField] private float _cameraXMin, _cameraXMax, _parentYMin, _parentYMax;
    [SerializeField] private Transform _parent;
    [SerializeField] private Vector3 _parentRotation;
    [SerializeField] private Transform _hitBoxTransform;
    [SerializeField] private float _angleX, _angleY;

    private void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
        _parent = _cameraTransform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        _screenBounds = new Vector2(Screen.width - _screenBuffer, Screen.height - _screenBuffer);
        _isRoot = _rootCamera == null;
        _parentRotation = _parent.eulerAngles;
        _initialRotation = new Vector3(_cameraTransform.rotation.eulerAngles.x, _parent.rotation.eulerAngles.y, 0);
        _cameraXMin = _initialRotation.x - _xRotLimit;
        _cameraXMax = _initialRotation.x + _xRotLimit;
        _parentYMin = _initialRotation.y - _yRotLimit;
        _parentYMax = _initialRotation.y + _yRotLimit;
        _angleX = _initialRotation.x;
        _angleY = _initialRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        _mousePos = Input.mousePosition;
        _lookLeft = _mousePos.x < _screenBuffer ? true : false;
        _lookRight = _mousePos.x > _screenBounds.x ? true : false;
        _lookUp = _mousePos.y < _screenBuffer ? true : false;
        _lookDown = _mousePos.y > _screenBounds.y ? true : false;
        RotateLeftRight();
        RotateUpDown();
    }

    private void RotateLeftRight()
    {
        if (!_lookLeft && !_lookRight) return;
        switch(_lookLeft)
        {
            case true:
                Rotate(0,-1);
                break;
            case false when _lookRight:
                Rotate(0, 1);
                break;
            case false:
                break;
        }
    }

    private void RotateUpDown()
    {
        if (!_lookUp && !_lookDown) return;
        switch (_lookUp)
        {
            case true:
                Rotate(1, 0);
                break;
            case false when _lookDown:
                Rotate(-1, 0);
                break;
            case false:
                break;
        }
    }

    private void Rotate(int x, int y)
    {
        if (x != 0)
        {
            _angleX += (x * 10f) * Time.deltaTime;
            _angleX = Mathf.Clamp(_angleX, _cameraXMin, _cameraXMax);
        }
        if (y != 0)
        { 
            _angleY += (y * 10f) * Time.deltaTime;
            _angleY = Mathf.Clamp(_angleY, _parentYMin, _parentYMax);
        }
        _parent.eulerAngles = new Vector3(0, _angleY, 0);
        _cameraTransform.eulerAngles = new Vector3(_angleX, _angleY, 0); //don't know why this needs angleX and angleY, but camera will not rotate left or right without angleY.
    }

    private float AngleClamp(float angle, float AxisRotationMin, float AxisRotationMax, int Direction)
    {
        angle += Direction * Time.deltaTime;
        angle = Mathf.Clamp(angle, AxisRotationMin, AxisRotationMax);
        return angle;
    }
}