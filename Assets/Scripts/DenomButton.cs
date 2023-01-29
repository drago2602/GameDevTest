using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DenomButton : MonoBehaviour
{
    public int DenomIndex;
    public decimal[] DenValues = { 0.25M, 0.50M, 1.00M, 5.00M };
    public TextMeshProUGUI DenomText;

    //Updates UI whenever something changes denom
    private void updateDenom()
    {
        DenomText.text = "$" + DenValues[DenomIndex].ToString();
    }

    //increases the denom index then updates the UI
    public void increaseDenom()
    {
        if (DenomIndex < DenValues.Length-1)
        {
            DenomIndex++;
            updateDenom();
        }
    }

    //decreases the denom index then updates the UI
    public void decreaseDenom()
    {
        if (DenomIndex > 0)
        {
            DenomIndex--;
            updateDenom();
        }
    }

    //returns the current denom value.
    public decimal GetCurrentDenom()
    {
        return DenValues[DenomIndex];
    }
}
