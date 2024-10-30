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
    private Vector2 directionToPlayer;
    private float angleToPlayer;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player");
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
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
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
