using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private PlayerMovement pm;

    public void NewRun()
    {
        StartCoroutine(LoadLevel(1)); //Load first level (Scene after this one in the build index)
    }

    public void LevelOne() {
        StartCoroutine(LoadLevel(2));
    }
    public void LevelTwo()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        pm.Heal(100);
        StartCoroutine(WaitBetweenLevels());
        StartCoroutine(LoadLevel(3));
    }
    public void LevelThree()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        pm.Heal(100);
        StartCoroutine(WaitBetweenLevels());
        StartCoroutine(LoadLevel(4));
    }

    public IEnumerator WaitBetweenLevels()
    {
        yield return new WaitForSeconds(2f);
    }

    public void MainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void GameOver()
    {
        StartCoroutine(LoadLevel(5)); //whatever the index is of the game over screen. will change.
    }

    public void Win() {
        StartCoroutine(LoadLevel(6));
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
