using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceVisual : MonoBehaviour
{
    public TextMeshProUGUI diceText;
    

    public int diceLow;
    public int diceHigh;
    public int diceStepSizes;

    // Next update in second
    private float nextUpdate = 0.1f;
    private int currentDiceNumber;


    // Start is called before the first frame update
    void Start()
    {
        currentDiceNumber = diceLow;
    }

    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second + x)
            nextUpdate = Time.time + 0.1f;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        diceText.text = currentDiceNumber.ToString();

        if (currentDiceNumber < diceHigh)
        {
            currentDiceNumber += diceStepSizes;
        } else
        {
            currentDiceNumber = diceLow;
        }
    }
}
