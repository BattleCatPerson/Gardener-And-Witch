using UnityEngine;

public class AnimatorSpeedTest : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Range(0, 100), SerializeField] public float speed;
    void Start()
    {
        
    }

    void Update()
    {
        animator.speed = speed; 
    }
}
