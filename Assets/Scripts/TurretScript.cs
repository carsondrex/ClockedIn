using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public TurretBulletScript bullet;
    public Animator anim;
    private bool canShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator Shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(2);
        Debug.Log("Triggering Shoot");
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(.1f);
        TurretBulletScript newBullet = Instantiate(bullet, transform.position-(new Vector3(0, .75f, 0)), new Quaternion(0, 0, 0, 0));
        yield return new WaitForSeconds(2);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            Debug.Log("Calling Shoot");
            StartCoroutine(Shoot());
        }
    }

    
}
