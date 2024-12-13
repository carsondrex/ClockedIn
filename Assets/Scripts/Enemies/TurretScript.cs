using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour, IDamagable
{
    public TurretBulletScript bullet;
    public Animator anim;
    private bool canShoot = true;
    public int health;
    private int isFlipped;
    private WinCondition deathChecker;
    public string state = "Idle";
    private SoundManager sm;

    //Loot table
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale.x > 0) {
            isFlipped = 0;
        } else {
            isFlipped = 1;
        }
        deathChecker = GameObject.Find("Main Camera").GetComponent<WinCondition>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(2);
        anim.SetTrigger("Shoot");
        sm.TurretShotSource.Play();
        yield return new WaitForSeconds(.1f);
        TurretBulletScript newBullet = Instantiate(bullet, transform.position-(new Vector3(0, .75f, 0)), Quaternion.Euler(new Vector3(0, 0, 180*isFlipped)));
        yield return new WaitForSeconds(2);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot && state != "Idle")
        {
            StartCoroutine(Shoot());
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            StartCoroutine(Die());
        } else {
            anim.SetTrigger("Hit");
        }
    }

    public IEnumerator Die() {
        anim.SetTrigger("Die");
        deathChecker.EnemyDied();
        foreach(LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
                break;
            }
        }
        yield return new WaitForSeconds(1.5f);
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y - 1.3f);
            GameObject droppedLoot = Instantiate(loot, position, Quaternion.identity);
        }
    }
    
}
