using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Encounter Data/Data", order = 1)]
public class EncounterData : ScriptableObject
{
    public List<EnemyHealth> enemies;
    public List<float> positions;
}
