using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDiceRollSelection : MonoBehaviour
{
    private Button button;
    private PlayerDice playerDice;

    // Start is called before the first frame update
    void Start()
    {
        playerDice = FindObjectOfType<PlayerDice>();
        button = gameObject.GetComponent(typeof(Button)) as Button;
        button.onClick.AddListener(EndRoll);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndRoll()
    {
        playerDice.EndRollingDice();
    }
}
