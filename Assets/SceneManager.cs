using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86;


public class Scene_Manager : MonoBehaviour
{
    public int scID;
    
    //Esta funcion se encarga de que al chocar con un objeto "GOTO", pasemos a la escena correspondiente
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(scID >= 0)
        {
            scID = SceneManager.GetActiveScene().buildIndex;
        }
        if (collision.name == "Player")
        {
            Debug.Log("Trigger "+scID); 
             if (CompareTag("NextScene"))
             {
                if(scID < 0)
                {
                    Destroy(collision.gameObject);
                    scID = 0;
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                }

                else SceneManager.LoadScene(scID + 1, LoadSceneMode.Single);

             }
            else if (CompareTag("ReturnScene"))
            {
                SceneManager.LoadScene(scID - 1, LoadSceneMode.Single);
            }
        }
        
    }
    
    
}
