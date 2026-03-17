using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Encounter Data", order = 1)]
public class EncounterData : ScriptableObject
{
    public List<EnemyHealth> enemies;
    public List<float> positions;
}
