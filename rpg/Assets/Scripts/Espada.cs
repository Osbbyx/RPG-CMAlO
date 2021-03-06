using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public int daño;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemigo>().FuerzaEspada();
            AplicarDano(collision.GetComponent<Enemigo>());
        }
        else if (collision.CompareTag("Planta")) 
        {
            Destroy(collision.gameObject);
        }
    }

   private void AplicarDano(Enemigo enemigo)
    {
        enemigo.AplicarDano(daño);
    }

}
