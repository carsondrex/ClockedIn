using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenButtons : MonoBehaviour
{
    public GameObject OptionsWindow;
    private void Start()
    {
        OptionsWindow = GameObject.Find("OptionsScreen");
        OptionsWindow.SetActive(false);
    }

    public void Options()
    {
        OptionsWindow.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        OptionsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
