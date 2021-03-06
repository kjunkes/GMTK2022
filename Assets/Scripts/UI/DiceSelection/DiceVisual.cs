using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class DiceVisual : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI diceText;
    public PlayerDice playerDice;
    
    public int diceId;
    public float diceNumberWaitTime;
    public float selectedDiceTransparency = 0.4f;

    // Next update in second
    private float nextUpdate = 0.1f;
    private int index;
    private int[] currentNumbers;
    private float randomTimeDiff;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitDice()
    {
        if(diceId < playerDice.dice.Length)
        {
            gameObject.SetActive(true);
            currentNumbers = playerDice.dice[diceId].numbers;
            diceText.text = currentNumbers[index].ToString();
            gameObject.GetComponent<Image>().sprite = playerDice.dice[diceId].image;
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks * diceId);
            index = currentNumbers[UnityEngine.Random.Range(0, currentNumbers.Length)];
            // Calculates Random time to change dice number switching interval
            randomTimeDiff = UnityEngine.Random.Range(-0.1f, 0.1f);
        } else
        {
            gameObject.SetActive(false);
            diceText.text = "";
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second + x)
            nextUpdate = Time.time + diceNumberWaitTime + randomTimeDiff;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        if(index >= currentNumbers.Length)
        {
            index = 0;
            diceText.text = currentNumbers[index].ToString();
            index++;
        }
        else
        {
            diceText.text = currentNumbers[index].ToString();
            index++;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentNumbers.Length != 1)
        {
            int finalNumber = currentNumbers[UnityEngine.Random.Range(0, currentNumbers.Length)];
            playerDice.dice[diceId].numbers = new int[] { finalNumber };
            currentNumbers = new int[] { finalNumber };

            diceText.text = finalNumber.ToString();

            // Change alpha channel of object
            Color gameObjectColor = gameObject.GetComponent<Image>().color;
            gameObjectColor.a = selectedDiceTransparency;
            gameObject.GetComponent<Image>().color = gameObjectColor;

            playerDice.EndRollingDice(finalNumber);
        }
    }
}

