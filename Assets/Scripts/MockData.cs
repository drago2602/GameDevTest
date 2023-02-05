using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MockData : MonoBehaviour
{
    //A testing tool to help test the game. Use it to set your balance to higher values.
    public float mockBalance;

    private decimal _balance;
    public TextMeshProUGUI BalanceText;

    public decimal LastWin;
    public TextMeshProUGUI WinBox;

    //Sets balance to 10 and updates UI to reflect that as per instructions.
    private void Start()
    {
        //check if user has any input on testing tool
        //Testing tool left in for evaluator convenience
        if (mockBalance > 0)
        {
            _balance = (decimal)mockBalance + 0.00m;
        }
        //if no test value is set, ordinary default of 10 as per instructions
        else
        {
            _balance = 10.00m;
        }

        UpdateBalance();
    }

    //Updates the current balance UI.
    public void UpdateBalance()
    {
        BalanceText.text = "Balance: $" + _balance.ToString();
    }

    //Updates the Last Win text to reflect the last win.
    private void UpdateLastWin()
    {
        WinBox.text = "Last Win: $" + LastWin.ToString();
    }
    
    //Increase balance by value and update UI
    public void increaseBalance(decimal value)
    {
        _balance += value;
        UpdateBalance();
    }
    
    //Decrease balance by value and update UI
    public void decreaseBalance(decimal value)
    {
        _balance -= value;
        UpdateBalance();
    }

    //A getter for the balance value
    public decimal GetBalance()
    {
        return _balance;
    }

    //Updates balance and last win in a single function to save repeat function calls
    public void WinUpdateUI(decimal win)
    {
        increaseBalance(win);
        if (win > 0)
        {
            LastWin = win;
            UpdateLastWin();
        }

    }
}
