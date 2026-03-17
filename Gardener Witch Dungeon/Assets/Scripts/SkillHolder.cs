using JetBrains.Annotations;
using NUnit.Framework;
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
    [Header("UI")]
    //[SerializeField] List<Image> skillIcons;
    [SerializeField] List<PlantSkill> equippedSkills;
    [SerializeField] SkillSelectionButton skillButtonPrefab;
    [SerializeField] RectTransform skillPanel;
    [SerializeField] GridLayoutGroup layoutGroup;
    //[SerializeField] List<float> timers;
    //[SerializeField] List<Image> cooldownIndicators;
    [Header("Energy")]
    [SerializeField] float energy;
    [SerializeField] float maxEnergy;
    [SerializeField] Image energyBar;
    //[SerializeField] float energyRechargeRate;
    [SerializeField] float energyBarAdjustTime;
    [SerializeField] bool barAdjusting;
    //[SerializeField] List<InputActionReference> actions;
    //[SerializeField] List<bool> onCooldown;
    [Header("Targeting")]
    [SerializeField] Transform targetMarkerPrefab;
    [SerializeField] List<Transform> targetMarkerInstances;
    [SerializeField] Transform targetMarkerParent; // position this out of camera view
    [SerializeField] InputActionReference shiftTarget;
    [SerializeField] int targetIndex;
    [SerializeField] bool targeting;
    [SerializeField] TargetType targetType;
    [SerializeField] PlantSkill selectedSkill;
    [SerializeField] InputActionReference useSkill;
    [SerializeField] InputActionReference cancelSkill;
    [SerializeField] Animator skillPanelAnimator;
    [Header("Turn Tracker")]
    [SerializeField] Image turnBar;
    [SerializeField] float turnTimer;
    [SerializeField] float maxTurnTime;
    [SerializeField] bool turnActive;
    [SerializeField] bool selectedSkillInTurn;

    Coroutine adjustCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < equippedSkills.Count; i++)
        {
            //skillIcons[i].sprite = equippedSkills[i].sprite;
            //timers[i] = equippedSkills[i].cooldown; // x is the current timer, y is the cooldown
            //actions[i].action.performed += equippedSkills[i].AttemptUse;

            // initializes stuff
            SkillSelectionButton s = Instantiate(skillButtonPrefab, skillPanel);
            s.SetValues(equippedSkills[i]);
        }
        // sets size of skill panel based on how many skills you have
        skillPanel.sizeDelta = new(skillPanel.sizeDelta.x, Mathf.Max(skillPanel.sizeDelta.y, equippedSkills.Count * layoutGroup.cellSize.y));

        // sets input action stuff
        shiftTarget.action.performed += ShiftTarget;
        useSkill.action.performed += StartMinigame;
        cancelSkill.action.performed += CancelSkill;

        adjustCoroutine = null;

        // sets targeting indicators
        enemies = EnemyManager.Instance.enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            Transform t = Instantiate(targetMarkerPrefab, targetMarkerParent);
            targetMarkerInstances.Add(t);
        }
        MoveTargetIndicators();
        turnTimer = maxTurnTime;
        StartTurn();

        energy = maxEnergy;
    }

    void Update()
    {
        //energy += energyRechargeRate * Time.deltaTime;
        //energy = Mathf.Min(energy, maxEnergy);
        barAdjusting = adjustCoroutine != null;
        //if (!barAdjusting) energyBar.fillAmount = energy / maxEnergy;
        if (turnActive)
        {
            if (enemies.Count > 0)
            {
                targetedEnemy = enemies[targetIndex];
            }
            else
            {
                targetedEnemy = null;
            }
            if (targeting)
            {
                switch (targetType)
                {
                    case TargetType.singleEnemy:
                        targetMarkerInstances[0].position = targetedEnemy.targetPosition.position; // put on the selected enemy
                        break;
                    case TargetType.allEnemies:
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            targetMarkerInstances[i].position = enemies[i].targetPosition.position; // put on all enemies
                        }
                        break;
                    case TargetType.player:
                        targetMarkerInstances[0].position = transform.position; // put on player
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            int multiplier = TurnManager.Instance.timePaused ? 0 : 1;
            turnTimer += Time.deltaTime * multiplier;
            if (turnTimer >= maxTurnTime)
            {
                StartTurn();
            }
        }
        turnBar.fillAmount = turnTimer / maxTurnTime;
    }
    //public void StartTimer(PlantSkill skill)
    //{
    //    int index = equippedSkills.IndexOf(skill);
    //    StartCoroutine(StartTimerCoroutine(index));
    //}
    //public IEnumerator StartTimerCoroutine(int index)
    //{
    //    onCooldown[index] = true;
    //    float tracker = 0;
    //    while (tracker < timers[index])
    //    {
    //        tracker += Time.deltaTime;
    //        cooldownIndicators[index].fillAmount = 1 - (tracker / timers[index]);
    //        yield return null;
    //    }
    //    onCooldown[index] = false;
    //}
    public bool CanUse(PlantSkill skill, float cost) => !selectedSkillInTurn && !targeting && turnActive && energy >= cost;
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
        energyBar.fillAmount = energy / maxEnergy;
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
    public void StartTargeting(PlantSkill skill)
    {
        selectedSkill = skill;
        targeting = true;
        skillPanelAnimator.SetTrigger("Out");
    }
    public void CancelSkill(InputAction.CallbackContext context)
    {
        if (!targeting) return;
        selectedSkill = null;
        targeting = false;
        MoveTargetIndicators();
        skillPanelAnimator.SetTrigger("In");
    }
    public void StartMinigame(InputAction.CallbackContext context)
    {
        if (!targeting) return;
        selectedSkillInTurn = true;
        SceneManager.LoadScene(selectedSkill.sceneName, LoadSceneMode.Additive);
        targeting = false;
        MoveTargetIndicators();
    }
    public void MoveTargetIndicators()
    {
        foreach (Transform t in targetMarkerInstances)
        {
            t.position = targetMarkerParent.position;
        }
    }
    public void EndMinigame(float success)
    {
        minigameReturnEvent?.Invoke(success);
        minigameReturnEvent = null;
        UseEnergy(selectedSkill.energyCost);
        selectedSkill = null;
        EndTurn();
    }
    public void StartTurn()
    {
        turnActive = true;
        TurnManager.Instance.timePaused = true;
        skillPanelAnimator.SetTrigger("In");
    }
    public void EndTurn()
    {
        turnActive = false;
        TurnManager.Instance.timePaused = false;
        selectedSkillInTurn = false;
        turnTimer = 0;
    }
}
