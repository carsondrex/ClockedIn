using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BallScript : MonoBehaviour, IDamagable
{
    private Animator anim;
    private bool canAttack = true;
    private bool willTakeDamage = false;
    private PlayerMovement playerHealth;
    private Vector2 directionToPlayer;
    private float angleToPlayer;
    private GameObject player;
    public int health;
    private bool dying = false;
    private NavMeshAgent agent;
    private WinCondition deathChecker;
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
        canAttack = true;
    }

    public IEnumerator Hurt() {
        yield return new WaitForSeconds(.5f);
        sm.BallSlamSource.Play();
        if (willTakeDamage == true) {
            playerHealth.TakeDamage(10);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
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
