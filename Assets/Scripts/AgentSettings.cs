using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AgentSettings : ScriptableObject
{
    [SerializeField]
    public List<GOAPGoal> goals;
    [SerializeField]
    public List<GOAPAction> actions = new List<GOAPAction>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
