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
        List<EnemyHealth> enemyPrefabs = encounterData.enemies;
        List<float> enemyPositions = encounterData.positions;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            EnemyHealth e = Instantiate(enemyPrefabs[i], transform);
            e.transform.localPosition = Vector3.right * enemyPositions[i];
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
            VictoryDefeatManager.Instance.Win();
        }
    }
}
