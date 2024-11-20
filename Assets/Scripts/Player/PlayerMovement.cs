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
    private float stamFillTime = 0f;
    public ParticleSystem blinkParticles;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
        staminaBar = GameObject.Find("Stamina Bar").GetComponent<Slider>();
        ll = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(new Vector2(horizontalInput * speed * Time.deltaTime * 500, rb.velocity.y), ForceMode2D.Impulse);
        rb.AddForce(new Vector2(rb.velocity.x, verticalInput * speed * Time.deltaTime * 500), ForceMode2D.Impulse);

        if (canBlink)
        {
            //problem: blink anim doesn't work when looking up or down because the up or down bools aren't being reset correctly
            anim.SetBool("run", horizontalInput != 0);
            if (anim.GetBool("down"))
            {
                anim.SetBool("uprun", false);
                anim.SetBool("downrun", verticalInput != 0f || horizontalInput != 0f);
            }
            else if (anim.GetBool("up"))
            {
                anim.SetBool("downrun", false);
                anim.SetBool("uprun", verticalInput != 0f || horizontalInput != 0f);
            }
            else if (!anim.GetBool("down") && !anim.GetBool("up") && verticalInput != 0f)
            {
                anim.SetBool("run", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canBlink && staminaBar.value >= 0.4 && (horizontalInput != 0f || verticalInput != 0f))
        {
            canBlink = false;
            Blink(horizontalInput, verticalInput);
        }

        if (staminaBar.value < 1f && canBlink == true)
        {
            staminaBar.value = Mathf.Lerp(staminaBar.value, staminaBar.maxValue, stamFillTime);
            stamFillTime += 0.0006f * Time.deltaTime;
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

    private void Blink(float horizontalInput, float verticalInput)
    {
        anim.SetTrigger("blink");
        float rotationAngle = Mathf.Atan2(verticalInput, horizontalInput);
        staminaBar.DOValue(staminaBar.value - 0.5f, 0.2f);
        rb.AddForce(new Vector2(horizontalInput * 5000, rb.velocity.y), ForceMode2D.Impulse);
        rb.AddForce(new Vector2(rb.velocity.x, verticalInput * 5000), ForceMode2D.Impulse);
        blinkParticles.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotationAngle);
        blinkParticles.Play();
        canBlink = true;
        anim.SetTrigger("blinkBack");
    }
}
