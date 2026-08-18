using System.Collections;
using UnityEngine;

public class VineWhip : PlantSkill
{
    public enum State
    {
        Start, MovingDown, Down, MovingUp, Up, End
    }

    [SerializeField] float damage;
    [SerializeField] float timeLimit;
    [SerializeField] float timer;
    [SerializeField] State state;
    [SerializeField] bool ended;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if ((timer <= 0 || skillHolder.enemies.Count == 0) && state != State.Start)
        {
            state = State.Start;
            skillHolder.plantSkillAnimator.SetTrigger("End");
            skillHolder.activeSkill = null;
            ended = true;
            Debug.Log("end");
        }
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeLimit);
        skillHolder.plantSkillAnimator.SetTrigger("End");
    }
    public void Damage()
    {
        foreach (EnemyHealth e in skillHolder.enemies)
        {
            e.TakeDamage(damage);
        }
    }
    public override void AnimationListener()
    {
        Debug.Log(state);
        if (state == State.Start)
        {
            Debug.Log("whip start");
            if (timer <= 0)
            {
                timer = timeLimit;
            }
            state = State.Up;
            ended = false;
        }
        //else if (state == State.Up)
        //{
        //    Damage();
        //    state = State.Down;
        //}
        //else if (state == State.Down)
        //{
        //    Damage();
        //    state = State.Up;
        //}
        //else if (state == State.End)
        //{
        //    state = State.Start;
        //}
    }

    public override void NegativeListener()
    {
        if (state == State.Up && skillHolder.canInput && !ended && skillHolder.activeSkill == this)
        {
            skillHolder.plantSkillAnimator.SetTrigger("Right");
            state = State.Down;
            Damage();
        }
    }

    public override void PositiveListener()
    {
        if (state == State.Down && skillHolder.canInput && !ended && skillHolder.activeSkill == this)
        {
            skillHolder.plantSkillAnimator.SetTrigger("Left");
            state = State.Up;
            Damage();
        }
    }
}
