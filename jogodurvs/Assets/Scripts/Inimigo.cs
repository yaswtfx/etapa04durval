using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            // Seguir o player
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction * speed;
            anim.SetBool("isMoving", true);
        }
        else
        {
            movement = Vector2.zero; // Para de se mover
            anim.SetBool("isMoving", false);
        }

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }

        void FixedUpdate()
    {
        // Move o inimigo
        //rb.velocity = movement;
        if (movement != Vector2.zero)
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
    }

    void Attack()
    {
        anim.SetTrigger("isAttacking");
        Debug.Log("O inimigo atacou!");
        // Aqui você pode adicionar dano ao player
        // player.GetComponent<PlayerHealth>().TakeDamage(10);
    }
    }
}

