using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistola : MonoBehaviour
{
    private bool puedeDisparar = true;
    private Vector3 posicionInicial;
    public GameObject bala;
    public int danoPorDisparo;
    public float tiempoDeDisparo;

    
    public Vector3 GetPosicionInicial()
    {
        return posicionInicial;
    }

    public bool GetPuedeDisparar()
    {
        return puedeDisparar;
    }

    public void Awake()
    {
        posicionInicial = transform.position;
    }

    public void Disparar()
    {
        StartCoroutine(Disparo());
    }
    private IEnumerator Disparo()
    {
        if (puedeDisparar)
        {
            puedeDisparar = false;
            GameObject go = Instantiate(bala, transform.position, Quaternion.identity);
            go.GetComponent<Bala>().SetDanoAplicable(danoPorDisparo);
            Destroy(go, 5);
           
            yield return new WaitForSeconds(tiempoDeDisparo);
            puedeDisparar = true;
        }
    }
}
