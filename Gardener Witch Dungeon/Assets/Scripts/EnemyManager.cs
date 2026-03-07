using UnityEngine;
using System.Collections.Generic;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public List<EnemyHealth> enemies;
    public PlayerHealth playerHealth;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
