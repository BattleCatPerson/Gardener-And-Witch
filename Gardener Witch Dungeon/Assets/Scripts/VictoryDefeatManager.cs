using System;
using System.Collections;
using UnityEngine;

public class VictoryDefeatManager : MonoBehaviour
{
    public static VictoryDefeatManager Instance;
    public event Action gameEndEvent;
    public bool conditionChosen;
    [SerializeField] bool won;
    [SerializeField] Animator nextButton;
    [SerializeField] Animator playerCanvas;
    [SerializeField] Animator defeatAnimator;
    [SerializeField] float delayToNext;
    [SerializeField] float delayToGameOver;
    private void Awake()
    {
        Instance = this;
    }
    public void SelectCondition(bool win)
    {
        won = win;
        conditionChosen = true;
        if (win)
        {
            StartCoroutine(ShowNextButton());
        }
        else
        {
            StartCoroutine(GameOver());
        }
        gameEndEvent?.Invoke();
    }
    public IEnumerator ShowNextButton()
    {
        yield return new WaitForSeconds(delayToNext);
        nextButton.SetTrigger("Win");
        playerCanvas.SetTrigger("Fade");
    }
    public IEnumerator GameOver()
    {
        playerCanvas.SetTrigger("Fade");
        yield return new WaitForSeconds(delayToGameOver);
        defeatAnimator.SetTrigger("Lose");
    }
}
