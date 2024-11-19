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
    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale.x > 0) {
            isFlipped = 0;
        } else {
            isFlipped = 1;
        }
        deathChecker = GameObject.Find("Main Camera").GetComponent<WinCondition>();
    }

    public IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(2);
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(.1f);
        TurretBulletScript newBullet = Instantiate(bullet, transform.position-(new Vector3(0, .75f, 0)), Quaternion.Euler(new Vector3(0, 0, 180*isFlipped)));
        yield return new WaitForSeconds(2);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
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
        yield return new WaitForSeconds(1.5f);
        deathChecker.EnemyDied();
        Destroy(this.gameObject);
    }
    
}
