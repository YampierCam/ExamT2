using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NinjaController : MonoBehaviour
{

    public float Velocidad = 10;
    public float FuerzaSalto = 25;

    public bool grounded;
    public bool dobleSalto;

    public Transform GroundCheck;
    public float checkRadius = 0.1f;
    public LayerMask queEsSuelo;

    public Transform balaSpawner;
    public GameObject balaPrefab;

    Rigidbody2D rb;
    Animator animator;

    public bool onLadder = false;
    public float exitHop = 3f;
    public BoxCollider2D plataforma;

    private bool deliza = false;

    public int puntaje = 0;
    public int vida = 3;
    public Text puntajeTexto;
    public Text vidaTexto;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, queEsSuelo);
        puntajeTexto.text = "Puntaje: " + puntaje;
        vidaTexto.text = "Vidas: " + vida;
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded)
            dobleSalto = false;

        if(vida == 0)
        {
            //rb.velocity = Vector2.zero;
            animator.SetBool("EstaMuerto", true);
            GetComponent<NinjaController>().enabled = false;
        }

        PlayerCorrer();
        PlayerSaltar();
        PlayerDisparar();
        PlayerDeslizarse();
        PlayerMuerePorCaida();
        PlayerUsaParacaidas();
    }

    public void PlayerCorrer()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(Velocidad, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(Velocidad * -1, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetFloat("VelocidadX", Mathf.Abs(rb.velocity.x));
    }

    public void PlayerSaltar()
    {

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, FuerzaSalto);

        }

        if (Input.GetKeyDown(KeyCode.Space) && !dobleSalto && !grounded)
        {

            rb.velocity = new Vector2(rb.velocity.x, FuerzaSalto);
            dobleSalto = true;
        }

        if(!onLadder)
            animator.SetFloat("VelocidadY", Mathf.Abs(rb.velocity.y));
    }

    public void PlayerDisparar()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("EstaDisparando", true);
            Instantiate(balaPrefab, balaSpawner.position, balaSpawner.rotation);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            animator.SetBool("EstaDisparando", false);
        }
    }

    public void PlayerDeslizarse()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            deliza = true;
            animator.SetBool("seDesliza", deliza);
        }
        else
        {
            deliza = false;
            animator.SetBool("seDesliza", deliza);
        }
    }

    public void PlayerMuerePorCaida()
    {
        if (Mathf.Abs(rb.velocity.y) > 35 )
        {
            vida = 0;
            animator.SetBool("EstaMuerto", true);
        }

    }

    public void PlayerUsaParacaidas()
    {
        if (Input.GetKey(KeyCode.X))
        {
            rb.gravityScale = 0.5f;
            animator.SetBool("usaParacaidas", true);
        }

        else
        {
            if (Input.GetKeyUp(KeyCode.X))
            {
                rb.gravityScale = 3;
                animator.SetBool("usaParacaidas", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemigo")
        {
            if(vida > 0)
                vida -= 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Escalera" && Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(0, 6);

            if (rb.velocity.y != 0)
            {
                onLadder = true;
                plataforma.enabled = false;
                rb.gravityScale = 0;

            }
            if (rb.velocity.y == 0 && onLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            animator.SetBool("EnEscalera", onLadder);

        }

        if (collision.tag == "Escalera" && Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(0, -6);
            if (rb.velocity.y != 0)
            {
                onLadder = true;
                plataforma.enabled = false;
                rb.gravityScale = 0;
            }

            if (rb.velocity.y == 0 && onLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);

            }
            animator.SetBool("EnEscalera", onLadder);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Escalera" && onLadder)
        {
            onLadder = false;
            plataforma.enabled = true;
            rb.gravityScale = 3;
            animator.SetBool("EnEscalera", onLadder);

            if (grounded)
                rb.velocity = new Vector2(rb.velocity.x, exitHop);
        }
    }
}

    

