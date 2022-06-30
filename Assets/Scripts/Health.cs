using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public bool isHealthy;
    //public TMP_Text healthText;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update(){

        if(currentHealth/maxHealth <= 0.5f){

            isHealthy = false;

        } else if(currentHealth/maxHealth > 0.5f){

            isHealthy = true;

        }

    }
    
    void LateUpdate(){

        //healthText.text = "Health [" + currentHealth + "/" + maxHealth + "]";

    }

    public void TakeDamage(float dmg){

        currentHealth = Mathf.Clamp(currentHealth - dmg,0,maxHealth);
        Debug.Log(gameObject.name + " took " + dmg);

        if(currentHealth == 0){

            //Debug.Log("DYING");
            Die();

        }

    }

    public void HealDamage(float heal){

        currentHealth = Mathf.Clamp(currentHealth + heal,0,maxHealth);
        //Debug.Log(gameObject.name + " healed " + heal);

    }

    void Die(){

        Debug.Log("Die");

        if(gameObject.tag == "Player"){

            StartCoroutine("PlayerDeath");

        } else {

            Destroy(gameObject,0.767f);

        }

    }

    IEnumerator PlayerDeath(){

        yield return new WaitForSeconds(0.767f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

}
