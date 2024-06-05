using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject;

    void Start()
    {
        // Comprobar si el jugador está presente en la escena
        if (playerObject == null)
        {
            Debug.LogError("El GameManager no tiene asignado el objeto del jugador.");
            return;
        }

        // Desactivar al jugador
        playerObject.SetActive(false);
         
        // Comprobar si ya hay otro jugador activo en la escena
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            // Si no hay ningún jugador activo, activar al jugador y asignarle la etiqueta "Player"
            playerObject.SetActive(true);
            playerObject.tag = "Player";
        }
    }
    public void CheckForPlayer()
    {

    }
}