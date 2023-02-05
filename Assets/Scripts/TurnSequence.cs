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
    public Image[] WinlineReference;
    public decimal WinTotal;
    public Image WinTallyBox;

    //In and out locations for the tally
    public Transform InPos;
    public Transform OutPos;

    public TextMeshProUGUI TallyText;
    public float TallySpeed;
    public Animator GoblinAnim;
    public Winline[] winlines;


    private Paytable _symbolfactory;
    private DenomButton _denomref;
    private PlayButton _playbutton;
    private decimal _calcwintotal;

    [System.Serializable]
    public class Winline
    {
        public int Pos1;
        public int Pos2;
        public int Pos3;
    }

    private void Start()
    {
        _symbolfactory = gameManager.GetComponent<Paytable>();
        _denomref = gameManager.GetComponent<DenomButton>();
        _playbutton = gameManager.GetComponent<PlayButton>();
    }


    public IEnumerator ClearSymbol(Transform j)
    {
        yield return new WaitForSeconds(0.5f);
        j.DOKill();
        j.gameObject.GetComponent<Image>().DOKill();

        GameObject.Destroy(j.gameObject);
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
                StartCoroutine(ClearSymbol(j));

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
        var Wincount = 0;
        foreach (Winline w in winlines)
        {
            if (symbolArray[w.Pos1] == symbolArray[w.Pos2] && symbolArray[w.Pos1] == symbolArray[w.Pos3])
            {
                Debug.Log("Running");
                WinTotal = WinTotal + _symbolfactory.PaytableSymbols[symbolArray[w.Pos1]].PayValue;
                StartCoroutine(WinHighlight(Wincount));
            }
            Wincount++;
        }

        if (WinTotal == 0)
        {
            yield return new WaitForSeconds(0.7f);
            EnableButtons();
        }
       
        // Begin UI update through postplay
        _playbutton.PostPlay();

    }

    //WinEffects begin here with some delay between each to avoid overwhelming player
    private IEnumerator WinHighlight(int winlineIndex)
    {
        WinlineReference[winlineIndex].DOFade(1, 1);
        yield return new WaitForSeconds(0.1f);
        GoblinAnim.Play("Attack01");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(WinTally());
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
        foreach(Image i in WinlineReference)
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
        var TallyAmount = 0.01m;
        while (currTallyVal < DenomWinTotal())
        {
            currTallyVal += TallyAmount;
            TallyText.text = "$" + currTallyVal + "";
            //Started to change tally amount because WaitForSeconds can't go any lower than 0.01 of a second accurately.
            if (currTallyVal < _denomref.GetCurrentDenom())
            {
                yield return new WaitForSeconds(TallySpeed);
            }
            else if (currTallyVal < 5)
            {
                TallyAmount = 0.25m;
                yield return new WaitForSeconds((TallySpeed));
            }
            else if (currTallyVal < 25)
            {
                TallyAmount = 1.00m;
                yield return new WaitForSeconds((TallySpeed));
            }
        }
        //Cap the tally at proper final win amount to avoid any hanging decimals
        currTallyVal = (DenomWinTotal());
        TallyText.text = "$" + currTallyVal + "";

        //Enable buttons once value is reached
        if (currTallyVal == (DenomWinTotal())){
            yield return new WaitForSeconds(0.7f);
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

    //Calculate the denom * the Wintotal and store it in _calcwintotal to reduce duplicate code.
    public decimal DenomWinTotal()
    {
        _calcwintotal = (WinTotal * _denomref.GetCurrentDenom());
        return _calcwintotal;
    }
    }


