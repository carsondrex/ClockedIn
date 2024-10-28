using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private Transform aimTransform;
    private SpriteRenderer weaponSprite;
    public Sprite[] weapons;
    public int weaponIndex;
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
        weaponIndex = 5;
    }

    // Update is called once per frame
    void Update()
    {

        //example of changing the weapon sprite:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponIndex = 0;
            weaponSprite.sprite = weapons[weaponIndex];
        }
    }
}
