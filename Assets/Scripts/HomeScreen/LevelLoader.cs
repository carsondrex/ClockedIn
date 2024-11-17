using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void NewRun()
    {
        StartCoroutine(LoadLevel(1)); //Load first level (Scene after this one in the build index)
    }

    public void MainMenu()
    {
        StartCoroutine(LoadLevel(0)); //0 is the index of the first scene in the build which should be main menu
    }

    public void GameOver()
    {
        StartCoroutine(LoadLevel(2)); //whatever the index is of the game over screen. will change.
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
