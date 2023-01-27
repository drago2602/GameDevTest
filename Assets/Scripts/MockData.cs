using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MockData : MonoBehaviour
{
    private decimal balance;
    public TextMeshProUGUI balanceText;

    public decimal lastWin;
    public TextMeshProUGUI winBox;

    private void Start()
    {
        balance = 10.00M;
        UpdateBalance();
    }

    public void UpdateBalance()
    {
        balanceText.text = "Balance: $" + balance.ToString();
    }

    private void UpdateLastWin()
    {
        winBox.text = "Last Win: $" + lastWin.ToString();
    }

    public void increaseBalance(decimal value)
    {
        balance += value;
        UpdateBalance();
    }

    public void decreaseBalance(decimal value)
    {
        balance -= value;
        UpdateBalance();
    }

    public void WinUpdateUI(decimal win)
    {
        balance = balance + win;
        UpdateBalance();
        lastWin = win;
        UpdateLastWin();
        //Debug.Log("check" + win.ToString());
    }
}
