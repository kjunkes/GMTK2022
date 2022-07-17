using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameLoop : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI playerActionStateText;

    public enum Turn {
        PLAYER,
        ENEMY
    }

    public enum PlayerActionState
    {
        DICE_SELECTION,
        WALKING,
        ACTION
    }

    public GameObject enemies;
    public Button resetButton;

    public string nextLevel;

    private PlayerDice playerDice;
    private PlayerHealth playerHealth;
    private PlayerEnergy playerEnergy;
    private AbilityManager abilityManager;
    private EndTurnButton endTurnButton;

    //List of all those enemies that are yet to act in the current turn
    private List<ActionToken> idleEnemies = new List<ActionToken>();

    //List of Objects that need to be enabled and reset in case of player death
    private List<Health> resetList = new List<Health>();

    //Game State
    private Turn currentTurn = Turn.PLAYER;
    private PlayerActionState playerActionState = PlayerActionState.DICE_SELECTION;

    // Start is called before the first frame update
    void Start()
    {
        playerDice = FindObjectOfType<PlayerDice>();
        endTurnButton = FindObjectOfType<EndTurnButton>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerEnergy = FindObjectOfType<PlayerEnergy>();
        abilityManager = FindObjectOfType<AbilityManager>();

        resetButton.onClick.AddListener(ReloadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTurn == Turn.ENEMY)
        {
            turnText.text = "ENEMY";
            playerActionStateText.text = "";
        } else
        {
            turnText.text = "PLAYER";

            if (playerActionState == PlayerActionState.DICE_SELECTION)
            {
                playerActionStateText.text = "DICE_SELECTION";
            }
            else if (playerActionState == PlayerActionState.WALKING)
            {
                playerActionStateText.text = "WALKING";
            }
            else
            {
                playerActionStateText.text = "ACTION";
            }
        }
    }

    public void IncrementTurnState()
    {
        switch (this.currentTurn)
        {
            case Turn.PLAYER:
                switch (this.playerActionState)
                {
                    case PlayerActionState.DICE_SELECTION:
                        this.playerActionState = PlayerActionState.WALKING;
                        this.endTurnButton.enabled = true;
                        return;
                    case PlayerActionState.WALKING:
                        if (playerEnergy.energy == 0)
                        {
                            this.currentTurn = Turn.ENEMY;

                            UpdateIdleEnemies();

                            if (this.idleEnemies.Count > 0)
                            {
                                this.idleEnemies[0].StartAction();
                            }
                        }
                        else
                        {
                            this.playerActionState = PlayerActionState.ACTION;

                            abilityManager.StartAbilitySelection();
                        }

                        return;
                    case PlayerActionState.ACTION:
                        this.currentTurn = Turn.ENEMY;

                        UpdateIdleEnemies();

                        if (this.idleEnemies.Count > 0)
                        {
                            this.idleEnemies[0].StartAction();
                        }

                        return;
                    default:
                        break;
                }
                break;
            case Turn.ENEMY:
                this.currentTurn = Turn.PLAYER;
                this.playerActionState = PlayerActionState.DICE_SELECTION;

                this.playerDice.StartRollingDice();
                return;
            default:
                break;
        }
    }

    public void UpdateIdleEnemies()
    {
        this.idleEnemies.Clear();

        foreach (ActionToken actionToken in enemies.GetComponentsInChildren<ActionToken>())
        {
            actionToken.Reset();
            this.idleEnemies.Add(actionToken);
            BaseEnemy baseEnemy = actionToken.GetComponent<BaseEnemy>();
            baseEnemy.UpdatePlayerDistance();
            baseEnemy.ChooseAbility();
        }

        this.idleEnemies = this.idleEnemies.Where(enemy =>
        {
            return enemy.isActiveAndEnabled;
        }).ToList();

        //sort by distance from player to preception range border to later on aggro in the correct order
        this.idleEnemies.Sort((x, y) =>
        {
            BaseEnemy xBaseEnemy = x.GetComponent<BaseEnemy>();
            BaseEnemy yBaseEnemy = y.GetComponent<BaseEnemy>();
            float difference = (xBaseEnemy.GetDistanceFromPlayer() - xBaseEnemy.PERCEPTION_RANGE) - (yBaseEnemy.GetDistanceFromPlayer() - yBaseEnemy.PERCEPTION_RANGE);

            if (difference < 0)
            {
                return -1;
            }
            else if (difference > 0)
            {
                return 1;
            }

            return 0;
        });
    }

    public bool CanWalk()
    {
        return this.currentTurn == Turn.PLAYER && this.playerActionState == PlayerActionState.WALKING;
    }

    public bool CanUseAbility()
    {
        return this.currentTurn == Turn.PLAYER && this.playerActionState == PlayerActionState.ACTION;
    }

    public void PassOnToken()
    {
        //should always be true
        if (this.idleEnemies.Count > 0)
        {
            this.idleEnemies.RemoveAt(0);
        }

        if (playerHealth.GetHealth() <= 0)
        {
            ReloadLevel();
        }

        //if enemies are yet to act, give them the token
        if (this.idleEnemies.Count > 0)
        {
            this.idleEnemies[0].StartAction();
        }
        else
        {
            IncrementTurnState();
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndLevel()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void AddToResetList(Health health)
    {
        this.resetList.Add(health);
    }

    public Turn GetCurrentTurn()
    {
        return this.currentTurn;
    }

    public void SetCurrentTurn(Turn value)
    {
        this.currentTurn = value;
    }

    public PlayerActionState GetPlayerActionState()
    {
        return this.playerActionState;
    }

    public void SetPlayerActionState(PlayerActionState value)
    {
        this.playerActionState = value;
    }
}
