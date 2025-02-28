using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public bool isJumping;
    public BoxCollider2D bx;
    private Rigidbody2D rig;

    private bool movimento = true;

    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        
        rig = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Jump();
        Move();
        Attack();
    }

     void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * speed;
        
        if (Input.GetAxis("Horizontal") > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Anim.SetBool("Andar", true);
            
        }

        if (Input.GetAxis("Horizontal") < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            Anim.SetBool("Andar", true);
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            Anim.SetBool("Andar", false);
        }
    }
    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Anim.SetBool("Ataque",true);
        }
        else {
            Anim.SetBool("Ataque",false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                Anim.SetBool("Pulo",true);
                Anim.SetBool("Andar",false);
                rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            Anim.SetBool("Pulo", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }
}
