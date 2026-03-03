using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] bool dead;
    public void TakeDamage (int damage)
    {
        health -= damage;
        dead = health <= 0;
    }
}
