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
        setGun(0); //assault rifle is default weapon, when no card is active
    }

    public void setGun(int index)
    {
        weaponIndex = index;
        weaponSprite.sprite = weapons[weaponIndex];
        damage = damages[weaponIndex];
    }

    public int getDamage() {
        return damage;
    }
}
