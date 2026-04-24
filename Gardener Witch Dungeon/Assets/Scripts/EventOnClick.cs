using UnityEngine;
using UnityEngine.Events;

public class EventOnClick : MonoBehaviour
{
    [SerializeField] UnityEvent clickEvent;
    public void Click() => clickEvent?.Invoke();
}
