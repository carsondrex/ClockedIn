using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class BIGBALL : MonoBehaviour, IDamagable
{
    private Animator anim;
    private bool canAttack = true;
    private bool willTakeDamage = false;
    private PlayerMovement playerHealth;
    private Vector2 directionToPlayer;
    private float angleToPlayer;
    private GameObject player;
    private bool dying = false;
    private NavMeshAgent agent;
    private WinCondition deathChecker;
    public PlayerBullet bullet;
    public float health = 100f;
    //health bar
    private float fillSpeed = 0.3f;
    private Slider healthBar;
    private SoundManager sm;

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
        healthBar = GameObject.Find("Boss Bar").GetComponent<Slider>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        directionToPlayer = player.transform.position - transform.position;
        float angle = Vector2.SignedAngle(transform.right, directionToPlayer);
        if (angle < 90f && angle > -90f)
        {
            transform.localScale = new Vector3(5, 5, 1);
        } else {
            transform.localScale = new Vector3(-5, 5, 1);
        }
        if (agent.velocity != new Vector3(0, 0, 0)) {
            anim.SetBool("Moving", true);
        } else {
            anim.SetBool("Moving", false);
        }
        if (dying == false) {
            GetComponent<Enemy>().speed += .1f * Time.deltaTime;
        }
        if ((GetComponent<Enemy>().state != "Idle") && (dying == false)) {
            GameObject.Find("Boss Bar").GetComponent<CanvasGroup>().alpha = 1f;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && canAttack == true && dying == false)
        {
            canAttack = false;
            willTakeDamage = true;
            StartCoroutine(Attack());
            StartCoroutine(Hurt());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            willTakeDamage = false;
        }
    }

    public IEnumerator Attack() {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        if (dying == false) {
            GetComponent<Enemy>().speed = 3f;
        }
        canAttack = true;
    }

    public IEnumerator Hurt() {
        yield return new WaitForSeconds(.8f);
        sm.BigBallSlamSource.Play();
        PlayerBullet newBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet.setTarget("Player", new Vector3(0, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet1 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet1.setTarget("Player", new Vector3(1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet2 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet2.setTarget("Player", new Vector3(-1, 0, 0), 9f, "Enemy");
        PlayerBullet newBullet3 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet3.setTarget("Player", new Vector3(0, -1, 0), 9f, "Enemy");
        PlayerBullet newBullet4 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet4.setTarget("Player", new Vector3(1, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet5 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet5.setTarget("Player", new Vector3(1, -1, 0), 9f, "Enemy");
        PlayerBullet newBullet6 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet6.setTarget("Player", new Vector3(-1, 1, 0), 9f, "Enemy");
        PlayerBullet newBullet7 = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), Quaternion.identity);
        newBullet7.setTarget("Player", new Vector3(-1, -1, 0), 9f, "Enemy");
        if (willTakeDamage == true) {
            playerHealth.TakeDamage(15);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        float targetFillAmount = health / 300;
        healthBar.DOValue(targetFillAmount, fillSpeed);
        if (health <= 0 && dying == false) {
            StartCoroutine(Die());
        } else if (dying == false) {
            anim.SetTrigger("Hit");
        }
    }

    public IEnumerator Die() {
        dying = true; 
        GetComponent<Enemy>().speed = 0;
        GetComponent<CircleCollider2D>().enabled = false;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.6f);
        for (int i = 0; i < 10; i++) 
        {
            yield return new WaitForSeconds(.1f);
            foreach (LootItem lootItem in lootTable) 
            {
                if (Random.Range(0f, 100f) <= lootItem.dropChance)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                    break;
                }
            }
        }
        deathChecker.EnemyDied();
        Destroy(this.gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Vector2 position = new Vector2(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f));
            GameObject droppedLoot = Instantiate(loot, position, Quaternion.identity);
        }
    }
}
