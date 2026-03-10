using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    public static SkillHolder Instance;
    public List<EnemyHealth> enemies;
    public EnemyHealth targetedEnemy;
    public event Action<float> minigameReturnEvent;
    [SerializeField] List<Image> skillIcons;
    [SerializeField] List<PlantSkill> equippedSkills;
    [SerializeField] List<float> timers;
    [SerializeField] List<Image> cooldownIndicators;
    [SerializeField] float energy;
    [SerializeField] float maxEnergy;
    [SerializeField] Image energyBar;
    [SerializeField] float energyRechargeRate;
    [SerializeField] float energyBarAdjustTime;
    [SerializeField] bool barAdjusting;
    [SerializeField] List<InputActionReference> actions;
    [SerializeField] List<bool> onCooldown;
    [SerializeField] Transform targetMarker;
    [SerializeField] InputActionReference shiftTarget;
    [SerializeField] int targetIndex;
    Coroutine adjustCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < equippedSkills.Count; i++)
        {
            skillIcons[i].sprite = equippedSkills[i].sprite;
            timers[i] = equippedSkills[i].cooldown; // x is the current timer, y is the cooldown
            actions[i].action.performed += equippedSkills[i].AttemptUse;
        }
        shiftTarget.action.performed += ShiftTarget;
        adjustCoroutine = null;
        enemies = EnemyManager.Instance.enemies;
    }

    void Update()
    {
        energy += energyRechargeRate * Time.deltaTime;
        energy = Mathf.Min(energy, maxEnergy);
        barAdjusting = adjustCoroutine != null;
        if (!barAdjusting) energyBar.fillAmount = energy / maxEnergy;
        if (enemies.Count > 0)
        {
            targetedEnemy = enemies[targetIndex];
            targetMarker.position = targetedEnemy.transform.position;
        }
        else
        {
            targetedEnemy = null;
        }
        targetMarker.gameObject.SetActive(enemies.Count > 0);

    }
    public void StartTimer(PlantSkill skill)
    {
        int index = equippedSkills.IndexOf(skill);
        StartCoroutine(StartTimerCoroutine(index));
    }
    public IEnumerator StartTimerCoroutine(int index)
    {
        onCooldown[index] = true;
        float tracker = 0;
        while (tracker < timers[index])
        {
            tracker += Time.deltaTime;
            cooldownIndicators[index].fillAmount = 1 - (tracker / timers[index]);
            yield return null;
        }
        onCooldown[index] = false;
    }
    public bool CanUse(PlantSkill skill, float cost) => !onCooldown[equippedSkills.IndexOf(skill)] && energy >= cost;
    public void UseEnergy(float cost)
    {
        energy -= cost;
        if (adjustCoroutine != null) StopCoroutine(adjustCoroutine);
        adjustCoroutine = StartCoroutine(AdjustBar(energyBar.fillAmount));
    }
    public IEnumerator AdjustBar(float initialFill)
    {
        float timer = 0;
        barAdjusting = true;
        while (timer < energyBarAdjustTime)
        {
            timer += Time.deltaTime;
            energyBar.fillAmount = Mathf.Lerp(initialFill, energy / maxEnergy, timer / energyBarAdjustTime);
            yield return null;
        }
        adjustCoroutine = null;
    }
    public void ShiftTarget(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        int adjustment = value < 1 ? -1 : 1;
        targetIndex += adjustment;
        targetIndex %= enemies.Count;
        if (targetIndex < 0) targetIndex += enemies.Count;
    }
    public void StartMinigame(PlantSkill skill)
    {
        SceneManager.LoadScene(skill.sceneName, LoadSceneMode.Additive);
    }
    public void EndMinigame(float success)
    {
        minigameReturnEvent?.Invoke(success);
        minigameReturnEvent = null;
    }
}
