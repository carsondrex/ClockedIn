using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerBullet : MonoBehaviour
{
    private GunManager gunManager;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        gunManager = GameObject.Find("Player").GetComponent<GunManager>();
        damage = gunManager.getDamage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            damage = gunManager.getDamage();
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
}
