using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Paytable : MonoBehaviour
{
    public PaytableSymbol[] PaytableSymbols;
    public DenomButton Denom;

    //Makes sure the paytable reflects the starting denom
    private void Start()
    {
        UpdatePaytable();
    }

    //Iterates through paytable symbols and updates the win amount based on denomination
    public void UpdatePaytable()
    {
        foreach (PaytableSymbol i in PaytableSymbols)
        {
            i.PaytableText.text = "3x           wins $" + i.PayValue * Denom.GetCurrentDenom();
        }
    }
}
