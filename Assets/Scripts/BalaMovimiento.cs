using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaMovimiento : MonoBehaviour
{
    public GameObject player;
    private Transform playerTransf;
    private Rigidbody2D balaRB;

    public float balaVelocidad = 15;
    public float balaVida = 3f;

    //se ejecuta primero, antes que start
    void Awake()
    {
        balaRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransf = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerTransf.localScale.x > 0)
        {
            balaRB.velocity = new Vector2(balaVelocidad, balaRB.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            balaRB.velocity = new Vector2(-balaVelocidad, balaRB.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, balaVida);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            player.GetComponent<NinjaController>().puntaje += 10;
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
