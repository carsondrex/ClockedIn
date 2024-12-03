using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    private string currentCard;
    private int[] cardCounts = {0, 0, 0, 0};
    private TMP_Text lcoilCountText;
    private TMP_Text shotgunCountText;
    private TMP_Text gattlingGunCountText;
    private TMP_Text flamerCountText;
    private Animator lcoilCounterAnim;
    private Animator shotgunCounterAnim;
    private Animator gattlingCounterAnim;
    private Animator flamerCounterAnim;
    private Animator lcoilCardAnim;
    private Animator shotgunCardAnim;
    private Animator gattlingCardAnim;
    private Animator flamerCardAnim;
    private GunManager gm;
    private SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        currentCard = "default";
        lcoilCountText = GameObject.Find("LCoil Count Text").GetComponent<TextMeshProUGUI>();
        shotgunCountText = GameObject.Find("Shotgun Count Text").GetComponent<TextMeshProUGUI>();
        gattlingGunCountText = GameObject.Find("Gattling Gun Count Text").GetComponent<TextMeshProUGUI>();
        flamerCountText = GameObject.Find("Flamer Count Text").GetComponent<TextMeshProUGUI>();

        lcoilCounterAnim = GameObject.Find("LCoil Count Text").GetComponent<Animator>();
        shotgunCounterAnim = GameObject.Find("Shotgun Count Text").GetComponent<Animator>();
        gattlingCounterAnim = GameObject.Find("Gattling Gun Count Text").GetComponent<Animator>();
        flamerCounterAnim = GameObject.Find("Flamer Count Text").GetComponent<Animator>();

        lcoilCardAnim = GameObject.Find("LCoil Card").GetComponent<Animator>();
        shotgunCardAnim = GameObject.Find("Shotgun Card").GetComponent<Animator>();
        gattlingCardAnim = GameObject.Find("Gattling Gun Card").GetComponent<Animator>();
        flamerCardAnim = GameObject.Find("Flamer Card").GetComponent<Animator>();

        gm = GameObject.Find("Player").GetComponent<GunManager>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }


    public string getCurrentCard()
    {
        return currentCard;
    }

    public int[] getCardCounts()
    {
        return cardCounts;
    }

    public bool noCardsLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            if (cardCounts[i] > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void changeCardCount(int index, int changeAmount)
    {
        cardCounts[index] = cardCounts[index] + changeAmount;
        if (index == 0)
        {
            lcoilCountText.text = cardCounts[index].ToString();
            if (changeAmount > 0)
            {
                sm.cardSource.Play();
                lcoilCounterAnim.SetTrigger("increase");
                lcoilCardAnim.SetTrigger("flare");
            }
            else
            {
                lcoilCounterAnim.SetTrigger("decrease");
            }
        }
        else if (index == 1)
        {
            shotgunCountText.text = cardCounts[index].ToString();
            if (changeAmount > 0)
            {
                sm.cardSource.Play();
                shotgunCounterAnim.SetTrigger("increase");
                shotgunCardAnim.SetTrigger("flare");
            }
            else
            {
                shotgunCounterAnim.SetTrigger("decrease");
            }
        }
        else if (index == 2)
        {
            gattlingGunCountText.text = cardCounts[index].ToString();
            if (changeAmount > 0)
            {
                sm.cardSource.Play();
                gattlingCounterAnim.SetTrigger("increase");
                gattlingCardAnim.SetTrigger("flare");
            }
            else
            {
                gattlingCounterAnim.SetTrigger("decrease");
            }
        }
        else if (index == 3)
        {
            flamerCountText.text = cardCounts[index].ToString();
            if (changeAmount > 0)
            {
                sm.cardSource.Play();
                flamerCounterAnim.SetTrigger("increase");
                flamerCardAnim.SetTrigger("flare");
            }
            else
            {
                flamerCounterAnim.SetTrigger("decrease");
            }
        }
    }

    public void ClickLCoil()
    {
        if (cardCounts[0] > 0)
        {
            currentCard = "l-coil";
            gm.refillAmmo();
        }
    }
    public void ClickShotgun()
    {
        if (cardCounts[1] > 0)
        {
            currentCard = "shotgun";
            gm.refillAmmo();
        }
    }
    public void ClickGattlingGun()
    {
        if (cardCounts[2] > 0)
        {
            currentCard = "gattlinggun";
            gm.refillAmmo();
        }
    }
    public void ClickFlamer()
    {
        if (cardCounts[3] > 0)
        {
            currentCard = "flamer";
            gm.refillAmmo();
        }
    }
}
