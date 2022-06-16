using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> items = new List<string>();
    public Transform invObj;

    public void AddItem(GameObject item){

        items.Add(item.name);
        item.transform.parent = invObj;
        Destroy(item);

    }

    public void RemoveItem(int index){

        items.RemoveAt(index);

    }

    public void ClearInventory(){

        for(int i = 0; i < items.Count;i++){

            items.Remove(items[i]);
            RemoveItem(i);

        }

    }

}
