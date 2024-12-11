using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurretBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float projSpeed;
    public float turnSpeed;
    public float lifeTime;
    private GameObject target;
    private PlayerMovement playerHealth;
    private Vector2 directionToPlayer;
    private float angleToPlayer;
    private Animator anim;
    public float radius;
    public float power;
    private SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        playerHealth = target.GetComponent<PlayerMovement>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        directionToPlayer = target.transform.position - transform.position;
        angleToPlayer = Vector2.SignedAngle(-transform.right, directionToPlayer);
        if (angleToPlayer < -1) 
        {
            transform.Rotate(0, 0, turnSpeed*Time.deltaTime);
        } else if (angleToPlayer > 1){
            transform.Rotate(0, 0, -turnSpeed*Time.deltaTime);
        }
        rb.transform.Translate(Vector2.right * Time.deltaTime * projSpeed);
        

    }
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        projSpeed = 0;
        anim.SetTrigger("Explode");
        sm.TurretExplodeSource.Play();
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && rb.gameObject.tag == "Player")
            {
                var dir = (rb.transform.position - transform.position);
                float explosionForce = power / dir.magnitude;
                if (explosionForce > 0)
                {
                    float wearoff = 1 - (dir.magnitude / radius);
                    dir.Normalize();
                    RigidBody2DExtension.AddExplosionForce(rb, explosionForce * 100, explosionPos, radius);
                }
            }

        }
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Explode());
            playerHealth.TakeDamage(10);
        }
        if(other.tag == "Wall")
        {
            StartCoroutine(Explode());
        }
    }
    
}
