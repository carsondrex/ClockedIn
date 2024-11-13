using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private Transform aimTransform;
    private SpriteRenderer weaponSprite;
    public Sprite[] weapons;
    public int[] damages;
    public int damage;
    public PlayerBullet[] bullets; //roundabout way to add the bullets to the static bullets[]
    public int weaponIndex;
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
        weaponIndex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //example of changing the weapon sprite:
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weaponIndex = 5;
        }
        weaponSprite.sprite = weapons[weaponIndex];
        damage = damages[weaponIndex];
    }

    public int getDamage() {
        return damage;
    }
}
