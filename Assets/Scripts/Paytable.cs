using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Paytable : MonoBehaviour
{
    public PaytableSymbol[] paytableSymbols;
    public DenomButton denom;

    private void Start()
    {
        UpdatePaytable();
    }

    public void UpdatePaytable()
    {
        foreach (PaytableSymbol i in paytableSymbols)
        {
            i.paytableText.text = "3x           wins $" + i.PayValue * denom.GetCurrentDenom();
        }
    }
}
