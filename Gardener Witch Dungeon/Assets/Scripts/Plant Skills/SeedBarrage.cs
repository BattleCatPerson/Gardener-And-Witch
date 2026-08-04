using UnityEngine;

public class SeedBarrage : PlantSkill
{
    public enum Phase
    {
        Initial, Choosing, Listening, Shooting, Ending, ShootAnim
    }
    public enum Direction
    {
        Left, Right
    }

    [SerializeField] float damage;
    [SerializeField] float goodDamageMult;
    [SerializeField] float okDamageMult;
    [SerializeField] float badDamageMult;
    [SerializeField] Phase phase;
    [SerializeField] Direction direction;
    [SerializeField] int maxShotCount;
    [SerializeField] int shotsFired;
    private void Start()
    {
        phase = Phase.Initial;
        skillHolder = SkillHolder.Instance;
    }
    public void Use()
    {
        Debug.Log("seed barrage hit!");
        SkillHolder.Instance.targetedEnemy.TakeDamage(damage);
        shotsFired++;
        phase = Phase.ShootAnim;
    }
    public override void PositiveListener()
    {
        if (phase == Phase.Listening)
        {
            if (direction == Direction.Left)
            {
                skillHolder.plantSkillAnimator.SetTrigger("Use");
                phase = Phase.Shooting;
            }
            else
            {
                skillHolder.plantSkillAnimator.SetTrigger("Fail");
                //phase = Phase.Ending;
            }
        }
    }

    public override void NegativeListener()
    {
        if (phase == Phase.Listening)
        {
            if (direction == Direction.Right)
            {
                skillHolder.plantSkillAnimator.SetTrigger("Use");
                phase = Phase.Shooting;
            }
            else
            {
                skillHolder.plantSkillAnimator.SetTrigger("Fail");
                //phase = Phase.Ending;
            }
        }
    }
    public override void AnimationListener()
    {
        if (phase == Phase.Initial)
        {
            ChooseRandom();
        }
        else if (phase == Phase.Listening)
        {
            phase = Phase.Ending; // use in fail animation
        }
        else if (phase == Phase.Shooting)
        {
            Use();
        }
        else if (phase == Phase.Ending)
        {
            phase = Phase.Initial;
            shotsFired = 0;
        }
        else if (phase == Phase.ShootAnim)
        {
            if (skillHolder.targetedEnemy != null && shotsFired < maxShotCount)
            {
                ChooseRandom();
            }
            else
            {
                skillHolder.plantSkillAnimator.SetTrigger("End");
                phase = Phase.Ending;
            }
        }
    }
    public void ChooseRandom()
    {
        int value = Random.Range(0, 2);
        if (value == 0)
        {
            SkillHolder.Instance.plantSkillAnimator.SetTrigger("Left");
            direction = Direction.Left;
        }
        else
        {
            SkillHolder.Instance.plantSkillAnimator.SetTrigger("Right");
            direction = Direction.Right;
        }
        Debug.Log("YOOOOOOOOOOOOOO");
        phase = Phase.Listening;
    }
}
