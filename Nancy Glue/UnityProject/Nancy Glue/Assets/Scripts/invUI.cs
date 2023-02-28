using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invUI : MonoBehaviour
{
    private bool open;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        open = false;
        canvas = GameObject.Find("UICanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            Debug.Log("Checking if inv open");
            if (open)
            {
                Debug.Log("inv open - closing inv");
                canvas.SetActive(false);
                open = false;
            }
            else
            {
                Debug.Log("inv closed - openeing inv");
                canvas.SetActive(true);
                open = true;
            }
            //if its open else close inv screeen

            //else open inventory




        }
    }
}
