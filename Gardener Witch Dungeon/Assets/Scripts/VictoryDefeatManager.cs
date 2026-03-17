using System.Collections;
using UnityEngine;

public class VictoryDefeatManager : MonoBehaviour
{
    public static VictoryDefeatManager Instance;
    public bool conditionChosen;
    [SerializeField] bool won;
    [SerializeField] Animator nextButton;
    [SerializeField] Animator playerCanvas;
    [SerializeField] float delayToNext;
    private void Awake()
    {
        Instance = this;
    }
    public void Win()
    {
        conditionChosen = true;
        won = true;
        StartCoroutine(ShowNextButton());
    }
    public void Lose()
    {
        conditionChosen = true;
        won = false;
    }
    public IEnumerator ShowNextButton()
    {
        yield return new WaitForSeconds(delayToNext);
        nextButton.SetTrigger("Win");
    }
}
