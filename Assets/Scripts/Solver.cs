using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public Paytable iconInfo;
    private int totalWeight;

    public int[] BoardArray;
    // Start is called before the first frame update
    void Start()
    {
        BoardArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        foreach (PaytableSymbol i in iconInfo.paytableSymbols)
        {
            totalWeight = totalWeight + i.symbolWeight;
            //Debug.Log(totalWeight);
        }
    }

    public void BoardSolve()
    {
        //BoardArray = null;
        for (int i = 0; i < 9; i++)
         {
            var symVal = Random.Range(0, totalWeight+1);
            var processedWeight = 0;
            foreach(PaytableSymbol k in iconInfo.paytableSymbols)
            {
                //change to percentages
                processedWeight += k.symbolWeight;
                if (symVal <= processedWeight)
                {
                    BoardArray[i] = k.symbolIndex;
                    break;
                }
            }
        }
    }
}
