using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DenomButton : MonoBehaviour
{
    public int DenomIndex;
    public decimal[] DenValues = { 0.25M, 0.50M, 1.00M, 5.00M };
    public TextMeshProUGUI DenomText;
    public GameObject GameManager;
    public Button PlayButtonRef;

    private MockData _mockData;

    private void Start()
    {
        _mockData = GameManager.GetComponent<MockData>();
    }
    //Updates UI whenever something changes denom
    private void UpdateDenom()
    {
        DenomText.text = "$" + DenValues[DenomIndex].ToString();
    }

    //increases the denom index then updates the UI
    public void increaseDenom()
    {
        if (DenomIndex < DenValues.Length-1)
        {
            DenomIndex++;
            UpdateDenom();
            CheckValidDenom();
        }

    }

    //decreases the denom index then updates the UI
    public void DecreaseDenom()
    {
        if (DenomIndex > 0)
        {
            DenomIndex--;
            UpdateDenom();
            CheckValidDenom();
        }
    }

    //returns the current denom value.
    public decimal GetCurrentDenom()
    {
        return DenValues[DenomIndex];
    }

    public void CheckValidDenom()
    {
        if (DenValues[DenomIndex] > _mockData.GetBalance())
        {
            PlayButtonRef.GetComponent<Button>().interactable = false;
        }
        else
        {
            PlayButtonRef.GetComponent<Button>().interactable = true;
        }
    }
}
