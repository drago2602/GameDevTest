using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public Paytable IconInfo;
    private int _totalWeight;
    public int[] BoardArray;

    // Start is called before the first frame update
    void Start()
    {
        //instantiates an array with the needed number of values.
        BoardArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //calculates the total weight of all symbols together
        foreach (PaytableSymbol i in IconInfo.PaytableSymbols)
        {
            _totalWeight = _totalWeight + i.SymbolWeight;
        }
    }

    public void BoardSolve()
    {
        //generates an array of symbols based on weighted random values.
        for (int i = 0; i < 9; i++)
         {
            var symVal = Random.Range(0, _totalWeight+1);
            var processedWeight = 0;
            foreach(PaytableSymbol k in IconInfo.PaytableSymbols)
            {
                //change to percentages
                processedWeight += k.SymbolWeight;
                if (symVal <= processedWeight)
                {
                    BoardArray[i] = k.SymbolIndex;
                    break;
                }
            }
        }
    }
}
