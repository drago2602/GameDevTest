using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaytableSymbol : MonoBehaviour
{
    public string SymbolName;
    public int SymbolIndex;
    public int PayValue;
    public TextMeshProUGUI PaytableText;
    public int SymbolWeight;

    //Gets a reference to the child textmeshpro item in case one isn't set
    private void Start()
    {
        if (PaytableText == null)
        {
            PaytableText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

}
