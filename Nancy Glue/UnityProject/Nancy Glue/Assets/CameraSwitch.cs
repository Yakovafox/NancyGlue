using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private bool _activeCam;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private MeshRenderer _myMeshRenderer;
    private void Awake()
    {
        _myMeshRenderer = GetComponent<MeshRenderer>();
        _cameraTransform = transform.GetChild(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        EnableDisableCam();
    }

    public void SwitchActiveCam()
    {
        _activeCam = !_activeCam;
        EnableDisableCam();
    }

    private void EnableDisableCam()
    {
        _cameraTransform.gameObject.SetActive(_activeCam);
        _myMeshRenderer.enabled = !_activeCam;
    }
}
