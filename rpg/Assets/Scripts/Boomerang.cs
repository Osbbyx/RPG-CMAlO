using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    private bool puedeLanzar = true;
    private Vector3 direccion;
    private PlayerController playerController;
    public int dano;
    public float velocidadBoomerang;
    public float tiempoEntreLanzamientos;

    public bool GetPuedeLanzar()
    {
        return puedeLanzar;
    }
    public void SetPuedeLanzar(bool newPuedeLanzar)
    {
        puedeLanzar = newPuedeLanzar;
    }

    public float GetTiempoEntreLanzamiento()
    {
        return tiempoEntreLanzamientos;
    }

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        direccion = playerController.GetDireccion();
    }

    private void Update()
    {
        transform.position += direccion * velocidadBoomerang * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemigo>().AplicarDano(dano);
        }
    }
}
