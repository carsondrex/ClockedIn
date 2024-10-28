using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey.Utils;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Animator anim;
    private SpriteRenderer weaponSprite;
    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        anim = GetComponent<Animator>();
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        //gun angle modifications
        //gun is on right of player
        if (angle < 90f && angle > -90f)
        {
            aimTransform.localScale = new Vector3(transform.localScale.x * -1f, 1, 1);
            //look right
            if (angle < 45f && angle > -60f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                weaponSprite.sortingOrder = 3;
                anim.SetBool("down", false);
                anim.SetBool("up", false);
            }
            //look up
            else if (angle > 45f)
            {
                anim.SetBool("down", false);
                anim.SetBool("up", true);
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle < -60f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("up", false);
                anim.SetBool("down", true);
            }
        }
        //gun is on left of player
        else
        {
            aimTransform.localScale = new Vector3(transform.localScale.x * -1f, -1, 1);
            //look left
            if ((angle > 150f && angle < 180f) || (angle > -180f && angle < -150f))
            {
                weaponSprite.sortingOrder = 3;
                transform.localScale = new Vector3(1, 1, 1);
                anim.SetBool("down", false);
                anim.SetBool("up", false);
            }
            //look up
            else if (angle < 150f && angle > 90f)
            {
                anim.SetBool("down", false);
                anim.SetBool("up", true);
                weaponSprite.sortingOrder = 1;
            }
            //look down
            else if (angle > -150f && angle < -90f)
            {
                weaponSprite.sortingOrder = 3;
                anim.SetBool("up", false);
                anim.SetBool("down", true);
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
