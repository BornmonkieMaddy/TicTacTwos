using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public int playerWhoseTurn;
    public Image[] _bluePawns;
    public Image[] _orangePawns;

    public bool canSelectPawn;
    public Pawn selectedPawn;
    public bool canMovePawn;

    private void Start()
    {
        Debug.Log("game started");
        StartGameSession();
        canSelectPawn = true;
    }

    public void StartGameSession()
    {
        playerWhoseTurn = Random.Range(0, 1);
    }

    public void SelectPawn(Pawn currentPawn)
    {
        if (!canSelectPawn) return;

        selectedPawn = currentPawn;
        if (playerWhoseTurn == selectedPawn.pawnIndex)
        {
            Debug.Log("selected pawn : " + selectedPawn.pawnColor + selectedPawn.pawnSize);
            if (selectedPawn.isPawnPlacedOnGrid)
            {
                Debug.Log("pawn already placed on grid");
                selectedPawn = null;
                canMovePawn = false;
            }
            else
            {
                canMovePawn = true;
                canSelectPawn = false;

                if (playerWhoseTurn == 0)
                    playerWhoseTurn = 1;
                else if (playerWhoseTurn == 1)
                    playerWhoseTurn = 0;
            }
        }

        else
        {
            Debug.Log("wrong pawn selected !! \n  Select your pawn");
            selectedPawn = null;
            canMovePawn = false;
        }

    }

    public void PlacePawnOnGrid(Transform endLocation)
    {
        if (selectedPawn != null)
        {
            if (canMovePawn)
            {
                Debug.Log("moving pawn");
                selectedPawn.transform.DOMove(endLocation.position, 0.5f).SetEase(Ease.OutFlash);
                selectedPawn.isPawnPlacedOnGrid = true;
                selectedPawn = null;
                canMovePawn = false;
                canSelectPawn = true;                
            }
        }
    }

}

