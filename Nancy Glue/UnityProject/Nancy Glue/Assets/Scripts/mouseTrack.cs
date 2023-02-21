using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to check mouse over any 3d game object collider 
//need to add 

public class mouseTrack : MonoBehaviour
{
    Vector3 worldPos;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        //Vector3 start = new Vector3(0, 1, -10);
        //Vector3 dir = new Vector3(0, 0, 1);
        //Debug.DrawRay(start, dir, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag)
                {
                    case ("Evidence"):
                        Debug.Log("Clicked evidence");
                        break;
                    case ("NPC"):
                        Debug.Log("Clicked NPC");
                        break;
                    case ("Interactable"):
                        Debug.Log("Clicked Interactable");
                        break;
                    case ("Untagged"):
                        Debug.Log("Other object hit");
                        break;
                }
            }
        }
    }
}