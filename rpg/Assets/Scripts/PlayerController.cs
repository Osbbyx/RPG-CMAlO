using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   /* private float _horizontal;
    private float _vertical;*/
    private Vector3 direccion;
    private Animator playerAnimator;
    private bool atacando;
    private bool defendiendo;
    private float velocidadDeMovimientoAux;
   // private Inventario inventario;
    private bool bloqueaMovimiento;
    private bool puedePonerBomba = true;
    private Rigidbody2D rb;

    public Inventario inventario;
    public float speed;
    public GameObject armaActual;
    public int vida;
    

    public Vector3 GetDireccion()
    {
        return direccion;
    }
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }


    public bool GetPuedePonerBomba()
    {
        return puedePonerBomba;
    }

    public void SetPuedePonerBomba(bool newBomba)
    {
        puedePonerBomba = newBomba;
    }

    public void SumarVida(int vidaACurar)
    {
        vida += vidaACurar;
    }

    public void AplicarDano(int dano)
    {
        if (!defendiendo)
            vida -= dano;
    }

    void Start()
    {
        velocidadDeMovimientoAux = speed; 
    }

  
    void Update()
    {
        vida = Mathf.Clamp(vida, 0, 100);
        Defender();
        Atacar();
        Muerte();
    }

    private void FixedUpdate()
    {
        if (!bloqueaMovimiento)
        {
            Movimiento();
        }
    }

    private void Movimiento()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerAnimator.SetBool("Moviendo", true);
            direccion = new Vector3(0, 1, 0);
            transform.position += direccion * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            playerAnimator.SetBool("Moviendo", true);
            direccion = new Vector3(0, -1, 0);
            transform.position += direccion * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerAnimator.SetBool("Moviendo", true);
            direccion = new Vector3(-1, 0, 0);
            transform.position += direccion * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerAnimator.SetBool("Moviendo", true);
            direccion = new Vector3(1, 0, 0);
            transform.position += direccion * speed * Time.deltaTime;
        }
        else
        {
            playerAnimator.SetBool("Moviendo", false);
        }
        playerAnimator.SetFloat("MovimienotHorizontal", (int)direccion.x);
        playerAnimator.SetFloat("MovimientoVertical", (int)direccion.y);
        
    }

/*    private void Move()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(_horizontal, _vertical, 0);
        transform.Translate(movement * speed * Time.deltaTime); 
        
    }*/
public void Defender()
    {
        if (inventario.tieneEscudo)
        {
            if (Input.GetMouseButton(1))
            {
                playerAnimator.SetBool("Defendiendose", true);
                defendiendo = true;
                speed = velocidadDeMovimientoAux / 4;
            }
            else
            {
                playerAnimator.SetBool("Defendiendose", false);
                speed = velocidadDeMovimientoAux;
                defendiendo = false;
            }
        }
    }

    private void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(armaActual != null)
            {
                if (armaActual.CompareTag("Pistola"))
                {
                    AtaqueConPistola();
                }
                else if (armaActual.CompareTag("Espada"))
                {
                    AtaqueConEspada();
                }
                else if (armaActual.CompareTag("Boomerang"))
                {
                    AtaqueConBoomerang();
                }
                else if (armaActual.CompareTag("Bomba"))
                {
                    StartCoroutine(PlantarBomba());
                }
            }
                
        }
    }

    IEnumerator PlantarBomba()
    {
        if (puedePonerBomba)
        {
            yield return null;
            GameObject go = Instantiate(armaActual, transform.position, Quaternion.identity);
            go.SetActive(true);
            transform.GetChild(0).GetComponent<Inventario>().cantidadDeBombas--;
        }
    }

    public void FuerzaExplosion(Vector2 direccion, float retroceso)
    {
        StartCoroutine(AplicarFuerza(direccion, retroceso));
    }

    IEnumerator AplicarFuerza(Vector2 direccion, float retroceso)
    {
        rb.AddForce(direccion.normalized * retroceso, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        rb.velocity = Vector2.zero;
    }
    private void AtaqueConEspada()
    {
        if (!atacando)
        {
            atacando = true;
            playerAnimator.SetBool("AtacarConEspada",true);
            bloqueaMovimiento = true;
            armaActual.transform.position = transform.position + direccion;
            armaActual.SetActive(true);
            StartCoroutine(DehabilitaAtaqueEspada());


        }
    }

    IEnumerator DehabilitaAtaqueEspada()
    {
        yield return new WaitForSeconds(0.2f);
        bloqueaMovimiento = false;
        if (armaActual.CompareTag("Espada"))
            armaActual.SetActive(false);
        playerAnimator.SetBool("AtacarConEspada",false);
        playerAnimator.SetBool("AtacarConPistola",false);
        playerAnimator.SetBool("AtacarConBoomerang",false);
        atacando = false;
    }
    public void DeshabilitaAtaque()
    {
        bloqueaMovimiento = false;
        if (armaActual.CompareTag("Espada"))
            armaActual.SetActive(false);
        playerAnimator.SetBool("AtacarConEspada", false);
        playerAnimator.SetBool("AtacarConPistola", false);
        playerAnimator.SetBool("AtacarConBoomerang", false);
        atacando = false;
    }

    private void AtaqueConBoomerang()
    {
        if(armaActual.GetComponent<Boomerang>().GetPuedeLanzar())
        {
            StartCoroutine(AtacarConBoomerang());
        }
    }

    IEnumerator AtacarConBoomerang()
    {
        //puede cambiar al inventario al false
        inventario.SetPuedeCambiar(false);
        armaActual.GetComponent<Boomerang>().SetPuedeLanzar(false);
        //animacion con boomeran verdadero
        playerAnimator.SetBool("AtacarConBoomerang", true);
        GameObject go = Instantiate(armaActual, transform.position, Quaternion.identity);
        go.SetActive(true);
        Destroy(go, 7);
        yield return new WaitForSeconds(armaActual.GetComponent<Boomerang>().GetTiempoEntreLanzamiento());
        //puede cambiar al inventario a true
        inventario.SetPuedeCambiar(true);
        armaActual.GetComponent<Boomerang>().SetPuedeLanzar(true);
    }
    private void AtaqueConPistola()
    {
        armaActual.transform.position = transform.position + direccion + new Vector3(0, 0.7f, 0);
        if (armaActual.GetComponent<Pistola>().GetPuedeDisparar())
        {
            playerAnimator.SetBool("AtacarConPistola",true);
            armaActual.GetComponent<Pistola>().Disparar();
        }
    }
    
    private void Muerte()
    {
        if(vida <= 0)
        {
            playerAnimator.SetBool("Muerte", true);
        }
        
    }
}
