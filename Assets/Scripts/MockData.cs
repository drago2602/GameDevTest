using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MockData : MonoBehaviour
{
    private decimal _balance;
    public TextMeshProUGUI BalanceText;

    public decimal LastWin;
    public TextMeshProUGUI WinBox;

    //Sets balance to 10 and updates UI to reflect that as per instructions.
    private void Start()
    {
        _balance = 10.00M;
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

    //Updates balance and last win in a single function to save repeat function calls
    public void WinUpdateUI(decimal win)
    {
        _balance = _balance + win;
        UpdateBalance();
        LastWin = win;
        UpdateLastWin();
    }
}
