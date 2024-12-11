using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BigScript : MonoBehaviour, IDamagable
{
    private Animator anim;
    private bool canAttack = true;
    private PlayerMovement playerHealth;
    private Vector2 directionToPlayer;
    private GameObject player;
    public int health;
    private bool dying = false;
    private NavMeshAgent agent;
    private WinCondition deathChecker;
    public PlayerBullet bullet;
    private float angle;
    private float shootSide;
    private SoundManager sm;
    public int size;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        deathChecker = GameObject.Find("Main Camera").GetComponent<WinCondition>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        directionToPlayer = player.transform.position - transform.position;
        angle = Vector2.SignedAngle(transform.right, directionToPlayer);
        if (angle < 90f && angle > -90f)
        {
            shootSide = 1;
            transform.localScale = new Vector3(size, size, 1);
        } else {
            shootSide = -1;
            transform.localScale = new Vector3(-size, size, 1);
        }
        if (agent.velocity != new Vector3(0, 0, 0)) {
            anim.SetBool("Moving", true);
        } else {
            anim.SetBool("Moving", false);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && canAttack == true && dying == false)
        {
            canAttack = false;
            GetComponent<Enemy>().speed = 0;
            StartCoroutine(Attack());
        }
    }

    public IEnumerator Attack() {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(.6f);
        sm.BigShootSource.Play();
        PlayerBullet newBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
        newBullet.setTarget("Player", directionToPlayer.normalized, 9f, "Enemy");
        PlayerBullet newBullet1 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
        newBullet1.setTarget("Player", Quaternion.AngleAxis(30, Vector3.back) * directionToPlayer.normalized, 9f, "Enemy");
        PlayerBullet newBullet2 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
        newBullet2.setTarget("Player", Quaternion.AngleAxis(-30, Vector3.back) * directionToPlayer.normalized, 9f, "Enemy");
        yield return new WaitForSeconds(.3f);
        sm.BigShootSource.Play();
        PlayerBullet newBullet3 = Instantiate(bullet, new Vector3(transform.position.x + shootSide, transform.position.y - .8f, transform.position.z), Quaternion.identity);
        newBullet3.setTarget("Player", Quaternion.AngleAxis(15, Vector3.back) * directionToPlayer.normalized, 9f, "Enemy");
        PlayerBullet newBullet4 = Instantiate(bullet, new Vector3(transform.position.x + shootSide, transform.position.y - .8f, transform.position.z), Quaternion.identity);
        newBullet4.setTarget("Player", Quaternion.AngleAxis(-15, Vector3.back) * directionToPlayer.normalized, 9f, "Enemy");
        yield return new WaitForSeconds(.6f);
        GetComponent<Enemy>().speed = 1;
        canAttack = true;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0 && dying == false) {
            StartCoroutine(Die());
        } else if (dying == false) {
            StartCoroutine(Hit());
        }
    }

    public IEnumerator Hit() {
        GetComponent<Enemy>().speed = 0;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(.5f);
        if (dying == false) {
            GetComponent<Enemy>().speed = 1;
        }
    }

    public IEnumerator Die() {
        dying = true; 
        GetComponent<Enemy>().speed = 0;
        GetComponent<CircleCollider2D>().enabled = false;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.6f);
        deathChecker.EnemyDied();
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
                break;
            }
        }
        Destroy(this.gameObject);
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
