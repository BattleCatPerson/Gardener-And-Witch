using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<EnemyHealth> enemies;
    public PlayerHealth playerHealth;
    public EncounterData encounterData;
    private void Awake()
    {
        Instance = this;
    }
    public void SetEnemies(EncounterData data)
    {
        encounterData = data;
        List<EnemyHealth> enemyPrefabs = encounterData.enemies;
        List<Vector2> enemyPositions = encounterData.positions;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            EnemyHealth e = Instantiate(enemyPrefabs[i], transform);
            e.transform.localPosition = enemyPositions[i];
            enemies.Add(e);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveEnemy(EnemyHealth e)
    {
        enemies.Remove(e);
        if (enemies.Count == 0)
        {
            Debug.Log("You win");
            VictoryDefeatManager.Instance.SelectCondition(true);
        }
    }
    public void BlurUnits(List<Health> activeUnits)
    {
        if (!activeUnits.Contains(playerHealth))
        {
            playerHealth.Blur(true);
        }
        foreach (Health health in enemies)
        {
            if (!activeUnits.Contains(health)) health.Blur(true);
        }
    }
    public void Unblur()
    {
        playerHealth.Blur(false);
        foreach (Health health in enemies)
        {
            health.Blur(false);
        }
    }
}
