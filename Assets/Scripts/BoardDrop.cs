using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;

public class BoardDrop : MonoBehaviour
{
    public GameObject[] BoardSymbols;
    public Transform[] BoardLocations;
    public Transform[] SpawnLocations;
    public Transform[] DestroyLocations;
    public Button[] Buttons;
    public Image[] Winlines;
    public Paytable symbolFactory;
    public PlayButton playButton;

    public decimal wintotal;

    public Image WinTallyBox;

    public Transform inpos;
    public Transform outpos;
    public TextMeshProUGUI tallyText;

    public float TallySpeed;
    public DenomButton denomref;

    public Animator GoblinAnim;


    public IEnumerator BoardFill(int[] symbolArray)
    {
        wintotal = 0;
        DisableButtons();

        var PosCounter = 0;
        foreach (Transform t in BoardLocations)
        {
            foreach (Transform j in t)
            {
                //Random generator creating a slightly variable delay in drop
                var varTime = Random.Range(0.1f, 0.5f);
                j.DOMoveY(DestroyLocations[PosCounter].position.y, varTime);
                j.gameObject.GetComponent<Image>().DOFade(0, varTime);
                PosCounter++;
                if (PosCounter >= 1)
                {
                    PosCounter = 0;
                }
                GameObject.Destroy(j.gameObject, varTime);
            }
        }

        yield return new WaitForSeconds(0.05f);

        var count = 0;
        var loopCount = 0;
        foreach(int i in symbolArray.Reverse())
        {
            //Instantiates the symbols with boardLocation markers as parents
            var currSymbol = Instantiate(BoardSymbols[i], SpawnLocations[loopCount].position, this.transform.rotation, BoardLocations[count]);

            //Random generator creating a slightly variable delay in drop
            var varTime = Random.Range(0.2f, 0.5f);

            //Dotween moving the symbol from the spawn location down to the Board Location
            currSymbol.transform.DOMoveY(BoardLocations[count].position.y, varTime);
            count++;
            loopCount++;
            if (loopCount >= 3)
            {
                loopCount = 0;
            }


            //Delays each symbol drop by 0.2 second to create a stagger effect
            yield return new WaitForSeconds(0.2f);
        }

        //move to solver if possible
        if (symbolArray[0].Equals(symbolArray[1]) && symbolArray[0].Equals(symbolArray[2]))
        {
            wintotal = wintotal + symbolFactory.paytableSymbols[symbolArray[0]].PayValue;
            Winlines[0].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);
            GoblinAnim.Play("Attack01");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WinTally());
        }
        if (symbolArray[3].Equals(symbolArray[4]) && symbolArray[3].Equals(symbolArray[5]))
        {
            wintotal = wintotal + symbolFactory.paytableSymbols[symbolArray[3]].PayValue;
            Winlines[1].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);
            GoblinAnim.Play("Attack01");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WinTally());
        }
        if (symbolArray[6].Equals(symbolArray[7]) && symbolArray[6].Equals(symbolArray[8]))
        {
            wintotal = wintotal + symbolFactory.paytableSymbols[symbolArray[6]].PayValue;
            Winlines[2].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);
            GoblinAnim.Play("Attack01");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WinTally());
        }
        if (symbolArray[0].Equals(symbolArray[4]) && symbolArray[0].Equals(symbolArray[8]))
        {
            wintotal = wintotal + symbolFactory.paytableSymbols[symbolArray[0]].PayValue;
            Winlines[3].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);
            GoblinAnim.Play("Attack01");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WinTally());
        }
        if (symbolArray[2].Equals(symbolArray[4]) && symbolArray[2].Equals(symbolArray[6]))
        {
            wintotal = wintotal + symbolFactory.paytableSymbols[symbolArray[2]].PayValue;
            Winlines[4].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);
            GoblinAnim.Play("Attack01");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WinTally());
        }
        if (wintotal == 0)
        {
            yield return new WaitForSeconds(0.5f);
            EnableButtons();
        }


        //If wins present then calculate total winnings
        // Begin Tally and UI
        playButton.PostPlay();

    }

    private void DisableButtons()
    {
        foreach(Button b in Buttons)
        {
            b.interactable = false;
        }

    }

    private void EnableButtons()
    {
        foreach (Button b in Buttons)
        {
            b.interactable = true;
        }
    }

    public void DimAllWinlines()
    {
        foreach(Image i in Winlines)
        {
            i.DOFade(0, 0.1f);
        }
    }

    public IEnumerator WinTally()
    {
        WinTallyBox.transform.DOMoveY(inpos.position.y, 1);
        var currTallyVal = 0.00m;
        yield return new WaitForSeconds(0.2f);
        while (currTallyVal < (wintotal*denomref.GetCurrentDenom()))
        {
            currTallyVal += 0.01m;
            tallyText.text = currTallyVal + "";
            if (currTallyVal < 1)
            {
                yield return new WaitForSeconds(TallySpeed*(float)denomref.GetCurrentDenom());
            }
            else if (currTallyVal < 5)
            {
                yield return new WaitForSeconds(TallySpeed/3);
            }
            else if (currTallyVal < 25)
            {
                yield return new WaitForSeconds(TallySpeed/5);
            }
        }
        if (currTallyVal == (wintotal * denomref.GetCurrentDenom())){
            yield return new WaitForSeconds(0.5f);
            EnableButtons();
        }
    }

    public void DismissTally()
    {
        //StopCoroutine(WinTally());
        var currTallyVal = 0;
        tallyText.text = currTallyVal + "";
        WinTallyBox.transform.DOMoveY(outpos.position.y, 0.2f);
    }
}
