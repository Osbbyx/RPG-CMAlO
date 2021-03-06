using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    private PlayerController playerController;
    public List<GameObject> objetosADestruir;
    public float tiempoParaExplotar;
    public int dano;
    public float retroceso;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        StartCoroutine(Explotar());

    }

    IEnumerator Explotar()
    {
        playerController.SetPuedePonerBomba(false);
        yield return new WaitForSeconds(tiempoParaExplotar);

            if(objetosADestruir.Count > 0)
            {
            for(int i = 0; i < objetosADestruir.Count; i++)
            {
                if (objetosADestruir[i].CompareTag("Player"))
                {
                  //  playerController.AplicarDano(dano);
                    Vector2 direccion = playerController.gameObject.transform.position - transform.position;
                    playerController.FuerzaExplosion(direccion, retroceso);
                }else if (objetosADestruir[i].CompareTag("Enemy"))
                {
                   objetosADestruir[i].GetComponent<Enemigo>().AplicarDano(dano);
                    Vector2 direccion = objetosADestruir[i].gameObject.transform.position - transform.position;
                   objetosADestruir[i].GetComponent<Enemigo>().FuerzaExplosion(direccion, retroceso);
                }
                else
                {
                    Destroy(objetosADestruir[i]);
                }
            }

        }
        playerController.SetPuedePonerBomba(true);
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if( collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            objetosADestruir.Remove(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Explotable") || collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            objetosADestruir.Add(collision.gameObject);
        }
    }

}
