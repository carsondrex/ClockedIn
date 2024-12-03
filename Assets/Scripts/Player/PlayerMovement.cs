using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour,IDamagable
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Animator anim;
    public float health = 100f;
    private Slider healthBar;
    private float fillSpeed = 0.3f;
    private LevelLoader ll;
    private bool canBlink = true;
    private Slider staminaBar;
    private int maxStamina = 100;
    private int currentStamina;
    public ParticleSystem blinkParticles;
    private WaitForSeconds regenTick = new WaitForSeconds(0.03f);
    private Coroutine regen;
    public bool isInvincible;
    private GunManager gm;
    //card drops
    public ParticleSystem dropPickUpParticles;
    private CardManager cm;
    private SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
        staminaBar = GameObject.Find("Stamina Bar").GetComponent<Slider>();
        ll = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        gm = GetComponent<GunManager>();
        cm = GameObject.Find("Cards").GetComponent<CardManager>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        isInvincible = false;
        currentStamina = 100;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(new Vector2(horizontalInput * speed * Time.deltaTime * 1000, rb.velocity.y), ForceMode2D.Force);
        rb.AddForce(new Vector2(rb.velocity.x, verticalInput * speed * Time.deltaTime * 1000), ForceMode2D.Force);

        if (canBlink)
        {
            if (anim.GetBool("down"))
            {
                anim.SetBool("downrun", verticalInput != 0f || horizontalInput != 0f);
                anim.SetBool("uprun", false);
                anim.SetBool("run", false);
            }
            else if (anim.GetBool("up"))
            {
                anim.SetBool("uprun", verticalInput != 0f || horizontalInput != 0f);
                anim.SetBool("downrun", false);
                anim.SetBool("run", false);
            }
            else if (anim.GetBool("leftRight"))
            {
                if (verticalInput != 0f || horizontalInput != 0f)
                {
                    anim.SetBool("run", verticalInput != 0f || horizontalInput != 0f);
                    anim.SetBool("downrun", false);
                    anim.SetBool("uprun", false);
                }
                else
                {
                    anim.SetBool("run", false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canBlink && currentStamina == 100 && (horizontalInput != 0f || verticalInput != 0f))
        {
            canBlink = false;
            StartCoroutine(Blink(horizontalInput, verticalInput));
        }
    }

    public void TakeDamage(int damage)
    {
        CinemachineShake.Instance.ShakeCamera(5.5f, 1f);
        health -= damage / 2; //divide by 2 since we have two colliders on player
        float targetFillAmount = health / 100;
        healthBar.DOValue(targetFillAmount, fillSpeed);
        if (health <= 0)
        {
            ll.GameOver(); ; //game over screen, enter corresponding scene index here.
        }
    }

    public void Heal(float healAmount)
    {
        float heal = health + healAmount;
        if (heal > 100)
        {
            heal = 100;
        }
        float targetFillAmount = heal / 100;
        healthBar.DOValue(targetFillAmount, fillSpeed);
    }

    private IEnumerator Blink(float horizontalInput, float verticalInput)
    {
        anim.SetBool("blink", true);
        float rotationAngle = Mathf.Atan2(verticalInput, horizontalInput);
        //manage stamina
        currentStamina = 0;
        staminaBar.DOValue(currentStamina, 0.2f);
        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(RegenStamina());

        isInvincible = true;
        rb.AddForce(new Vector2(horizontalInput * 5000, rb.velocity.y), ForceMode2D.Impulse);
        rb.AddForce(new Vector2(rb.velocity.x, verticalInput * 5000), ForceMode2D.Impulse);

        blinkParticles.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotationAngle);
        blinkParticles.Play();

        yield return new WaitForSeconds(.1f);
        anim.SetBool("blinkBack", true);
        anim.SetBool("blink", false);
        yield return new WaitForSeconds(.1f);
        anim.SetBool("blinkBack", false);
        isInvincible = false;
        canBlink = true;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);
        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

    public bool getIsInvincible()
    {
        return isInvincible;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Drop")
        {
            ParticleSystem dropPickUp = Instantiate(dropPickUpParticles, collision.gameObject.transform.position, Quaternion.identity);
            if (collision.gameObject.name == "HealthDrop(Clone)")
            {
                sm.healSource.Play();
                Heal(collision.gameObject.GetComponent<HealthDrop>().healAmount);
            }
            else if (collision.gameObject.name == "LCoilDrop(Clone)")
            {
                cm.changeCardCount(0, 1);
            }
            else if (collision.gameObject.name == "ShotgunDrop(Clone)")
            {
                cm.changeCardCount(1, 1);
            }
            else if (collision.gameObject.name == "GattlingGunDrop(Clone)")
            {
                cm.changeCardCount(2, 1);
            }
            else if (collision.gameObject.name == "FlamerDrop(Clone)")
            {
                cm.changeCardCount(3, 1);
            }

            Destroy(collision.gameObject);
            StartCoroutine(DestroyParticles(dropPickUp));
        }
    }

    IEnumerator DestroyParticles(ParticleSystem dropPickUp)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(dropPickUp.gameObject);
    }
}
