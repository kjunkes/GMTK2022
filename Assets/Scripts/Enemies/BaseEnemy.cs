using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int PERCEPTION_RANGE = 4;

    private Ability[] abilities;
    private ActionToken actionToken;
    private EnemyMovement movement;
    private PlayerHealth playerHealth;

    private Ability abilityThisTurn;

    private List<BaseEnemy> otherEnemiesInGroup = new List<BaseEnemy>();

    private bool aggroed = false;
    private float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        abilities = transform.GetComponents<Ability>();
        actionToken = transform.GetComponent<ActionToken>();
        movement = transform.GetComponent<EnemyMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();

        BaseEnemy[] siblings = transform.parent.GetComponentsInChildren<BaseEnemy>();

        foreach(BaseEnemy enemy in siblings)
        {
            if(enemy != this)
            {
                otherEnemiesInGroup.Add(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actionToken.GetHasToken())
        {
            UpdatePlayerDistance();
            ManageAggro();//passes back token in case of no aggro on this enemy

            if (movement.GetRoute().Count == 0)
            {
                if (abilityThisTurn is DamagingAbility)
                {
                    if (distanceFromPlayer > ((DamagingAbility)abilityThisTurn).range)
                    {
                        if (!this.actionToken.GetHasMovedThisTurn())
                        {
                            //move if distance is too great and move has not yet occurred
                            Vector2Int playerPosition = new Vector2Int(Mathf.RoundToInt(playerHealth.transform.position.x), Mathf.RoundToInt(playerHealth.transform.position.y));
                            movement.InitiateMove(playerPosition);
                            this.actionToken.SetHasMovedThisTurn(true);
                        }
                        else
                        {
                            //if distance is still too great after the move has been executed, give back actiontoken
                            this.actionToken.EndAction();
                        }
                    }
                    else
                    {
                        //if damaging ability has been chosen and player is in range, use the ability
                        ((DamagingAbility)abilityThisTurn).Use(playerHealth);
                        this.actionToken.EndAction();
                    }
                }
                else
                {
                    //Non damaging ability usage
                }
            }
        }
    }

    public void ChooseAbility()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        abilityThisTurn = abilities[UnityEngine.Random.Range(0, abilities.Length - 1)];
    }

    public void ManageAggro()
    {
        if (!aggroed)
        {
            if (distanceFromPlayer < PERCEPTION_RANGE)
            {
                aggroed = true;
            }
        }

        if (aggroed)
        {
            foreach(BaseEnemy enemy in otherEnemiesInGroup)
            {
                enemy.SetAggroed(true);
            }
        }
        else
        {
            this.actionToken.EndAction();
        }
    }

    public bool GetAggroed()
    {
        return this.aggroed;
    }

    public void SetAggroed(bool value)
    {
        this.aggroed = value;
    }

    public float GetDistanceFromPlayer()
    {
        return this.distanceFromPlayer;
    }

    public void UpdatePlayerDistance()
    {
        this.distanceFromPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);
    }
}
