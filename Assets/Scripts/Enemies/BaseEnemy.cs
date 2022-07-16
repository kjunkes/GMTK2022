using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Ability[] abilities;
    private ActionToken actionToken;
    private EnemyMovement movement;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        abilities = transform.GetComponents<Ability>();
        actionToken = transform.GetComponent<ActionToken>();
        movement = transform.GetComponent<EnemyMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actionToken.GetHasToken() && movement.GetRoute().Count == 0)
        {
            //choose ability to use this round
            int index = Random.Range(0, abilities.Length - 1);
            Ability ability = abilities[index];

            if(ability is DamagingAbility)
            {
                //calculate distance to player
                Vector2Int playerPosition = new Vector2Int(Mathf.RoundToInt(playerHealth.transform.position.x), Mathf.RoundToInt(playerHealth.transform.position.y));
                Vector2Int position = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                int distance = Mathf.RoundToInt(Vector2Int.Distance(playerPosition, position));

                if(distance > ((DamagingAbility)ability).range)
                {
                    if (!this.actionToken.GetHasMovedThisTurn())
                    {
                        //move if distance is too great and move has not yet occurred
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
                    ((DamagingAbility)ability).Use(playerHealth);
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
