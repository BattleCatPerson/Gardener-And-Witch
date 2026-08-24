using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotationSpeedTest : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 initialMousePos;
    [SerializeField] Vector2 currentMousePos;
    [SerializeField] float initialAngle;
    [SerializeField] float lastAngle;
    [SerializeField] float angle;
    [SerializeField] float angleDelta;
    [SerializeField] Transform center;
    [SerializeField] Vector2 centerScreenPos;
    [SerializeField] float totalAngle;
    [SerializeField] AnimatorSpeedTest animatorSpeedTest;
    void Start()
    {
        centerScreenPos = Camera.main.WorldToScreenPoint(center.position);
        initialMousePos = Mouse.current.position.value;
    }

    void Update()
    {
        currentMousePos = Mouse.current.position.value;

        angle = Mathf.Atan2(currentMousePos.y - centerScreenPos.y, currentMousePos.x - centerScreenPos.x) * Mathf.Rad2Deg;
        //if (angle < 0 && lastAngle < 0 || angle > 0 && lastAngle > 0)
        //{
        //    if (angle < lastAngle) direction = 1;
        //    else if (angle > lastAngle) direction = -1;
        //}
        if (lastAngle < 0 && angle < 0 || lastAngle > 0 && angle > 0)
        {
            angleDelta = angle - lastAngle;
        }
        totalAngle += angleDelta;
        animatorSpeedTest.speed += Mathf.Abs(angleDelta) * Time.deltaTime;
        lastAngle = angle;
    }
}
