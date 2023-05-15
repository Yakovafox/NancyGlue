using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private string Menu_Scene;

    //if any key is pressed skip back to the main menu
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            MainMenu();
        }
    }

    public void MainMenu() //calls for a scene Manager change.
    {
        SceneManager.LoadScene(Menu_Scene);
    }
}
