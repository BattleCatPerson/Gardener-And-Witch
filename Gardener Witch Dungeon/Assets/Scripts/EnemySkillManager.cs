using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public static bool enemyAttacking = false;
    [SerializeField] List<EnemySkill> skillList;
    [SerializeField] float timeBetweenSkills;
    [SerializeField] float timer;
    [SerializeField] float variance;
    [SerializeField] Animator animator;
    [SerializeField] EnemySkill selectedSkill;
    [SerializeField] bool attacking;
    void Start()
    {
        //StartCoroutine(SelectSkill());
    }

    void Update()
    {
        float multiplier = TurnManager.Instance.timePaused || VictoryDefeatManager.Instance.conditionChosen ? 0 : 1;
        if (!TurnManager.Instance.timePaused)
        {
            timer += Time.deltaTime * multiplier;
            if (timer >= timeBetweenSkills)
            {
                //SelectRandomSkill();
                selectedSkill = skillList[Random.Range(0, skillList.Count)];
                enemyAttacking = true;
                animator.SetTrigger(selectedSkill.skillName);
                TurnManager.Instance.timePaused = true;
                // start animations and stuff
                timer = 0;
            }
        }

    }
    //public IEnumerator SelectSkill()
    //{
    //    float timer = 0;
    //    while (timer < timeBetweenSkills)
    //    {
    //        float multiplier = 1;
    //        //float multiplier = TurnManager.Instance.timePaused ? 0 : 1;
    //        timer += Time.deltaTime * multiplier;
    //        yield return null;
    //    }
    //}
    // call these in animations
    public void UseSkill()
    {
        //TurnManager.Instance.timePaused = true;
        selectedSkill.Use();
        timeBetweenSkills = selectedSkill.skillCooldown;
        timeBetweenSkills += Random.Range(-variance, variance);
        //StartCoroutine(SelectSkill());
    }
    public void EndSkill()
    {
        enemyAttacking = false;
        TurnManager.Instance.timePaused = false;
    }
}
