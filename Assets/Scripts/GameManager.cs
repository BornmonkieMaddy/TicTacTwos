using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<string, int> onGameOver;
    public UnityEvent onGameDraw;
    public bool gameOver;

    public int playerWhoseTurn;

    public bool canSelectPawn;
    public Pawn selectedPawn;
    public bool canMovePawn;

    public Color _originalColor;
    public int totalMovesCompleted;

    public int totaBlueLarge, totalBlueMedium;
    public int totaOrangeLarge, totalOrangeMedium;

    public GameObject[] gridElements;

    [Header("UI")]
    public GameObject winPanel;
    public Text winText;
    public GameObject[] pawnSelectBg;


    private void Start()
    {
        StartGameSession();
        canSelectPawn = true;
    }

    public void StartGameSession()
    {
        gameOver = false;
        totalBlueMedium = totaBlueLarge = totalOrangeMedium = totaOrangeLarge = 2;
        playerWhoseTurn = Random.Range(0, 1);
        pawnSelectBg[playerWhoseTurn].SetActive(true);
    }

    //select pawn and check all neccessary conditions 
    public void SelectPawn(Pawn currentPawn)
    {
        if (!canSelectPawn) return;

        selectedPawn = currentPawn;
        if (playerWhoseTurn == selectedPawn.pawnIndex)
        {
            //Debug.Log("selected pawn : " + selectedPawn.pawnColor + selectedPawn.pawnSize);
            if (selectedPawn.isPawnPlacedOnGrid)
            {
                //Debug.Log("pawn already placed on grid");
                selectedPawn = null;
                canMovePawn = false;
            }
            else
            {
                _originalColor = selectedPawn.GetComponent<Image>().color;
                selectedPawn.GetComponent<Image>().color = Color.yellow;
                canMovePawn = true;
                canSelectPawn = false;
            }
        }
        else
        {
            //Debug.Log("wrong pawn selected !! \n  Select your pawn");
            selectedPawn = null;
            canMovePawn = false;
        }


    }

    //place pawn on the grid after selecting pawn
    public void PlacePawnOnGrid(Transform endLocation)
    {
        if (selectedPawn != null)
        {
            if (canMovePawn)
            {
                //check if there are no pre placed pawns on location
                if (endLocation.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Empty)
                {
                    //Debug.Log("moving pawn");
                    selectedPawn.transform.DOMove(endLocation.position, 0.5f).SetEase(Ease.OutFlash).OnComplete(() =>
                    {
                        totalMovesCompleted++;
                        selectedPawn.GetComponent<Image>().color = _originalColor;
                        Destroy(endLocation.GetChild(0).gameObject);
                        selectedPawn.transform.SetParent(endLocation);
                        ReduceRemainingPawns(selectedPawn.pawnColor, selectedPawn.pawnSize);
                        selectedPawn.isPawnPlacedOnGrid = true;
                        selectedPawn = null;
                        canMovePawn = false;
                        canSelectPawn = true;

                        Invoke("CheckWinCondition", 0.1f);

                        if (playerWhoseTurn == 0)
                        {
                            playerWhoseTurn = 1;
                            pawnSelectBg[0].SetActive(false);
                            pawnSelectBg[1].SetActive(true);
                        }
                        else if (playerWhoseTurn == 1)
                        {
                            playerWhoseTurn = 0;
                            pawnSelectBg[1].SetActive(false);
                            pawnSelectBg[0].SetActive(true);
                        }

                    });
                }

                //if already pawn is placed on that location
                else
                {
                    //if selected pawn is small we cannot replace any pawns
                    if (selectedPawn.pawnSize == PawnSize.Small) return;

                    //if selected pawn is medium we can replace only with small pawns
                    if (selectedPawn.pawnSize == PawnSize.Medium)
                    {
                        if (endLocation.GetChild(0).GetComponent<Pawn>().pawnSize == PawnSize.Small)
                        {
                            //Debug.Log("destroying current small pawn and moving medium pawn to its place");
                            selectedPawn.transform.DOMove(endLocation.position, 0.5f).SetEase(Ease.OutFlash).OnComplete(() =>
                            {
                                totalMovesCompleted++;
                                selectedPawn.GetComponent<Image>().color = _originalColor;
                                Destroy(endLocation.GetChild(0).gameObject);
                                selectedPawn.transform.SetParent(endLocation);
                                ReduceRemainingPawns(selectedPawn.pawnColor, selectedPawn.pawnSize);
                                selectedPawn.isPawnPlacedOnGrid = true;
                                selectedPawn = null;
                                canMovePawn = false;
                                canSelectPawn = true;

                                Invoke("CheckWinCondition", 0.1f);

                                if (playerWhoseTurn == 0)
                                {
                                    playerWhoseTurn = 1;
                                    pawnSelectBg[0].SetActive(false);
                                    pawnSelectBg[1].SetActive(true);
                                }
                                else if (playerWhoseTurn == 1)
                                {
                                    playerWhoseTurn = 0;
                                    pawnSelectBg[1].SetActive(false);
                                    pawnSelectBg[0].SetActive(true);
                                }
                            });
                        }
                    }

                    //if selected pawn is large we can replace with both small and medium pawns
                    if (selectedPawn.pawnSize == PawnSize.Large)
                    {
                        if (endLocation.GetChild(0).GetComponent<Pawn>().pawnSize == PawnSize.Small || endLocation.GetChild(0).GetComponent<Pawn>().pawnSize == PawnSize.Medium)
                        {
                            //Debug.Log("destroying current small pawn and moving medium pawn to its place");
                            selectedPawn.transform.DOMove(endLocation.position, 0.5f).SetEase(Ease.OutFlash).OnComplete(() =>
                            {
                                totalMovesCompleted++;
                                selectedPawn.GetComponent<Image>().color = _originalColor;
                                Destroy(endLocation.GetChild(0).gameObject);
                                selectedPawn.transform.SetParent(endLocation);
                                ReduceRemainingPawns(selectedPawn.pawnColor, selectedPawn.pawnSize);
                                selectedPawn.isPawnPlacedOnGrid = true;
                                selectedPawn = null;
                                canMovePawn = false;
                                canSelectPawn = true;

                                Invoke("CheckWinCondition", 0.1f);

                                if (playerWhoseTurn == 0)
                                {
                                    playerWhoseTurn = 1;
                                    pawnSelectBg[0].SetActive(false);
                                    pawnSelectBg[1].SetActive(true);
                                }
                                else if (playerWhoseTurn == 1)
                                {
                                    playerWhoseTurn = 0;
                                    pawnSelectBg[1].SetActive(false);
                                    pawnSelectBg[0].SetActive(true);
                                }
                            });
                        }
                    }
                }
            }
        }
    }

    public void CheckWinCondition()
    {

        #region ColumnCheck
        //column check
        for (int j = 0; j < 3; j++)
        {
            int countB = 0;
            int countO = 0;

            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    if (gridElements[j].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Blue)
                        countB++;
                    else if (gridElements[j].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Orange)
                        countO++;
                }
                else
                {
                    if (gridElements[((i * i + j) + 2)].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Blue)
                        countB++;
                    else if (gridElements[((i * i + j) + 2)].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Orange)
                        countO++;
                }
            }

            if (countB == 3)
            {
                //Debug.Log("3 Blue in column done:");
                onGameOver?.Invoke($"3 Blue in Column {j + 1} done", playerWhoseTurn);
            }

            if (countO == 3)
            {
                //Debug.Log("3 Orange in column done");
                onGameOver?.Invoke($"3 Orange in Column {j + 1} done", playerWhoseTurn);
            }
        }

        #endregion

        #region RowCheck
        //row check
        for (int i = 0; i < 3; i++)
        {
            int countB = 0;
            int countO = 0;

            for (int j = 0; j < 3; j++)
            {
                if (gridElements[((i + i + i) + j)].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Blue)
                    countB++;
                else if (gridElements[((i + i + i) + j)].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Orange)
                    countO++;
            }

            if (countB == 3)
            {
                //Debug.Log("3 Blue in row done:");
                onGameOver?.Invoke($"3 Blue in Row {i + 1} done", playerWhoseTurn);
            }

            if (countO == 3)
            {
                //Debug.Log("3 Orange in row done");
                onGameOver?.Invoke($"3 Orange in Row {i + 1} done", playerWhoseTurn);
            }
        }
        #endregion

        #region DiagonalCheck
        int countBD = 0;
        int countOD = 0;

        //diagonal check
        for (int i = 0; i < gridElements.Length; i++)
        {
            if (i % 4 == 0)
            {
                if (gridElements[i].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Blue)
                    countBD++;
                else if ((gridElements[i].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Orange))
                    countOD++;
            }

            if (countBD == 3)
            {
                //Debug.Log("3 Blue in row done:");
                onGameOver?.Invoke($"3 Blue in Diag done", playerWhoseTurn);
            }

            if (countOD == 3)
            {
                //Debug.Log("3 Orange in row done");
                onGameOver?.Invoke($"3 Orange in Diag done", playerWhoseTurn);
            }
        }

        //reverse diagonal check
        int countBDR = 0;
        int countODR = 0;
        //diagonal check
        for (int i = 0; i < gridElements.Length; i++)
        {
            if (i == 2 || i == 4 || i == 6)
            {
                if (gridElements[i].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Blue)
                    countBDR++;
                else if ((gridElements[i].transform.GetChild(0).GetComponent<Pawn>().pawnColor == PawnColor.Orange))
                    countODR++;
            }

            if (countBDR == 3)
            {
                //Debug.Log("3 Blue in row done:");
                onGameOver?.Invoke($"3 Blue in Diag Reverse done", playerWhoseTurn);
            }

            if (countODR == 3)
            {
                //Debug.Log("3 Orange in row done");
                onGameOver?.Invoke($"3 Orange in Diag Reverse done", playerWhoseTurn);
            }
        }

        #endregion

        //Draw condition
        if (totalBlueMedium == 0 && totaBlueLarge == 0 && totalOrangeMedium == 0 && totaOrangeLarge == 0)
        {
            for (int i = 0; i < gridElements.Length; i++) ;
            {
                if (gridElements[0].transform.GetChild(0).GetComponent<Pawn>().pawnColor != PawnColor.Empty)
                {
                    onGameDraw?.Invoke();
                }
            }           
        }
    }

    public void GameOver(string reason, int whowon)
    {
        gameOver = true;
        Debug.Log("Win Reason : " + reason);
        winPanel.SetActive(true);
        if (whowon == 1) whowon = 0;
        else if (whowon == 0) whowon = 1;
        winText.text = "Player " + whowon + " wins !!!";
    }

    public void GameDraw()
    {
        Debug.Log("Match Draw");
        winPanel.SetActive(true);
        winText.text = "Match Ended in a Draw";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitEditor()
    {
        Debug.LogError("quitting editor");
        UnityEditor.EditorApplication.isPlaying = false;
    }

    //reduce remaining medium and large pawn count
    public void ReduceRemainingPawns(PawnColor color, PawnSize size)
    {
        if (color == PawnColor.Blue && size == PawnSize.Medium)
        {
            totalBlueMedium--;
        }

        if (color == PawnColor.Blue && size == PawnSize.Large)
        {
            totaBlueLarge--;
        }

        if (color == PawnColor.Orange && size == PawnSize.Medium)
        {
            totalOrangeMedium--;
        }

        if (color == PawnColor.Orange && size == PawnSize.Large)
        {
            totaOrangeLarge--;
        }
    }

}