using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public MockData mockdata;
    public DenomButton denom;
    public Solver solver;
    public BoardDrop BoardController;

    public void Play()
    {
        BoardController.DismissTally();
        BoardController.DimAllWinlines();
        mockdata.decreaseBalance(denom.denvalues[denom.denomIndex]);
        solver.BoardSolve();
        StartCoroutine(BoardController.BoardFill(solver.BoardArray));
    }

    public void PostPlay()
    {
        mockdata.WinUpdateUI(BoardController.wintotal);
    }

}
