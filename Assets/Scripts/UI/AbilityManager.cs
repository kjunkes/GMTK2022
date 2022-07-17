using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityManager : MonoBehaviour
{
    public enum AbilityType
    {
        PUNCH,
        ROUNDHOUSE_KICK,
        STONE_THROW,
        SHOT,
        SELF_BUFF,
        ENEMY_DEFENSE_NERF
    }

    private GameLoop gameLoop;
    private PlayerEnergy playerEnergy;
    private PlayerHealth playerHealth;

    private AbilityType currentAbility;
    private bool abilityActive = false;
    private float attackMultiplier = 1f;

    public Button punchButton;
    public Button roundhouseKickButton;
    public Button stoneThrowButton;
    public Button shotButton;
    public Button selfBuffButton;
    public Button enemyDefenseNerfButton;

    // Start is called before the first frame update
    void Start()
    {
        playerEnergy = FindObjectOfType<PlayerEnergy>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        gameLoop = FindObjectOfType<GameLoop>();

        punchButton.onClick.AddListener(() => {
            SetAbility(AbilityType.PUNCH);
        });
        roundhouseKickButton.onClick.AddListener(() => {
            SetAbility(AbilityType.ROUNDHOUSE_KICK);
        });
        stoneThrowButton.onClick.AddListener(() => {
            SetAbility(AbilityType.STONE_THROW);
        });
        shotButton.onClick.AddListener(() => {
            SetAbility(AbilityType.SHOT);
        });
        selfBuffButton.onClick.AddListener(() => {
            SetAbility(AbilityType.SELF_BUFF);
        });
        enemyDefenseNerfButton.onClick.AddListener(() => {
            SetAbility(AbilityType.ENEMY_DEFENSE_NERF);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAbilitySelection()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void EndAbilityPhase()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        attackMultiplier = 1f;
        playerEnergy.attackRangeIndicator.gameObject.SetActive(false);
        gameLoop.IncrementTurnState();
    }

    //this is called by the buttons
    private void SetAbility(AbilityType type)
    {
        Ability ability = this.playerEnergy.GetAbilityOfType(type);

        if (this.playerEnergy.energy < ability.GetManacost())
        {
            return;
        }

        DamagingAbility damagingAbility = ability as DamagingAbility;
        DefenseNerfAbility defenseNerfAbility = ability as DefenseNerfAbility;

        if (damagingAbility != null || defenseNerfAbility != null)
        {
            float range = damagingAbility ? damagingAbility.range : defenseNerfAbility.range;
            playerEnergy.attackRangeIndicator.gameObject.SetActive(true);
            playerEnergy.attackRangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 1);
        }
        else
        {
            playerEnergy.attackRangeIndicator.gameObject.SetActive(false);
        }

        //sufficient Mana available
        currentAbility = type;
        abilityActive = true;

        BuffAbility buffAbility = ability as BuffAbility;
        AOEAbility aoeAbility = ability as AOEAbility;

        if (buffAbility != null || aoeAbility != null)
        {
            UseAbility();
        }
    }

    public void UseAbility()
    {
        if (!abilityActive)
        {
            return;
        }

        switch (currentAbility)
        {
            case AbilityType.PUNCH:
                UsePunch();
                break;
            case AbilityType.ROUNDHOUSE_KICK:
                UseRoundhouseKick();
                break;
            case AbilityType.STONE_THROW:
                UseStoneThrow();
                break;
            case AbilityType.SHOT:
                UseShot();
                break;
            case AbilityType.SELF_BUFF:
                UseSelfBuff();
                break;
            case AbilityType.ENEMY_DEFENSE_NERF:
                UseEnemyDefenseNerf();
                break;
            default:
                break;
        }
    }

    private void UsePunch()
    {
        DamagingAbility punch = (DamagingAbility)playerEnergy.GetAbilityOfType(AbilityType.PUNCH);
        UseDamagingAbility(punch);
    }

    private void UseRoundhouseKick()
    {
        AOEAbility aoeAbility = (AOEAbility)playerEnergy.GetAbilityOfType(AbilityType.ROUNDHOUSE_KICK);
        aoeAbility.Use(attackMultiplier);
        this.playerEnergy.energy -= aoeAbility.GetManacost();

        if (this.playerEnergy.energy <= 0)
        {
            EndAbilityPhase();
        }
    }

    private void UseStoneThrow()
    {
        DamagingAbility stoneThrow = (DamagingAbility)playerEnergy.GetAbilityOfType(AbilityType.STONE_THROW);
        UseDamagingAbility(stoneThrow);
    }

    private void UseShot()
    {
        DamagingAbility shot = (DamagingAbility)playerEnergy.GetAbilityOfType(AbilityType.SHOT);
        UseDamagingAbility(shot);
    }

    private void UseSelfBuff()
    {
        BuffAbility selfBuff = (BuffAbility)playerEnergy.GetAbilityOfType(AbilityType.SELF_BUFF);
        attackMultiplier = selfBuff.factor;
        this.playerEnergy.energy -= selfBuff.GetManacost();

        if (this.playerEnergy.energy <= 0)
        {
            EndAbilityPhase();
        }
    }

    private void UseEnemyDefenseNerf()
    {
        DefenseNerfAbility defenseNerf = (DefenseNerfAbility)playerEnergy.GetAbilityOfType(AbilityType.ENEMY_DEFENSE_NERF);
        UseDefenseNerfAbility(defenseNerf);
    }

    private void UseDamagingAbility(DamagingAbility ability)
    {
        EnemyHealth enemy = IsMouseOverEnemy();

        if (enemy != null)
        {
            if (ability.Use(enemy, attackMultiplier * enemy.GetDefenseNerf()))
            {
                this.playerEnergy.energy -= ability.GetManacost();

                if (this.playerEnergy.energy <= 0)
                {
                    EndAbilityPhase();
                }
            }
        }
    }

    private void UseDefenseNerfAbility(DefenseNerfAbility ability)
    {
        EnemyHealth enemy = IsMouseOverEnemy();

        if (enemy != null)
        {
            if (ability.Use(enemy, ability.defenseNerf))
            {
                this.playerEnergy.energy -= ability.GetManacost();

                if (this.playerEnergy.energy <= 0)
                {
                    EndAbilityPhase();
                }
            }
        }
    }

    //Checks if the mouse is currently hovering over Enemy
    private EnemyHealth IsMouseOverEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] raycastHits = Physics2D.GetRayIntersectionAll(ray, 1500f);

        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit2D curRaycastHit = raycastHits[i];

            if (curRaycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return curRaycastHit.collider.gameObject.GetComponent<EnemyHealth>();
            }
        }

        return null;
    }

    public float GetAttackMultiplier()
    {
        return this.attackMultiplier;
    }

    public void SetAttackMultiplier(float value)
    {
        this.attackMultiplier = value;
    }
}
