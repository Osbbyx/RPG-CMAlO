using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogos : MonoBehaviour
{
    private bool _enDialogo;
    public string[] dialogo;
    public Text textDialogos;
    public PlayerController playerController;
    public float tiempoEntreTextos;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKeyDown(KeyCode.Space) && !_enDialogo)
        {
            textDialogos.transform.parent.gameObject.SetActive(true);
            _enDialogo = true;
            playerController.enabled = false;
            StartCoroutine(Dialogar());
        }
    }

    private IEnumerator Dialogar()
    {
        for(int i = 0; i < dialogo.Length; i++)
        {
            char[] textoActual = dialogo[i].ToCharArray();
            for(int j = 0; j < textoActual.Length; j++)
            {
                textDialogos.text += textoActual[j];
                if (Input.GetKeyDown(KeyCode.Space)){
                    textDialogos.text = dialogo[i];
                    j = textoActual.Length - 1;
                }
                yield return new WaitForSeconds(tiempoEntreTextos);
            }
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Continuar");
                yield return null;
            }
            textDialogos.text = string.Empty;
            yield return null;
        }
        _enDialogo = false;
        textDialogos.transform.parent.gameObject.SetActive(false);
        playerController.enabled = true;
    }
}
