using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DenomButton : MonoBehaviour
{
    public int denomIndex;
    public decimal[] denvalues = { 0.25M, 0.50M, 1.00M, 5.00M };
    public TextMeshProUGUI denomText;

    //Call this whenever something would change denom
    private void updateDenom()
    {
        denomText.text = "$" + denvalues[denomIndex].ToString();
    }

    //increases the denom index then updates the UI
    public void increaseDenom()
    {
        if (denomIndex < denvalues.Length-1)
        {
            denomIndex++;
            updateDenom();
        }
    }

    //decreases the denom index then updates the UI
    public void decreaseDenom()
    {
        if (denomIndex > 0)
        {
            denomIndex--;
            updateDenom();
        }
    }

    public decimal GetCurrentDenom()
    {
        return denvalues[denomIndex];
    }
}
