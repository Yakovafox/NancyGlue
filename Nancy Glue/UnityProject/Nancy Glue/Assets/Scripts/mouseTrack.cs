using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class mouseTrack : MonoBehaviour
{
    Vector3 worldPos;
    public Inventory inv;
    public GameObject evid0;
    public GameObject evid1;
    private bool UIOpen;
    public GameObject canvas;
    //public 
    private void Start()
    {
        UIOpen = false;
        canvas.SetActive(false);



        inv = GameObject.Find("Player").GetComponent<Inventory>();
       // evid0 = GameObject.Find("Evidence0");
    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                worldPos = hitData.point;


                switch (hitData.transform.tag)
                {
                    case ("Evidence id0"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(0);
                        
                        //destroy object 
                        Destroy(evid0);
                        
                        break;
                    case ("Evidence id1"):
                        Debug.Log("Clicked evidence");
                        inv.GiveItem(1);

                        //destroy object 
                        Destroy(evid1);

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

        if (Input.GetKeyUp(KeyCode.I))
        {
            Debug.Log("Checking if inv open");
            if (UIOpen)
            {
                Debug.Log("inv open - closing inv");
                canvas.SetActive(false);
                UIOpen = false;
            }
            else
            {
                Debug.Log("inv closed - openeing inv");
                canvas.SetActive(true);
                UIOpen = true;
            }

        }




    }
}