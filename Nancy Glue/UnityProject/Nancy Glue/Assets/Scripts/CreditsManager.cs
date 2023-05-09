using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private string Menu_Scene;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(Menu_Scene);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(Menu_Scene);
    }
}
