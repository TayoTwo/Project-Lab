using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetter : MonoBehaviour
{
    //Prefab to spawn , number too spawn
    [SerializeField]
    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    public List<int> prefabAmount = new List<int>();
    GridManager gridManager;

    //Spawn a set amount of each prefab randomly
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        
        for(int i = 0;i < prefabsToSpawn.Count;i++){

            for(int j = 0; j < prefabAmount[i];j++){

                float spawnPosX = (gridManager.gridDim.x - 1) * gridManager.unitLength * Random.value;
                float spawnPosY = (gridManager.gridDim.y - 1) * gridManager.unitLength * Random.value;

                Vector3 spawnPos = new Vector3(spawnPosX,0,spawnPosY) - gridManager.offset;

                Instantiate(prefabsToSpawn[i],spawnPos,Quaternion.identity);
            
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
