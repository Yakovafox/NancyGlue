using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraSwapping : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;

    // Start is called before the first frame update
    void Start()
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Cam1.activeSelf)
            {
                Cam1.SetActive(false);
                Cam2.SetActive(true);
            }
            else
            {
                Cam1.SetActive(true);
                Cam2.SetActive(false);
            }
        }
    }
}
