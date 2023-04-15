using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
