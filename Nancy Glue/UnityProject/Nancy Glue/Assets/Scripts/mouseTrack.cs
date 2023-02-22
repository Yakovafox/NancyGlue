using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to check mouse over any 3d game object collider 
//need to add 

public class mouseTrack : MonoBehaviour
{
    Vector3 worldPos;
    public Inventory inv;

    private void Start()
    {
        inv = GameObject.Find("Player").GetComponent<Inventory>();
    }


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
                        inv.GiveItem(0);
                        break;
                    case ("NPC"):
                        Debug.Log("Clicked NPC");
                        break;
                    case ("Interactable"):
                        Debug.Log("Clicked Interactable");
                        break;
                    case ("Untagged"):
                        Debug.Log("Untagged - ignored");
                        break;
                }
            }
        }
    }
}