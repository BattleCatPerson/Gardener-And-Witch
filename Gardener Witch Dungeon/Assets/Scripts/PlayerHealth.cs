using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public static float savedHealth = -1;
    private void Start()
    {
        base.Start();
        if (savedHealth < 0)
        {
            health = maxHealth;
        }
        else
        {
            health = savedHealth;
        }
    }
    private void Update()
    {
        base.Update();
        if (!VictoryDefeatManager.Instance.conditionChosen && health <= 0)
        {
            VictoryDefeatManager.Instance.SelectCondition(false);
        }
        savedHealth = health;
    }
}
