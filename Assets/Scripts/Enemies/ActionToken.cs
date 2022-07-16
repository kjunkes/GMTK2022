using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionToken : MonoBehaviour
{
    private GameLoop gameLoop;

    private bool hasActionToken = false;
    private bool hasActedThisTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetHasToken()
    {
        return this.hasActionToken;
    }

    public bool GetHasActedThisTurn()
    {
        return this.hasActedThisTurn;
    }

    public void StartAction()
    {
        this.hasActionToken = true;
    }

    public void EndAction()
    {
        this.hasActedThisTurn = true;
        this.hasActionToken = false;
        this.gameLoop.PassOnToken();
    }
}
