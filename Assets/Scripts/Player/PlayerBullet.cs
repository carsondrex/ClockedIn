using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float LifeTime;
    private string target;
    protected Rigidbody2D rb;
    protected float speed;
    public GameObject impact;
    private SpriteRenderer bulletSprite;
    private GunManager gunManager;
    private int damage;
    private SoundManager sm;
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
        gunManager = GameObject.Find("Player").GetComponent<GunManager>();
        damage = gunManager.getDamage();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    public void setTarget(string name, Vector3 dir, float force, string type = "Player")
    {
        target = name;
        rb = GetComponent<Rigidbody2D>();
        speed = force;
        transform.up = dir;
        StartCoroutine(startCountdown());
        if (type == "Enemy") {
            damage = 6;
        }
    }

    public IEnumerator startCountdown()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (target == "Enemy" && other.tag == "Enemy")
        {
            sm.hitEnemySource.Play();
            bulletSprite.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            Vector2 collisionPoint = this.transform.position;
            bulletSprite.enabled = false;
            GameObject thisImpact = Instantiate(impact, new Vector3(collisionPoint.x, collisionPoint.y, 0), Quaternion.identity);
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            StartCoroutine(SelfDestruct(thisImpact));
        }
        else if (other.tag == "Wall")
        {
            bulletSprite.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            Vector2 collisionPoint = this.transform.position;
            bulletSprite.enabled = false;
            GameObject thisImpact = Instantiate(impact, new Vector3(collisionPoint.x, collisionPoint.y, 0), Quaternion.identity);
            StartCoroutine(SelfDestruct(thisImpact));
        } 
        else if (target == "Player" && other.tag == "Player") 
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    IEnumerator SelfDestruct(GameObject thisImpact)
    {
        yield return new WaitForSeconds(0.1f);
        if (thisImpact)
        {
            Destroy(thisImpact.gameObject);
        }
        Destroy(this.gameObject);
    }
}
