using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyScript : MonoBehaviour, IDamagable
{
    private Animator anim;
    private bool canAttack = true;
    private Vector2 directionToPlayer;
    private GameObject player;
    public int health;
    private bool dying = false;
    private NavMeshAgent agent;
    private WinCondition deathChecker;
    public PlayerBullet bullet;
    private SoundManager sm;
    private bool shooting = false;
    private bool recoiling = false;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    private bool inRange = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        deathChecker = GameObject.Find("Main Camera").GetComponent<WinCondition>();
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        directionToPlayer = player.transform.position - transform.position;
        float angle = Vector2.SignedAngle(transform.right, directionToPlayer);
        if (dying == false) {
            if (angle < 90f && angle > -90f)
            {
                transform.localScale = new Vector3(5, 5, 1);
            } else {
                transform.localScale = new Vector3(-5, 5, 1);
            }
        }
        if(inRange == true && canAttack == true && dying == false)
        {
            canAttack = false;
            GetComponent<Enemy>().speed = 0;
            StartCoroutine(Attack());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && canAttack == true && dying == false)
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            inRange = true;
        }
    }
    public IEnumerator Attack() {
        anim.SetBool("Attack", true);
        StartCoroutine(Shoot());
        yield return new WaitForSeconds(3.5f);   
        anim.SetBool("Attack", false);
        GetComponent<Enemy>().speed = 1;
        yield return new WaitForSeconds(2f);
        canAttack = true; 
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0 && dying == false) {
            StopAllCoroutines();
            StartCoroutine(Die());
        } else if (dying == false && recoiling == false) {
            StartCoroutine(Hit());
        }
    }

    public IEnumerator Hit() {
        recoiling = true;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(.2f);
        recoiling = false;
    }

    public IEnumerator Die() {
        dying = true; 
        if (shooting) {
            sm.FlyBotLazerSource.Stop();
        }
        GetComponent<Enemy>().speed = 0;
        GetComponent<CircleCollider2D>().enabled = false;
        anim.SetTrigger("Die");
        deathChecker.EnemyDied();
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
                break;
            }
        }
        yield return new WaitForSeconds(.4f);
        sm.FlyDieSource.Play();
        yield return new WaitForSeconds(.6f);
        Destroy(this.gameObject);
    }
    public IEnumerator Shoot() {
        shooting = true;
        sm.FlyBotLazerSource.Play();
        yield return new WaitForSeconds(1f);    
        PlayerBullet newBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet.setTarget("Player", new Vector3(0, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet1 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet1.setTarget("Player", new Vector3(1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet2 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet2.setTarget("Player", new Vector3(-1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet3 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet3.setTarget("Player", new Vector3(0, -1, 0), 9f, "Enemy");
        yield return new WaitForSeconds(1f); 
        PlayerBullet newBullet4 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet4.setTarget("Player", new Vector3(1, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet5 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet5.setTarget("Player", new Vector3(1, -1, 0), 9f, "Enemy");
        PlayerBullet newBullet6 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet6.setTarget("Player", new Vector3(-1, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet7 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet7.setTarget("Player", new Vector3(-1, -1, 0), 9f, "Enemy");
        yield return new WaitForSeconds(1f); 
        PlayerBullet newBullet8 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet8.setTarget("Player", new Vector3(0, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet9 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet9.setTarget("Player", new Vector3(1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet10 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet10.setTarget("Player", new Vector3(-1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet11 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet11.setTarget("Player", new Vector3(0, -1, 0), 9f, "Enemy");
        yield return new WaitForSeconds(1f);
        shooting = false;
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            GameObject droppedLoot = Instantiate(loot, position, Quaternion.identity);
        }
    }
}
