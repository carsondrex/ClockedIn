using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    private string currentCard;
    private int[] cardCounts = {0, 0, 0, 0};
    //count numbers
    private TMP_Text lcoilCountText;
    private TMP_Text shotgunCountText;
    private TMP_Text gattlingGunCountText;
    private TMP_Text flamerCountText;
    //weapon titles
    private TMP_Text lcoilTitleText;
    private TMP_Text shotgunTitleText;
    private TMP_Text gattlingGunTitleText;
    private TMP_Text flamerTitleText;
    //count number animators
    private Animator lcoilCounterAnim;
    private Animator shotgunCounterAnim;
    private Animator gattlingCounterAnim;
    private Animator flamerCounterAnim;
    //card animators
    private Animator lcoilCardAnim;
    private Animator shotgunCardAnim;
    private Animator gattlingCardAnim;
    private Animator flamerCardAnim;

    private GunManager gm;
    private SoundManager sm;
    private PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        currentCard = "default";
        lcoilCountText = GameObject.Find("LCoil Count Text").GetComponent<TextMeshProUGUI>();
        shotgunCountText = GameObject.Find("Shotgun Count Text").GetComponent<TextMeshProUGUI>();
        gattlingGunCountText = GameObject.Find("Gattling Gun Count Text").GetComponent<TextMeshProUGUI>();
        flamerCountText = GameObject.Find("Flamer Count Text").GetComponent<TextMeshProUGUI>();

        lcoilTitleText = GameObject.Find("LCoil Title Text").GetComponent<TextMeshProUGUI>();
        shotgunTitleText = GameObject.Find("Shotgun Title Text").GetComponent<TextMeshProUGUI>();
        gattlingGunTitleText = GameObject.Find("Gattling Gun Title Text").GetComponent<TextMeshProUGUI>();
        flamerTitleText = GameObject.Find("Flamer Title Text").GetComponent<TextMeshProUGUI>();

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
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

        //set to saved card counts
        lcoilCountText.text = cardCounts[0].ToString();
        shotgunCountText.text = cardCounts[1].ToString();
        gattlingGunCountText.text = cardCounts[2].ToString();
        flamerCountText.text = cardCounts[3].ToString();
    }


    public string getCurrentCard()
    {
        return currentCard;
    }

    public void setCardCounts(int[] set)
    {
        cardCounts = set;
    }

    public void setCurrentCard(string card)
    {
        currentCard = card;
        undoAllUnderlines();
        if (currentCard == "l-coil")
        {
            lcoilTitleText.fontStyle = FontStyles.Underline;
        }
        else if (currentCard == "shotgun")
        {
            shotgunTitleText.fontStyle = FontStyles.Underline;
        }
        else if (currentCard == "gattlinggun")
        {
            gattlingGunTitleText.fontStyle = FontStyles.Underline;
        }
        else if (currentCard == "flamer")
        {
            flamerTitleText.fontStyle = FontStyles.Underline;
        }
    }

    private void undoAllUnderlines()
    {
        lcoilTitleText.fontStyle = FontStyles.Normal;
        shotgunTitleText.fontStyle = FontStyles.Normal;
        gattlingGunTitleText.fontStyle = FontStyles.Normal;
        flamerTitleText.fontStyle = FontStyles.Normal;

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
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
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
        if (cardCounts[0] > 0 && currentCard != "l-coil" && !pm.dead)
        {
            sm.cardSelectSource.Play();
            currentCard = "l-coil";
            gm.refillAmmo();
        }
    }
    public void ClickShotgun()
    {
        if (cardCounts[1] > 0 && currentCard != "shotgun" && !pm.dead)
        {
            sm.cardSelectSource.Play();
            currentCard = "shotgun";
            gm.refillAmmo();
        }
    }
    public void ClickGattlingGun()
    {
        if (cardCounts[2] > 0 && currentCard != "gattlinggun" && !pm.dead)
        {
            sm.cardSelectSource.Play();
            currentCard = "gattlinggun";
            gm.refillAmmo();
        }
    }
    public void ClickFlamer()
    {
        if (cardCounts[3] > 0 && currentCard != "flamer" && !pm.dead)
        {
            sm.cardSelectSource.Play();
            currentCard = "flamer";
            gm.refillAmmo();
        }
    }
}
