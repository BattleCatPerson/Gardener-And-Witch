using UnityEngine;
using System.Collections.Generic;

public class FloorTracker : MonoBehaviour
{
    public static int floor = 0;
    public static int runCollectedBones = 0;
    public static bool firstFloor = true;
    [SerializeField] List<EncounterData> encounterDataList;
    [SerializeField] EnemyManager enemyManager;
    private void Awake()
    {
        EncounterData data = encounterDataList[floor];
        enemyManager.SetEnemies(data);
        if (firstFloor)
        {
            firstFloor = false;
            runCollectedBones = 0;
        }
    }
    public void IncrementFloor() => floor++;
}
