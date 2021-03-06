using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    private Vector3 direccionActual;
    private int danoAplicable = 0;
    private PlayerController playerController;
    public float velocidadProyectil;

    public void SetDanoAplicable(int dano)
    {
        danoAplicable = dano;

    }
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        direccionActual = playerController.GetDireccion();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemigo>().AplicarDano(danoAplicable);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += direccionActual * velocidadProyectil * Time.deltaTime;
    }
}
