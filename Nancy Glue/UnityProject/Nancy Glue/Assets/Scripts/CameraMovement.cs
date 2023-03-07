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
    [SerializeField][Range(0, 50)] private int _screenBuffer;
    [SerializeField] private bool _lookLeft, _lookRight, _lookUp, _lookDown;
    [SerializeField] private Quaternion _initialRotation;
    [SerializeField] private Quaternion _maxRotation;
    [SerializeField] private Quaternion _minRotation;
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _hitBoxTransform;
  

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
        _yRotLimit = 45f;
        _xRotLimit = 45f;
        _initialRotation = _cameraTransform.rotation;
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
        RotateX();
        RotateY();
    }

    private void RotateY()
    {
        if (!_lookLeft && !_lookRight) return;
        switch(_lookLeft)
        {
            case true:
                Rotate(1 * -(int)_yRotLimit,0,0);
                break;
            case false when _lookRight:
                Rotate(1 * (int)_yRotLimit, 0, 0);
                break;
            case false:
                break;
        }
    }

    private void RotateX()
    {
        if (!_lookUp && !_lookDown) return;
        switch (_lookUp)
        {
            case true:
                Rotate(0, 1 * (int)_xRotLimit, 0);
                break;
            case false when _lookDown:
                Rotate(0, 1 * -(int)_xRotLimit, 0);
                break;
            case false:
                break;
        }
    }

    private void Rotate(int x, int y, int z)
    {
        _maxRotation = new Quaternion(_initialRotation.x + x, _initialRotation.y + y, 0, 0);
        _minRotation = new Quaternion(_initialRotation.x - x, _initialRotation.y - y, 0, 0);
        if (_cameraTransform.rotation != _maxRotation || _cameraTransform.rotation != _minRotation)
        {
            _cameraTransform.Rotate((Vector3.left * -y) * Time.deltaTime);
            _parent.Rotate((Vector3.up * x)* Time.deltaTime);
        }
    }
}