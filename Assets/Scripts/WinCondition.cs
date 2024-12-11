using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private int numEnemies;
    private LevelLoader levload;
    private int level;
    // Start is called before the first frame update
    void Start()
    {
        levload = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        numEnemies = 0;
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            numEnemies += 1;
        }
        if (GameObject.Find("Level 1") != null)
        {
            level = 1;
        }
        else if (GameObject.Find("Level 2") != null)
        {
            level = 2;
        }
        else if (GameObject.Find("Level 3") != null)
        {
            level = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (level == 1 && numEnemies == 0)
        {
            Debug.Log("Here");
            levload.LevelTwo();
        }
        else if (level == 2 && numEnemies == 0)
        {
            levload.LevelThree();
        }
        else if (level == 3 && numEnemies == 0)
        {
            levload.Win();
        }
    }

    public void EnemyDied() {
        numEnemies -= 1;
    }
}
