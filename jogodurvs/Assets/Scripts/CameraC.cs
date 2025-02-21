using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    
    private Transform alvo;
    

    private Vector3 direcao;

    private float y;

    private float x;
    // Start is called before the first frame update
    void Start()
    {
        alvo = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Local();    
    }

    void Local()
    {
        y = 0;
        x = alvo.position.x;
        
        if (x < 0)
           x = 0;
        if (x > 36)
        x = 36;
        direcao = new Vector3(x, y, -15f);
        transform.position = direcao;
    }
}
