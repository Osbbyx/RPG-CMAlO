﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombas : MonoBehaviour
{
    private Inventario inventario;

    private void Awake()
    {
        inventario = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Inventario>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventario.cantidadDeBombas++;
            Destroy(gameObject);
        }
    }
}
