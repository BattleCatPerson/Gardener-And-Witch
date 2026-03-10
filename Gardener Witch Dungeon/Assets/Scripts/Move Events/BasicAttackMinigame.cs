using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BasicAttackMinigame : MonoBehaviour
{
    [SerializeField] RectTransform image;
    [SerializeField] RectTransform imageTarget;
    [SerializeField] float targetScale;
    [SerializeField] float maxScale;
    [SerializeField] float totalTime;
    [SerializeField] float targetTime;
    [SerializeField] InputActionReference input;
    [SerializeField] bool inputRegistered;
    [SerializeField] float success;
    [SerializeField] float returnTime;
    [SerializeField] String sceneName;
    Coroutine coroutine;
    float maxError;
    void Start()
    {
        targetTime = targetScale / maxScale;
        imageTarget.sizeDelta = new(targetScale, targetScale);
        input.action.performed += Input;
        coroutine = StartCoroutine(Minigame());
        maxError = (maxScale - targetScale) / targetScale;
    }

    void Update()
    {
        
    }
    public IEnumerator Minigame()
    {
        float timer = 0;
        while (timer < totalTime)
        {
            timer += Time.unscaledDeltaTime;
            float size = Mathf.Lerp(maxScale, targetScale, timer / totalTime);
            image.sizeDelta = new(size, size);
            yield return null;
        }
        success = 0;
        StartCoroutine(ReturnToGameplay());
    }
    public void Input(InputAction.CallbackContext context)
    {
        if (!inputRegistered)
        {
            inputRegistered = true;
            StopCoroutine(coroutine);
            float currentScale = image.sizeDelta.x;
            success = 1 - ((Mathf.Abs(image.sizeDelta.x - targetScale) / targetScale) / maxError);
            StartCoroutine(ReturnToGameplay());
        }
    }
    public IEnumerator ReturnToGameplay()
    {
        yield return new WaitForSeconds(returnTime);
        input.action.performed -= Input;
        SkillHolder.Instance.EndMinigame(success);
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
