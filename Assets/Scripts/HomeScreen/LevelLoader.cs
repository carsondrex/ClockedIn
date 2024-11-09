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

    public IEnumerator LoadLevel(int levelIndex)
    {
        if (levelIndex != 0)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(levelIndex);
        }
    }
}
