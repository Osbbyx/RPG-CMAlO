using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Vector3 posicionAnterior;

    private Vector3 direccion;
    private Animator animator;
    public List<Transform> wayPoints;
    private int currentPoint = 0;

    private bool atacar = false;
    private bool atacando = false;
    private bool jugadorFueraArea = true;
    private bool recibeDano = false;
    public float tiempoDeReaccion;

    public string tipoDeEnemigo;
    public int vida;
    public int daño;
    public float VelocidadDeMovimiento;
    public float retroceso;
    public float distanciaDeDeteccion;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public string GetTipoDeEnemigo()
    {
        return tipoDeEnemigo;
    }
    public void AplicarDano(int danoRecibido)
    {
        if(tipoDeEnemigo == "Megalodon")
        {
            if (recibeDano)
                vida -= danoRecibido;
        }
        else
        {
            vida -= danoRecibido;
        }
    }

    public void DetectarJugador()
    {
        if(Vector3.Distance(player.transform.position, transform.position)<= distanciaDeDeteccion)
        {
            IAMurcielago();
        }
    }

    private void Update()
    {
        Muerte();
        if(playerController != null)
        {
            if (tipoDeEnemigo == "Murcielago") 
               DetectarJugador();
            if(tipoDeEnemigo == "Bandido")
            {
                IABandido();
            }
            if(tipoDeEnemigo == "Megalodon")
            {
                IAMegalodon();
            }
        }
    }

    public void FuerzaEspada()
    {
        StartCoroutine(AplicarFuerza());
    }

    IEnumerator AplicarFuerza()
    {
        Vector2 direccion = posicionAnterior - transform.position;
        rb.AddForce(direccion.normalized * retroceso, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
    }

    public void FuerzaExplosion(Vector2 direccion, float retroceso)
    {
        StartCoroutine(AplicarFuerza(direccion,  retroceso));
    }

    IEnumerator AplicarFuerza(Vector2 direccion, float retroceso)
    {
        rb.AddForce(direccion.normalized * retroceso, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
    }


    private void Muerte()
    {
        if(vida <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorFueraArea = true;
        }
    }

    private IEnumerator OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(atacar && !atacando)
            {
                atacando = true;
                yield return new WaitForSeconds(tiempoDeReaccion);
                animator.SetBool("Atacar", true);
                if (!jugadorFueraArea)
                {
                    playerController.AplicarDano(daño);
                    playerController.FuerzaExplosion(player.transform.position - transform.position, 8f);
                    
                }
                GetComponent<CircleCollider2D>().radius = 0;
                recibeDano = true;
                yield return new WaitForSeconds(1.5f);
                recibeDano = false;
                animator.SetBool("Atacar", false);
                GetComponent<CircleCollider2D>().radius = 1;
                atacar = false;
                atacando = false;
            }
            yield return null;
        }
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorFueraArea = false;
            if(tipoDeEnemigo == "Megalodon")
            {
                if (!atacar)
                {
                    GetComponent<CircleCollider2D>().radius = 2.4f;
                    atacar = true;
                }

            }
            else
            {

            playerController.AplicarDano(daño);
            StartCoroutine(AplicarFuerza());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.AplicarDano(daño);
            if (tipoDeEnemigo == "Bandido")
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                animator.SetBool("Atacando", true);
                playerController.FuerzaExplosion(player.transform.position - transform.position, 7f);
            }
            else
            {
                StartCoroutine(AplicarFuerza());
            }
        }
    }

    public void DeshabilitaAtaque()
    {
        animator.SetBool("Atacando", false);
    }
    private void IAMurcielago()
    {
        posicionAnterior = transform.position;
        float paso = VelocidadDeMovimiento * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, paso);
    }

    private void IABandido()
    {
        float paso = VelocidadDeMovimiento * Time.deltaTime;
        if(Vector3.Distance(player.transform.position, transform.position) <= distanciaDeDeteccion)
        {
            Vector3 jugadorY = new Vector3(0, player.transform.position.y, 0);
            Vector3 enemigoY = new Vector3(0, transform.position.y, 0);
            Vector3 jugadorX = new Vector3(player.transform.position.x,0, 0);
            Vector3 enemigoX = new Vector3( transform.position.x, 0, 0);

            if(Vector3.Distance(jugadorY, enemigoY) >= Vector3.Distance(jugadorX, enemigoX))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(enemigoX.x, jugadorY.y), paso);

                if (rb.velocity == Vector2.zero)
                    direccion = new Vector3(enemigoX.x, jugadorY.y) - transform.position;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(jugadorX.x, enemigoY.y), paso);

                if (rb.velocity == Vector2.zero)
                    direccion = new Vector3(jugadorX.x, enemigoY.y) - transform.position;
            }
            direccion = direccion.normalized;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].position, paso);
            direccion = wayPoints[currentPoint].position - transform.position;

            if (transform.position == wayPoints[currentPoint].position)
                currentPoint++;

            if (currentPoint >= wayPoints.Count)
                currentPoint = 0;
        }

        animator.SetFloat("MovimientoHorizontal", direccion.x);
        animator.SetFloat("MovimientoVertical", direccion.y);
    }

    private void IAMegalodon()
    {
        if (!atacar)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= distanciaDeDeteccion)
            {
                float paso = VelocidadDeMovimiento * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, paso);
            }
        }
        
    }
}
