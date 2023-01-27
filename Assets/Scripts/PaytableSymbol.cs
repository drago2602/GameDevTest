using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaytableSymbol : MonoBehaviour
{
    public string symbolName;
    public int symbolIndex;
    public int PayValue;
    public TextMeshProUGUI paytableText;
    public int symbolWeight;
    private void Start()
    {
        paytableText = GetComponentInChildren<TextMeshProUGUI>();
    }

}
