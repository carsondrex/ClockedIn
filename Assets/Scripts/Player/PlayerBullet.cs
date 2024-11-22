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
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
        gunManager = GameObject.Find("Player").GetComponent<GunManager>();
        damage = gunManager.getDamage();
    }
    public void setTarget(string name, Vector3 dir, float force)
    {
        target = name;
        rb = GetComponent<Rigidbody2D>();
        speed = force;
        transform.up = dir;
        StartCoroutine(startCountdown());
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
            bulletSprite.enabled = false;
            Vector2 collisionPoint = other.ClosestPoint(transform.position);
            GameObject thisImpact = Instantiate(impact, new Vector3(collisionPoint.x, collisionPoint.y, 0), Quaternion.identity);
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            StartCoroutine(SelfDestruct(this.gameObject, thisImpact));
        }
    }

    IEnumerator SelfDestruct(GameObject thisBullet, GameObject thisImpact)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(thisImpact);
        Destroy(this.gameObject);
    }
}
