using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cogumelo : MonoBehaviour
{
    public float speed = 10f; // Velocidade do projétil
    public float lifeTime = 3f; // Tempo de vida do projétil
    private Rigidbody2D rb;

    private int direction = 1; // Direção padrão (direita)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed * direction, 0f); // Aplica a força na direção correta

        Destroy(gameObject, lifeTime); // Destroi o projétil após "lifeTime" segundos
    }

    // Define a direção do projétil antes de ser lançado
    public void SetDirection(int dir)
    {
        direction = dir;
    }


}
