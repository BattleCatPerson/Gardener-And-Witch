using System.Collections;
using UnityEngine;

public class VineWhip : PlantSkill
{
    public enum State
    {
        Start, Down, Up, End
    }

    [SerializeField] float damage;
    [SerializeField] float timeLimit;
    [SerializeField] float timer;
    [SerializeField] State state;
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
        else if (state != State.Start)
        {
            state = State.Start;
            skillHolder.plantSkillAnimator.SetTrigger("End");
            Debug.Log("end");
        }
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeLimit);
        skillHolder.plantSkillAnimator.SetTrigger("End");
        state = State.End;
    }
    public void Damage()
    {
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage);
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
        }
        else if (state == State.Up)
        {
            Damage();
            state = State.Down;
        }
        else if (state == State.Down)
        {
            Damage();
            state = State.Up;
        }
        //else if (state == State.End)
        //{
        //    state = State.Start;
        //}
    }

    public override void NegativeListener()
    {
        if (state == State.Up && skillHolder.canInput)
        {
            skillHolder.plantSkillAnimator.SetTrigger("Right");
        }
    }

    public override void PositiveListener()
    {
        if (state == State.Down && skillHolder.canInput)
        {
            skillHolder.plantSkillAnimator.SetTrigger("Left");
        }
    }
}
