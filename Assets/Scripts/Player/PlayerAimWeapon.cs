using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Animator anim;
    private SpriteRenderer weaponSprite;
    public Animator weaponAnim;
    public ParticleSystem gunFlash;
    public ParticleSystem flamerParticles;
    public bool canShoot;
    private PlayerBullet bullet;
    private GunManager gm;
    private SpriteRenderer playerSprite;
    private float fillSpeed = 0.3f;
    private Slider ammoBar;
    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        anim = GetComponent<Animator>();
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
        weaponAnim = aimTransform.Find("Weapon").GetComponent<Animator>();
        gm = transform.GetComponent<GunManager>();
        playerSprite = GetComponent<SpriteRenderer>();
        flamerParticles.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        canShoot = true;
        ammoBar = GameObject.Find("Ammo Bar").GetComponent<Slider>();
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
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle < -60f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("down", true);
                anim.SetBool("leftRight", false);
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
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle > -150f && angle < -90f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("down", true);
                anim.SetBool("leftRight", false);
            }
        }
    }

    private void HandleShooting(Vector3 aimDirection)
    {
        //var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            if (canShoot && gm.ammo >= gm.getShotIncrement()) //&& mouseWorldPos.x < 9f
            {
                StartCoroutine(Shoot(aimDirection));
                weaponAnim.SetBool("Shoot", true);
            }
        }
        else
        {
            weaponAnim.SetBool("Shoot", false);
        }
    }

    protected IEnumerator Shoot(Vector3 shootDirection)
    {
        int bulletIndex = 0;
        //bullet type decider depending on active gun
        if (gm.weaponIndex == 0) //L-Coil
            bulletIndex = 4;
        else if (gm.weaponIndex == 1) //Shotgun
            bulletIndex = 1;
        else if (gm.weaponIndex == 2) //Gattling Gun / Gun 5
            bulletIndex = 2;
        //else if (gm.weaponIndex == 3) //flamer has no bullet type
        else if (gm.weaponIndex == 4) //Assault rifle
            bulletIndex = 0;

        CinemachineShake.Instance.ShakeCamera(.4f, .03f); //could potentially change these values depending on the bullet or even make bosses shake the screen
        if (gm.weaponIndex == 3) //flamer
        {
            flamerParticles.Play();
            flamerParticles.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
        else if (gm.weaponIndex == 2)
        {
            bullet = gm.bullets[bulletIndex];
            StartCoroutine(GattlingGunShoot(shootDirection));
        }
        else
        {
            bullet = gm.bullets[bulletIndex];
            PlayerBullet newBullet = Instantiate(bullet, aimTransform.position, Quaternion.identity);
            newBullet.setTarget("Enemy", shootDirection, 18);
        }

        gunFlash.Play();
        canShoot = false;
        gm.ammo = gm.ammo - gm.getShotIncrement();
        float targetFillAmount = gm.ammo;
        ammoBar.DOValue(targetFillAmount, fillSpeed);
        if (gm.ammo <= 0)
        {
            if (gm.weaponIndex != 4)
            {
                gm.breakGun();
            }
            else
            {
                gm.reload();
            }
        }

        yield return new WaitForSeconds(0.4f);
        flamerParticles.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        canShoot = true;
    }

    private IEnumerator GattlingGunShoot(Vector3 shootDirection)
    {
        for (int i = 0; i < 3; i++)
        {
            PlayerBullet newBullet = Instantiate(bullet, aimTransform.position, Quaternion.identity);
            newBullet.setTarget("Enemy", shootDirection, 20);
            if (i != 2)
            {
                yield return new WaitForSeconds(0.07f);
            }
        }
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
