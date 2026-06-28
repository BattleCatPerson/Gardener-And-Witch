using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public abstract class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool dead;
    public Image healthbar;
    public bool barAdjusting;
    public float healthbarAdjustTime;
    public Transform targetPosition;
    public Animator animator;
    public string flashTrigger;
    public float damageMultiplier = 1f;
    public bool inCombat; // use to blur
    public SpriteRenderer sprite;
    Coroutine adjustCoroutine;
    public void Initialize()
    {
        //maxHealth = health;
        adjustCoroutine = null;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage * damageMultiplier;
        health -= finalDamage;
        health = Mathf.Max(health, 0);
        dead = health <= 0;
        if (!dead && finalDamage > 0)
        {
            animator.SetTrigger(flashTrigger);
        }
        if (adjustCoroutine != null) StopCoroutine(adjustCoroutine);
        adjustCoroutine = StartCoroutine(AdjustBar(healthbar.fillAmount));
    }
    public void AdjustBar()
    {
        barAdjusting = adjustCoroutine != null;
        if (!barAdjusting) healthbar.fillAmount = (float)health / maxHealth;
    }
    public IEnumerator AdjustBar(float initialFill)
    {
        float timer = 0;
        barAdjusting = true;
        while (timer < healthbarAdjustTime)
        {
            timer += Time.deltaTime;
            healthbar.fillAmount = Mathf.Lerp(initialFill, (float)health / maxHealth, timer / healthbarAdjustTime);
            yield return null;
        }
        adjustCoroutine = null;
    }
    public void Blur(bool enabled)
    {
        animator.SetBool("Blur", enabled);
    }
}
