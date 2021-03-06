using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    private int armaActual = 0;
    private bool puedeCambiar = true;
    public int cantidadDeArmasTotal;

    [Space(10)]
    public Espada espada;
    public Sprite ImagenEspada;
    public bool tieneEspada;
    [Space(10)]
    public Pistola pistola;
    public Sprite ImagenPistola;
    public bool tienePistola;
    [Space(10)]
    public Bomba bomba;
    public Sprite ImagenBomba;
    public bool tieneBomba;
    [Space(10)]
    public Boomerang boomerang;
    public Sprite ImagenBoomerang;
    public bool tieneBoomerang;
    [Space(10)]
    public PlayerController playerController;
    public Image imageArmaActual;
    public Image imagenPociones;
    public Image imagenLlaves;
    [Space(20)]
    public int cantidadDeBombas;
    public int cantidadDePosiciones;
    public int vidaACurar;
    [Space(20)]
    public List<string> ID_llavesAPuertasAsignadas;
    [Space(20)]
    public bool tieneEscudo;
    public int monedas;
    public void SetPuedeCambiar(bool newPuedeCambiar)
    {
        puedeCambiar = newPuedeCambiar;
    }
    public void SeleccionarArma()
    {
        if (puedeCambiar)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (armaActual == cantidadDeArmasTotal) 
                    armaActual = 1;
                    else
                        armaActual++;
            }
            Seleccion();
        }
    }

    private void Seleccion()
    {
        if (!tieneEspada && !tienePistola && !tieneBomba && tieneBoomerang)
            return;
        if (armaActual == 0)
            playerController.armaActual = null;
        else if(armaActual == 1)
        {
            if (tieneEspada)
            {
                playerController.armaActual = espada.gameObject;
                imageArmaActual.sprite = ImagenEspada;
            }
            else
            {
                armaActual++;
                Seleccion();
            }
        }
        else if (armaActual == 2)
        {
            if (tienePistola)
            {
                playerController.armaActual = pistola.gameObject;
                imageArmaActual.sprite = ImagenPistola;
            }
            else
            {
                armaActual++;
                Seleccion();
            }
        }
        else if (armaActual == 3)
        {
            if (tieneBomba)
            {
                playerController.armaActual = bomba.gameObject;
                imageArmaActual.sprite = ImagenBomba;
            }
            else
            {
                armaActual++;
                Seleccion();
            }
        }
        else if (armaActual == 4)
        {
            if (tieneBoomerang)
            {
                playerController.armaActual = boomerang.gameObject;
                imageArmaActual.sprite = ImagenBoomerang;
            }
            else
            {
                armaActual = 1;
                Seleccion();
            }
        }
    }

    private void Update()
    {
        if (cantidadDeBombas <= 0)
            tieneBomba = false;
        else tieneBomba = true;

        SeleccionarArma();
        UsarPocion();
    }

    private void UsarPocion()
    {
        if (cantidadDePosiciones > 0)
        {
            imagenPociones.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (playerController.vida != 100)
                {
                    Debug.Log("lol");
                    playerController.SumarVida(vidaACurar);
                    cantidadDePosiciones--;
                }
            }
        }
        else
        {
            imagenPociones.gameObject.SetActive(false);
        }
        }
        public void UsarLlave(string ID_Puerta)
        {
            for(int i = 0; i < ID_llavesAPuertasAsignadas.Count; i++)
        {
            if(ID_llavesAPuertasAsignadas[i] == ID_Puerta)
            {
                Debug.Log("abre");
            }
        }
        }
    
}
