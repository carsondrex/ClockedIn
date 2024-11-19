using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private int numEnemies;
    private LevelLoader levload;
    // Start is called before the first frame update
    void Start()
    {
        levload = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        numEnemies = 0;
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            numEnemies += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemies == 0) {
            levload.Win();
        }
    }

    public void EnemyDied() {
        numEnemies -= 1;
    }
}
