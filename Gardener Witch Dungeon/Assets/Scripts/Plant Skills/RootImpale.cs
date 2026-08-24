using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RootImpale : PlantSkill
{
    [SerializeField] float damage;
    [SerializeField] bool started;
    [SerializeField] float duration;
    [SerializeField] float timer;
    [SerializeField] Vector2 center;
    [SerializeField] Vector2 currentMousePos;
    [SerializeField] float angleDelta;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxAnimatorSpeed;
    [SerializeField] float angle;
    [SerializeField] float lastAngle;
    void Start()
    {
        Initialize();
        center = new(Screen.width / 2, Screen.height / 2);
    }

    void Update()
    {
        if (started)
        {
            if (timer > 0 && skillHolder.targetedEnemy != null)
            {
                timer -= Time.deltaTime;
                currentMousePos = Mouse.current.position.value;
                angle = Mathf.Atan2(currentMousePos.y - center.y, currentMousePos.x - center.x) * Mathf.Rad2Deg;
                //if (angle < 0 && lastAngle < 0 || angle > 0 && lastAngle > 0)
                //{
                //    if (angle < lastAngle) direction = 1;
                //    else if (angle > lastAngle) direction = -1;
                //}
                if (lastAngle < 0 && angle < 0 || lastAngle > 0 && angle > 0)
                {
                    angleDelta = angle - lastAngle;
                }
                skillHolder.plantSkillAnimator.speed += Mathf.Clamp(Mathf.Abs(angleDelta) * Time.deltaTime, 0, maxSpeed * Time.deltaTime);
                skillHolder.plantSkillAnimator.speed = Mathf.Clamp(skillHolder.plantSkillAnimator.speed, 1, maxAnimatorSpeed);
                lastAngle = angle;
            }
            else
            {
                End();
            }
        }

    }
    public void End()
    {
        timer = 0;
        started = false;
        skillHolder.plantSkillAnimator.speed = 1;
        skillHolder.plantSkillAnimator.SetTrigger("End");
    }
    public override void AnimationListener()
    {
        if (!started)
        {
            timer = duration;
            started = true;
        }
        else
        {
            skillHolder.targetedEnemy.TakeDamage(damage);
        }
    }

    public override void NegativeListener()
    {
    }

    public override void PositiveListener()
    {
    }

}
