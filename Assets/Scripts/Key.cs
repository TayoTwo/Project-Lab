using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public float rotSpeed;
    public bool endKey;

    private void Update() {
        //Rotate the coin at a set speed
        transform.Rotate(0,0,rotSpeed * Time.deltaTime);

    }

    void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag == "Player"){

            other.transform.root.GetComponent<PlayerInventory>().AddItem(gameObject);

        }
        
    }

}
