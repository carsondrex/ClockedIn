using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Animator anim;
    private SpriteRenderer weaponSprite;
    public Animator weaponAnim;
    public ParticleSystem gunFlash;
    private bool canShoot;
    private PlayerBullet bullet;
    private GunManager gm;
    private SpriteRenderer playerSprite;
    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        anim = GetComponent<Animator>();
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
        weaponAnim = aimTransform.Find("Weapon").GetComponent<Animator>();
        gm = transform.GetComponent<GunManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        HandleAiming(angle);
        HandleShooting(aimDirection);
    }

    private void HandleAiming(float angle)
    {
        //gun angle modifications
        //gun is on right of player
        if (angle < 90f && angle > -90f)
        {
            playerSprite.flipX = false;
            weaponSprite.flipX = false;
            weaponSprite.flipY = true;
            //look right
            if (angle < 45f && angle > -60f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("leftRight", true);
                anim.SetBool("down", false);
                anim.SetBool("up", false);
                playerSprite.flipX = false;
            }
            //look up
            else if (angle > 45f)
            {
                anim.SetBool("up", true);
                anim.SetBool("leftRight", false);
                //anim.SetBool("down", false);
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle < -60f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("down", true);
                anim.SetBool("leftRight", false);
                //anim.SetBool("up", false);
            }
        }
        //gun is on left of player
        else
        {
            playerSprite.flipX = true;
            weaponSprite.flipX = false;
            weaponSprite.flipY = false;
            //look left
            if ((angle > 150f && angle < 180f) || (angle > -180f && angle < -150f))
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("leftRight", true);
                anim.SetBool("down", false);
                anim.SetBool("up", false);
            }
            //look up
            else if (angle < 150f && angle > 90f)
            {
                anim.SetBool("up", true);
                anim.SetBool("leftRight", false);
                //anim.SetBool("down", false);
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle > -150f && angle < -90f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("down", true);
                anim.SetBool("leftRight", false);
                //anim.SetBool("up", false);
            }
        }
    }

    private void HandleShooting(Vector3 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot(aimDirection));
            }
            weaponAnim.SetBool("Shoot", true);
        }
        else
        {
            weaponAnim.SetBool("Shoot", false);
        }
    }

    protected IEnumerator Shoot(Vector3 shootDirection)
    {
        //bullet type decider depending on active gun
        if (gm.weaponIndex == 0) //Assault rifle
            bullet = gm.bullets[0];//undecided
        else if (gm.weaponIndex == 1) //flamer
            bullet = gm.bullets[1];//have to add flame bullet
        else if (gm.weaponIndex == 2) //Gun 4
            bullet = gm.bullets[5]; //waveform
        else if (gm.weaponIndex == 3) //Gun 5
            bullet = gm.bullets[2];//undecided
        else if (gm.weaponIndex == 4) //L-Coil
            bullet = gm.bullets[4];//spark
        else if (gm.weaponIndex == 5) //L-Coil
            bullet = gm.bullets[3];//spark
        else
            Debug.Log("Unrecognized gun");

        CinemachineShake.Instance.ShakeCamera(4f, .3f); //could potentially change these values depending on the bullet or even make bosses shake the screen
        PlayerBullet newBullet = Instantiate(bullet, aimTransform.position, Quaternion.identity);
        newBullet.setTarget("Enemy", shootDirection, 18);
        gunFlash.Play();
        canShoot = false;
        yield return new WaitForSeconds(0.4f);
        canShoot = true;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
