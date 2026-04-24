using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHandler : MonoBehaviour
{
    [SerializeField] InputActionReference mouse;
    [SerializeField] InputActionReference click;
    [SerializeField] EventOnClick hoveredObject;
    EventOnClick hoveredObjectClick;
    void Start()
    {
        click.action.started += InitializeClick;
        click.action.canceled += ClickRelease;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mouse.action.ReadValue<Vector2>()), Vector2.zero);
        if (hit && hit.collider.TryGetComponent<EventOnClick>(out EventOnClick e))
        {
            hoveredObject = e;
        }
        else hoveredObject = null;
    }

    public void InitializeClick(InputAction.CallbackContext context)
    {
        if (hoveredObject)
        {
            hoveredObjectClick = hoveredObject;
        }
    }
    public void ClickRelease(InputAction.CallbackContext context)
    {
        if (hoveredObject && hoveredObject == hoveredObjectClick)
        {
            hoveredObject.Click();
        }
    }
}
