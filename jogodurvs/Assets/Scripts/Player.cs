using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public bool isJumping;

    private AudioSource stepSource;  
    private AudioSource actionSource; 
    public GameObject projectilePrefab;
    

    public AudioClip jump;
    public AudioClip Run;
    public AudioClip Ataque;

    private Rigidbody2D rig;
    private Animator Anim;

    private bool isWalking = false;
    private Coroutine stepCoroutine;

    public float stepInterval = 0.3f;

    public GameObject projétilPrefab; // Prefab do projétil
    public Transform ataquePosicao; // Posição onde o projétil irá sair (geralmente na frente do personagem)

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            stepSource = gameObject.AddComponent<AudioSource>();
            actionSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            stepSource = sources[0]; 
            actionSource = sources[1]; 
        }

        rig = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(moveInput, 0f, 0f);
        transform.position += movement * Time.fixedDeltaTime * speed;

        if (moveInput != 0)
        {
            transform.eulerAngles = (moveInput > 0) ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 180f, 0f);
            Anim.SetBool("Andar", true);

            if (!isWalking && !isJumping)
            {
                isWalking = true;

                if (stepCoroutine == null)
                {
                    stepCoroutine = StartCoroutine(PlayStepSound());
                }
            }
        }
        else
        {
            if (isWalking)
            {
                Anim.SetBool("Andar", false);
                StopStepSound();
            }
            isWalking = false;
        }

        if (moveInput == 0 && stepSource.isPlaying)
        {
            stepSource.Stop();
        }
    }

    IEnumerator PlayStepSound()
    {
        yield return new WaitForSeconds(0.05f);

        while (isWalking && !isJumping)
        {
            if (!stepSource.isPlaying) 
            {
                stepSource.PlayOneShot(Run);
            }
            yield return new WaitForSeconds(stepInterval);
        }
        stepCoroutine = null;
    }

    void StopStepSound()
    {
        if (stepCoroutine != null)
        {
            StopCoroutine(stepCoroutine);
            stepCoroutine = null;
        }

        if (stepSource.isPlaying)
        {
            stepSource.Stop();
        }
    }

    void Attack()
{
    if (Input.GetKeyDown(KeyCode.Z))
    {
        Anim.SetBool("Ataque", true);
        actionSource.PlayOneShot(Ataque);

        // Criar o projétil
        GameObject projectile = Instantiate(projétilPrefab, ataquePosicao.position, Quaternion.identity);

        // Descobrir a direção em que o jogador está virado
        int direction = transform.eulerAngles.y == 0 ? 1 : -1;

        // Configurar a direção do projétil
        projectile.GetComponent<Cogumelo>().SetDirection(direction);
    }
    else
    {
        Anim.SetBool("Ataque", false);
    }
}

    void LaunchProjétil()
    {
        if (projétilPrefab != null)
        {
            // Instancia o projétil
            GameObject projétil = Instantiate(projétilPrefab, ataquePosicao.position, Quaternion.identity);

            // Direção do projétil (baseada na direção do personagem)
            float direction = transform.localScale.x;  // Se o personagem está virado para a direita ou esquerda

            // Adiciona uma força ao projétil
            Rigidbody2D projétilRb = projétil.GetComponent<Rigidbody2D>();
            if (projétilRb != null)
            {
                projétilRb.velocity = new Vector2(direction * 10f, 0f);  // 10f é a velocidade do projétil
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            actionSource.PlayOneShot(jump);
            Anim.SetBool("Pulo", true);
            Anim.SetBool("Andar", false);

            rig.velocity = new Vector2(rig.velocity.x, 0);
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            isJumping = true;
            StopStepSound();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            Anim.SetBool("Pulo", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }
}
