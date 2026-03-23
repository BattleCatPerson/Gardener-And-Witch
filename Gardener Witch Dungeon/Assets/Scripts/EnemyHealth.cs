using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] int boneDrops;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (health <= 0)
        {
            EnemyManager.Instance.RemoveEnemy(this);
            ResourceTracker.bones += boneDrops;
            FloorTracker.runCollectedBones += boneDrops;
            Destroy(gameObject);
        }
    }
}
