using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]private float LifeTime;
    private string target;
    protected Rigidbody2D rb;
    protected float speed;
    void Start(){}
    public void setTarget(string name,Vector3 dir,float force)
    {
        target = name;
        rb = GetComponent<Rigidbody2D>();
        speed=force;
        transform.up=dir;
        StartCoroutine(startCountdown());
    }
    public IEnumerator startCountdown()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
    void FixedUpdate()
    {
        rb.velocity = transform.up*speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == target)
        {
            Destroy(this.gameObject);
        }
    }

}
