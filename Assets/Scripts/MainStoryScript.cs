using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainStoryScript : MonoBehaviour
{
    [Header("Texts")]
    public List<TMP_Text> Texts = new List<TMP_Text>();
    public AudioSource Ringing;
    public AudioSource PickUp;
    public AudioSource PutDown;
    public AudioSource MainT;
    public AudioSource OtherT;
    public int i = 0;
    public int j = 0;
    bool MainTalking = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Story());
    }

    public IEnumerator Story() 
    {
        yield return new WaitForSeconds(3f);
        Texts[i].gameObject.SetActive(true);
        Ringing.Play();
        yield return new WaitForSeconds(3f);
        Texts[i].gameObject.SetActive(false);
        i++;
        Texts[i].gameObject.SetActive(true);
        Ringing.Play();
        yield return new WaitForSeconds(2f);
        Texts[i].gameObject.SetActive(false);
        Ringing.Stop();
        PickUp.Play();
        yield return new WaitForSeconds(1f);
        i++;
        Texts[i].gameObject.SetActive(true);
        MainT.Play();
        while (Texts[i].gameObject.GetComponent<TypeEffect>().done != true) {
            yield return new WaitForSeconds(.01f);
        }
        MainT.loop = false;
        j = 2;
    }

    public IEnumerator NextTalk(bool MainTalking, int i) {
        if (MainTalking) {
            MainT.Play();
        } else {
            OtherT.Play();
        }
        Texts[i].gameObject.SetActive(true);
        while (Texts[i].gameObject.GetComponent<TypeEffect>().done != true) {
            yield return new WaitForSeconds(.01f);
        }
        if (MainTalking) {
            MainT.loop = false;
        } else {
            OtherT.loop = false;
        }
        if (i == 17) {
            yield return new WaitForSeconds(1f);
            Texts[i].gameObject.SetActive(false);
            PutDown.Play();
        }
        j++;
    }

    void Update() 
    {
        if (i > 1 && i < Texts.Count) {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && (i == j)) {
                 if (MainTalking) {
                    MainT.loop = true;
                } else {
                    OtherT.loop = true;
                }
                MainTalking = !MainTalking;
                Texts[i].gameObject.SetActive(false);
                i++;
                if (i != Texts.Count) {
                    StartCoroutine(NextTalk(MainTalking, i));
                }
            }
        } 
        if (i == Texts.Count) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LevelOne();
            }
        }
    }

}
