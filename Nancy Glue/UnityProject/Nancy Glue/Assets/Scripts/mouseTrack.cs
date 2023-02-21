using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to check mouse over any 3d game object collider 

public class mouseTrack : MonoBehaviour
{
    Vector3 worldPos;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Vector3 start = new Vector3(0, 1, -10);
        //Vector3 dir = new Vector3(0, 0, 1);
        //Debug.DrawRay(start, dir, Color.blue);
        RaycastHit hitData;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                worldPos = hitData.point;
                Debug.Log("object hit");
            }
        }
    }
}