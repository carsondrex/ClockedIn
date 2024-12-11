using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GunManager : MonoBehaviour
{
    private Transform aimTransform;
    private SpriteRenderer weaponSprite;
    public Sprite[] weapons;
    public int weaponIndex;
    public int[] damages;
    public int currentDamage;
    public PlayerBullet[] bullets;
    public int[] shotIncrements;
    public int currentShotIncrement;
    public int ammo;
    private CardManager cm;
    private Slider ammoBar;
    private float fillSpeed = 0.3f;
    private PlayerAimWeapon paw;
    private SoundManager sm;
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        weaponSprite = aimTransform.Find("Weapon").GetComponent<SpriteRenderer>();
        cm = GameObject.Find("Cards").GetComponent<CardManager>();
        ammo = 8;
        ammoBar = GameObject.Find("Ammo Bar").GetComponent<Slider>();
        paw = GetComponent<PlayerAimWeapon>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (cm.noCardsLeft() || cm.getCurrentCard() == "default")
        {
            setGun(4); //default gun
        }
        else if (cm.getCurrentCard() == "l-coil" && cm.getCardCounts()[0] > 0)
        {
            setGun(0);
        }
        else if (cm.getCurrentCard() == "shotgun" && cm.getCardCounts()[1] > 0)
        {
            setGun(1);
        }
        else if (cm.getCurrentCard() == "gattlinggun" && cm.getCardCounts()[2] > 0)
        {
            setGun(2);
        }
        else if (cm.getCurrentCard() == "flamer" && cm.getCardCounts()[3] > 0)
        {
            setGun(3);
        }
    }

    public void reload()
    {
        StartCoroutine(ReloadGun());
    }

    private IEnumerator ReloadGun()
    {
        paw.canShoot = false;
        yield return new WaitForSeconds(1);
        sm.reloadSource.Play();
        yield return new WaitForSeconds(1);
        refillAmmo();
        paw.canShoot = true;
    }

    public void refillAmmo()
    {
        if (weaponIndex != 4)
        {
            if (cm.getCardCounts()[weaponIndex] > 0)
            {
                sm.refillAmmoSource.Play();
            }
        }
        ammo = 8;
        ammoBar.DOValue(8, fillSpeed);
    }

    public void breakGun()
    {
        cm.changeCardCount(weaponIndex, -1);
        if (cm.getCardCounts()[weaponIndex] == 0)
        {
            sm.breakGunSource.Play();
            cm.setCurrentCard("default");
            setGun(4);
        }
        refillAmmo();
    }

    public void setGun(int index)
    {
        weaponIndex = index;
        weaponSprite.sprite = weapons[weaponIndex];
        currentDamage = damages[weaponIndex];
        currentShotIncrement = shotIncrements[weaponIndex];
        if (index == 0)
        {
            cm.setCurrentCard("l-coil");
        }
        else if (index == 1)
        {
            cm.setCurrentCard("shotgun");
        }
        else if (index == 2)
        {
            cm.setCurrentCard("gattlinggun");
        }
        else if (index == 3)
        {
            cm.setCurrentCard("flamer");
        }
        else if (index == 4)
        {
            cm.setCurrentCard("default");
        }
    }

    public int getDamage() {
        return currentDamage;
    }

    public int getShotIncrement()
    {
        return currentShotIncrement;
    }
}
