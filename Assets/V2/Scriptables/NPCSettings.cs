using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCSettings : ScriptableObject
{
    
    [SerializeField]
    public List<GOAPGoal> goals;
    [SerializeField]
    public List<GOAPAction> actions = new List<GOAPAction>();

}
