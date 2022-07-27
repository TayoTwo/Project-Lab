using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{

    PlayerInventory pi;

    void Start(){

        pi = FindObjectOfType<PlayerInventory>();

    }

    //Check if the player has a key and if they do let them progress
    void OnTriggerEnter(Collider other){

        if(other.transform.root.tag == "Player"){

            //pi = gameManager.player.GetComponent<PlayerInventory>();

            for(int i = 0;i < pi.items.Count;i++){

                //Debug.Log(pi.items[i]);

                if(pi.items[i].Contains("EndKey")){

                    pi.RemoveItem(i);
                    Valid();
                    return;

                }

            }

            Invalid();

        }

    }

    //What to do if the player has a key
    void Valid(){

        Debug.Log("Player has a key");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //What to do if the player doesn't have a key
    void Invalid(){

        Debug.Log("Player doesn't have a key");

    }
}
