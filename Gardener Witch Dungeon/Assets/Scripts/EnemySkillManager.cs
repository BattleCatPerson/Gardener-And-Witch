using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public static bool enemyAttacking = false;
    [SerializeField] EnemyHealth health;
    [SerializeField] List<EnemySkill> skillList;
    [SerializeField] float timeBetweenSkills;
    [SerializeField] float timer;
    [SerializeField] float variance;
    [SerializeField] Animator animator;
    [SerializeField] EnemySkill selectedSkill;
    [SerializeField] bool attacking;
    [SerializeField] float moveToPositionTime;
    [SerializeField] Vector3 initialPos;
    void Start()
    {
        //StartCoroutine(SelectSkill());
        initialPos = transform.position;
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
                StartSkill();
                TurnManager.Instance.timePaused = true;
                // start animations and stuff
                timer = 0;
            }
        }

    }
    public void StartSkill()
    {
        StartCoroutine(MoveToPosition());
        List<Health> activeUnits = new();
        activeUnits.Add(EnemyManager.Instance.playerHealth);
        activeUnits.Add(health);
        EnemyManager.Instance.BlurUnits(activeUnits);
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
        StartCoroutine(ReturnToInitialPosition());
    }

    public IEnumerator MoveToPosition()
    {
        float timer = 0;
        Vector3 finalPos = EnemyManager.Instance.playerHealth.transform.position + Vector3.right * selectedSkill.distanceFromPlayer;
        while (timer < moveToPositionTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, finalPos, timer / moveToPositionTime);
            yield return null;
        }
        animator.SetTrigger(selectedSkill.skillName);
    }
    public IEnumerator ReturnToInitialPosition()
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < moveToPositionTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, initialPos, timer / moveToPositionTime);
            yield return null;
        }
        enemyAttacking = false;
        TurnManager.Instance.timePaused = false;
        EnemyManager.Instance.Unblur();
    }
}
