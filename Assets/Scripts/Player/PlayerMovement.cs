using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour,IDamagable
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Animator anim;
    public float health = 100f;
    //health bar
    private Slider healthBar;
    private float fillSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, verticalInput * speed);

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

    public void TakeDamage(int damage)
    {
        CinemachineShake.Instance.ShakeCamera(4.5f, .5f);
        health -= damage;
        float targetFillAmount = health / 100;
        healthBar.DOValue(targetFillAmount, fillSpeed);
        if (health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            //Invoke("GameOver", 1f); //game over screen
        }
    }
}
