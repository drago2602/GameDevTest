using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;

public class TurnSequence : MonoBehaviour
{
    //gameManager kept under corecomponents in hierarchy
    public GameObject gameManager;

    public GameObject[] BoardSymbols;
    public Transform[] BoardLocations;
    public Transform[] SpawnLocations;
    public Transform[] DestroyLocations;
    public Button[] Buttons;
    public Image[] Winlines;
    public decimal WinTotal;
    public Image WinTallyBox;

    //In and out locations for the tally
    public Transform InPos;
    public Transform OutPos;

    public TextMeshProUGUI TallyText;
    public float TallySpeed;
    public Animator GoblinAnim;

    private Paytable _symbolfactory;
    private DenomButton _denomref;
    private PlayButton _playbutton;


    private void Start()
    {
        _symbolfactory = gameManager.GetComponent<Paytable>();
        _denomref = gameManager.GetComponent<DenomButton>();
        _playbutton = gameManager.GetComponent<PlayButton>();
    }
    //Win Sequence Controller
    public IEnumerator BoardFill(int[] symbolArray)
    {
        //clears last win and disables buttons for the turn
        WinTotal = 0;
        DisableButtons();

        //A counter used for the icon drop in
        var PosCounter = 0;
        foreach (Transform t in BoardLocations)
        {
            foreach (Transform j in t)
            {
                //Random generator creating a slightly variable delay in drop out
                var varTime = Random.Range(0.1f, 0.5f);

                //Move objects off the board and fade the image as it goes.
                j.DOMoveY(DestroyLocations[PosCounter].position.y, varTime);
                j.gameObject.GetComponent<Image>().DOFade(0, varTime);
                PosCounter++;
                if (PosCounter >= 1)
                {
                    PosCounter = 0;
                }
                //Destroy icon
                GameObject.Destroy(j.gameObject, varTime);
            }
        }

        //a delay to give time between symbols dropping out and dropping in
        yield return new WaitForSeconds(0.05f);

        var count = 0;
        var loopCount = 0;
        foreach(int i in symbolArray.Reverse())
        {
            //Instantiates the symbols with boardLocation markers as parents
            var currSymbol = Instantiate(BoardSymbols[i], SpawnLocations[loopCount].position, this.transform.rotation, BoardLocations[count]);

            //Random generator creating a slightly variable delay in drop in speed
            var varTime = Random.Range(0.2f, 0.5f);

            //Dotween moving the symbol from the spawn location down to the Board Location
            currSymbol.transform.DOMoveY(BoardLocations[count].position.y, varTime);
            count++;
            loopCount++;
            if (loopCount >= 3)
            {
                loopCount = 0;
            }

            //Makes new symbol wait 0.2 seconds before appearing to prevent them all falling simultaneously
            yield return new WaitForSeconds(0.2f);
        }

        //Checks each winline for wins and displays win info to player if present
        if (symbolArray[0].Equals(symbolArray[1]) && symbolArray[0].Equals(symbolArray[2]))
        {
            WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[0]].PayValue;
            StartCoroutine(WinHighlight(0));
        }
        if (symbolArray[3].Equals(symbolArray[4]) && symbolArray[3].Equals(symbolArray[5]))
        {
            WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[3]].PayValue;
            StartCoroutine(WinHighlight(1));
        }
        if (symbolArray[6].Equals(symbolArray[7]) && symbolArray[6].Equals(symbolArray[8]))
        {
            WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[6]].PayValue;
            StartCoroutine(WinHighlight(2));
        }
        if (symbolArray[0].Equals(symbolArray[4]) && symbolArray[0].Equals(symbolArray[8]))
        {
            WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[0]].PayValue;
            StartCoroutine(WinHighlight(3));
        }
        if (symbolArray[2].Equals(symbolArray[4]) && symbolArray[2].Equals(symbolArray[6]))
        {
            WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[2]].PayValue;
            StartCoroutine(WinHighlight(4));
        }
        if (WinTotal == 0)
        {
            yield return new WaitForSeconds(0.5f);
            EnableButtons();
        }
       
        // Begin UI update through postplay
        _playbutton.PostPlay();

    }

    //WinEffects begin here with some delay between each to avoid overwhelming player
    private IEnumerator WinHighlight(int winlineIndex)
    {
        Winlines[winlineIndex].DOFade(1, 1);
        yield return new WaitForSeconds(0.1f);
        GoblinAnim.Play("Attack01");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(WinTally());
        Debug.Log("finishing");
    }

    //Disables UI buttons
    private void DisableButtons()
    {
        foreach(Button b in Buttons)
        {
            b.interactable = false;
        }

    }

    //Enables UI buttons
    private void EnableButtons()
    {
        foreach (Button b in Buttons)
        {
            b.interactable = true;
        }
    }

    //Fades out all active winlines
    public void DimAllWinlines()
    {
        foreach(Image i in Winlines)
        {
            i.DOFade(0, 0.1f);
        }
    }

    //Controls win tally
    public IEnumerator WinTally()
    {
        //Tweens the box where the value tallys onto the screen
        WinTallyBox.transform.DOMoveY(InPos.position.y, 1);
        //Begins count again
        var currTallyVal = 0.00m;
        yield return new WaitForSeconds(0.2f);

        //Continuously ticks up the value in the winbox until it meets the required value
        //Speeds up the tally at certain intervals to keep it from taking forever on big wins.
        while (currTallyVal < (WinTotal*_denomref.GetCurrentDenom()))
        {
            currTallyVal += 0.01m;
            TallyText.text = currTallyVal + "";
            if (currTallyVal < 1)
            {
                yield return new WaitForSeconds(TallySpeed*(float)_denomref.GetCurrentDenom());
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

        //Enable buttons once value is reached
        if (currTallyVal == (WinTotal * _denomref.GetCurrentDenom())){
            yield return new WaitForSeconds(0.5f);
            EnableButtons();
        }
    }

    //Dismisses tally box and resets tally value.
    public void DismissTally()
    {
        var currTallyVal = 0;
        TallyText.text = currTallyVal.ToString();
        WinTallyBox.transform.DOMoveY(OutPos.position.y, 0.2f);
    }
}
