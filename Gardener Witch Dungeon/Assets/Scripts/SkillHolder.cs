using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class SkillHolder : MonoBehaviour
{
    [Serializable]
    public class UpgradeTypeToSkillList
    {
        public UpgradeType upgradeType;
        public List<PlantSkill> skills;
    }

    public static SkillHolder Instance;
    public PlayerHealth playerHealth;
    public List<EnemyHealth> enemies;
    public EnemyHealth targetedEnemy;
    public event Action<SkillResult> skillEvent;
    public static float savedEnergy = -1;
    [Header("UI")]
    //[SerializeField] List<Image> skillIcons;
    [SerializeField] List<UpgradeTypeToSkillList> skillLists;
    [SerializeField] List<PlantSkill> startingSkills;
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
    [SerializeField] Animator playerBarAnimator;
    [Header("Skill Use")]
    public Animator plantSkillAnimator;
    [SerializeField, Range(0f, 1f)] public float success; // change this in the animation 
    [SerializeField] SkillResult skillResult;
    [SerializeField] InputActionReference inputPositive;
    [SerializeField] InputActionReference inputNegative;
    [SerializeField] bool skillInUse;
    public bool canInput; // set this in animation
    [SerializeField] float moveToPositionTime;
    [SerializeField] Vector3 initialPos;
    public PlantSkill activeSkill;
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
        EquipSkills();

        // sets input action stuff
        shiftTarget.action.performed += ShiftTarget;
        useSkill.action.performed += StartMinigame;
        cancelSkill.action.performed += CancelSkill;
        inputPositive.action.performed += PositiveInput;
        inputNegative.action.performed += NegativeInput;
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

        if (savedEnergy < 0)
        {
            energy = maxEnergy;
        }
        else
        {
            energy = savedEnergy;
        }
        energyBar.fillAmount = energy / maxEnergy;
        initialPos = transform.position;
        SceneManager.activeSceneChanged += UnbindInputs;

        playerBarAnimator.SetTrigger("In");
    }

    void Update()
    {
        //energy += energyRechargeRate * Time.deltaTime;
        //energy = Mathf.Min(energy, maxEnergy);
        barAdjusting = adjustCoroutine != null;
        savedEnergy = energy;
        //if (!barAdjusting) energyBar.fillAmount = energy / maxEnergy;
        if (turnActive)
        {
            targetIndex = Mathf.Clamp(targetIndex, 0, enemies.Count - 1);
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
            int multiplier = TurnManager.Instance.timePaused || VictoryDefeatManager.Instance.conditionChosen ? 0 : 1;
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
        Debug.Log("shift target");
        if (enemies.Count <= 1 || !targeting) return;
        float value = context.ReadValue<float>();
        int adjustment = value < 1 ? -1 : 1;
        targetIndex += adjustment;
        if (targetIndex >= enemies.Count)
        {
            targetIndex = 0;
        }
        else if (targetIndex < 0)
        {
            targetIndex = enemies.Count - 1;
        }
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
        targeting = false;
        MoveTargetIndicators();
        List<Health> activeUnits = new();
        activeUnits.Add(targetedEnemy);
        activeUnits.Add(playerHealth);
        EnemyManager.Instance.BlurUnits(activeUnits);
        playerBarAnimator.SetTrigger("Out");
        StartCoroutine(MoveToPosition());
    }
    public void StartPlantAnimation()
    {
        selectedSkillInTurn = true;
        plantSkillAnimator.SetTrigger(selectedSkill.triggerName);
        UseEnergy(selectedSkill.energyCost);
        //SceneManager.LoadScene(selectedSkill.sceneName, LoadSceneMode.Additive);
    }
    public IEnumerator MoveToPosition()
    {
        float timer = 0;
        Vector3 finalPos = targetedEnemy.transform.position - Vector3.right * selectedSkill.distanceFromTarget;
        while (timer < moveToPositionTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPos, finalPos, timer / moveToPositionTime);
            yield return null;
        }
        StartPlantAnimation();
    }
    public IEnumerator ReturnToInitialPosition()
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < moveToPositionTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, initialPos, timer / moveToPositionTime);
            yield return null;
        }
        ConfirmTurnEnd();   
    }
    public void MoveTargetIndicators()
    {
        foreach (Transform t in targetMarkerInstances)
        {
            t.position = targetMarkerParent.position;
        }
    }
    //public void EndMinigame(float success)
    //{
    //    skillEvent?.Invoke(success);
    //    skillEvent = null;
    //    UseEnergy(selectedSkill.energyCost);
    //    selectedSkill = null;
    //    EndTurn();
    //}
    public void StartTurn()
    {
        turnActive = true;
        TurnManager.Instance.timePaused = true;
        skillPanelAnimator.SetTrigger("In");
    }
    public void EndTurn()
    {
        EnemyManager.Instance.Unblur();
        StartCoroutine(ReturnToInitialPosition());
    }
    public void ConfirmTurnEnd()
    {
        turnActive = false;
        TurnManager.Instance.timePaused = false;
        selectedSkillInTurn = false;
        turnTimer = 0;
        playerBarAnimator.SetTrigger("In");
    }
    //public void UseSkill(InputAction.CallbackContext context)
    //{
    //    if (canInput)
    //    {
    //        skillResult = selectedSkill.GetSkillResult(success);
    //        //plantSkillAnimator.SetTrigger(skillResult.ToString());
    //        if (skillResult == SkillResult.Fail)
    //        {
    //            plantSkillAnimator.SetTrigger("Fail");
    //        }
    //        else
    //        {
    //            plantSkillAnimator.SetTrigger("Use");
    //        }
    //    }
    //    //if (skillInUse)
    //    //{
    //    //    skillEvent?.Invoke(success);
    //    //}
    //}
    public void PositiveInput(InputAction.CallbackContext context)
    {
        if (activeSkill != null)
        {
            activeSkill.PositiveListener();
        }
    }
    public void NegativeInput(InputAction.CallbackContext context)
    {
        if (activeSkill != null)
        {
            activeSkill.NegativeListener();
        }
    }
    public void InvokePlantSkillEvent() //use in animation
    {
        //skillEvent?.Invoke(skillResult);
        activeSkill.AnimationListener();
        Debug.Log("USE SKILL");
    }
    public void EquipSkills()
    {   foreach (PlantSkill skill in startingSkills)
        {
            SkillSelectionButton s = Instantiate(skillButtonPrefab, skillPanel);
            PlantSkill skillInstance = Instantiate(skill, s.transform);
            s.SetValues(skillInstance);
        }
        //foreach (var skillList in skillLists)
        //{
        //    int upgradeCount = StatManager.upgrades[skillList.upgradeType];
        //    for (int i = 0; i < upgradeCount; i++)
        //    {
        //        SkillSelectionButton s = Instantiate(skillButtonPrefab, skillPanel);
        //        PlantSkill skillInstance = Instantiate(skillList.skills[i], s.transform);
        //        s.SetValues(skillInstance);
        //    }
        //}
        // sets size of skill panel based on how many skills you have
        skillPanel.sizeDelta = new(skillPanel.sizeDelta.x, Mathf.Max(skillPanel.sizeDelta.y, startingSkills.Count * layoutGroup.cellSize.y));
    }
    public void UnbindInputs(Scene arg0, Scene arg1)
    {
        shiftTarget.action.performed -= ShiftTarget;
        useSkill.action.performed -= StartMinigame;
        cancelSkill.action.performed -= CancelSkill;
        inputPositive.action.performed -= PositiveInput;
        inputNegative.action.performed -= NegativeInput;
        SceneManager.activeSceneChanged -= UnbindInputs;
    }
}
