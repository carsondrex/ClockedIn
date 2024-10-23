using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, verticalInput * speed);

        anim.SetBool("run", horizontalInput != 0);
        if (anim.GetBool("down") == true)
        {
            anim.SetBool("downrun", verticalInput < -0.01f);
        }
    }
}
