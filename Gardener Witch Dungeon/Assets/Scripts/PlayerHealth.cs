using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public static float savedHealth = -1;
    [SerializeField] float blockCooldown;
    [SerializeField] bool canBlock;
    [SerializeField] InputActionReference blockInput;
    [SerializeField] float blockDamagePercent;
    [SerializeField] List<float> blockUpgradePercentages;
    Material material;
    private void Start()
    {
        canBlock = true;
        Initialize();
        if (savedHealth < 0)
        {
            health = maxHealth;
        }
        else
        {
            health = savedHealth;
        }
        blockInput.action.performed += Block;
        blockDamagePercent = blockUpgradePercentages[StatManager.upgrades[UpgradeType.Block]];
        material = sprite.material;
        SceneManager.activeSceneChanged += UnbindBlock;
    }
    private void Update()
    {
        AdjustBar();
        material.SetFloat("_BlurAmount", 0.1f);
        sprite.material = material;
        if (!VictoryDefeatManager.Instance.conditionChosen && health <= 0)
        {
            VictoryDefeatManager.Instance.SelectCondition(false);
        }
        savedHealth = health;
    }
    public void Block(InputAction.CallbackContext context)
    {
        if (canBlock && EnemySkillManager.enemyAttacking)
        {
            Debug.Log("block");
            animator.SetTrigger("Block");
        }
        //if (canBlock && EnemySkillManager.enemyAttacking) StartCoroutine(BlockTimer());
    }
    public void StartBlock()
    {
        canBlock = false;
        damageMultiplier = blockDamagePercent;
    }
    public void EndBlock()
    {
        damageMultiplier = 1;
        StartCoroutine(BlockTimer());
    }
    public IEnumerator BlockTimer()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock = true;
    }
    public void UnbindBlock(Scene arg0, Scene arg1)
    {
        blockInput.action.performed -= Block;
    }
}
