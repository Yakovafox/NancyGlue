using System.Collections.Generic;
using Dialogue;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool _isRoot;
    [SerializeField] private Transform _rootCamera; //transform for the root camera.
    [SerializeField] private Transform _cameraTransform; //transform for the current camera.
    [SerializeField] public List<Transform> _switchableCameras; //transforms for cameras that can be switched to.
    [SerializeField] [Range(0,90)] private float _yRotLimit; //Limit rotation on the Y Axis
    [SerializeField] [Range(0,45)] private float _xRotLimit; //Limit Rotation on the X Axis
    [SerializeField] [Range(20,50)]private float _turnSpeed;
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private Vector2 _screenBounds;
    [SerializeField][Range(50, 100)] private int _screenBuffer = 100;
    [SerializeField] private bool _lookLeft, _lookRight, _lookUp, _lookDown;
    [SerializeField] private Vector3 _initialRotation;
    public Vector3 InitialRotation { get => _initialRotation; }
    [SerializeField] private float _cameraXMin, _cameraXMax, _cameraYMin, _cameraYMax;
    [SerializeField] private Transform _hitBoxTransform;
    [SerializeField] private float _angleX, _angleY;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _invUI;

    public float AngleX { get => _angleX; set => _angleX = value; }
    public float AngleY { get => _angleY; set => _angleY = value; }

    private void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
        _initialRotation = new Vector3(_cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y, 0);
        _dialogueBox = FindObjectOfType<DialogueSystem>().gameObject;
        _invUI = FindObjectOfType<invUI>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        _screenBounds = new Vector2(Screen.width - _screenBuffer, Screen.height - _screenBuffer);
        _isRoot = _rootCamera == null;
        _cameraXMin = _initialRotation.x - _xRotLimit;
        _cameraXMax = _initialRotation.x + _xRotLimit;
        _cameraYMin = _initialRotation.y - _yRotLimit;
        _cameraYMax = _initialRotation.y + _yRotLimit;
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
        if (LockMovement()) return;
        _mousePos = Input.mousePosition;
        _lookLeft = _mousePos.x < _screenBuffer || Input.GetKey(KeyCode.A) ? true : false;
        _lookRight = _mousePos.x > _screenBounds.x || Input.GetKey(KeyCode.D) ? true : false;
        _lookUp = _mousePos.y > _screenBounds.y || Input.GetKey(KeyCode.W) ? true : false;
        _lookDown =  _mousePos.y < _screenBuffer || Input.GetKey(KeyCode.S) ? true : false;
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
                Rotate(-1, 0);
                break;
            case false when _lookDown:
                Rotate(1, 0);
                break;
            case false:
                break;
        }
    }

    private void Rotate(int x, int y)
    {
        if (x != 0)
            _angleX = AngleClamp(x, _angleX, _cameraXMin, _cameraXMax);
        
        if (y != 0)
            _angleY = AngleClamp(y, _angleY, _cameraYMin, _cameraYMax);
        _cameraTransform.eulerAngles = new Vector3(_angleX, _angleY, 0);
    }

    private float AngleClamp(int axisDirection, float axisAngle, float clampMin, float clampMax)
    {
        axisAngle += (axisDirection * _turnSpeed) * Time.deltaTime;
        axisAngle = Mathf.Clamp(axisAngle, clampMin, clampMax);
        return axisAngle;
    }

    private bool LockMovement()
    {
        var uiOpen = _invUI.activeSelf || _dialogueBox.activeSelf;
        
        return uiOpen;
    }

    private void OnEnable()
    {
        _angleX = _initialRotation.x;
        _angleY = _initialRotation.y;
        _cameraTransform.eulerAngles = InitialRotation;
    }
}