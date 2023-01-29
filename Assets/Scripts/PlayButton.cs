using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    //gameManager kept under core components
    public GameObject gameManager;

    private MockData _mockdata;
    private DenomButton _denom;
    private Solver _solver;
    public TurnSequence BoardController;

    private void Start()
    {
        _mockdata = gameManager.GetComponent<MockData>();
        _denom = gameManager.GetComponent<DenomButton>();
        _solver = gameManager.GetComponent<Solver>();
    }
    //Dismiss win effects from last turn and begins a new win sequence, called from the play button
    public void Play()
    {
        BoardController.DismissTally();
        BoardController.DimAllWinlines();
        _mockdata.decreaseBalance(_denom.GetCurrentDenom());
        _solver.BoardSolve();
        StartCoroutine(BoardController.BoardFill(_solver.BoardArray));
    }

    //Called at the very end of the BoardFill Coroutine, this runs immeidately after all symbols and win effects have come on screen in TurnSequence
    public void PostPlay()
    {
        _mockdata.WinUpdateUI(BoardController.WinTotal);
    }

}
