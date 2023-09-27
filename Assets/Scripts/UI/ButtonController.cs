using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ButtonController : MonoBehaviour
{
    public Button turnOver; 
    //public Board thisBoard; 
    // Start is called before the first frame update
    void Start()
    {
        //thisBoard = GetComponent<Board>(); 
        turnOver.onClick.AddListener(EndTurn); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndTurn() {
        Debug.Log("TURN OVER");
        for(int i = 0; i < Board.pieceMovement.Count; i++) {
            TurnPlanner.turnActions.Add(Board.pieceMovement[i]); 
        }
        TurnPlanner tp = new TurnPlanner(this);
        StartCoroutine(this.TimeoutEndTurnButton(2, TurnPlanner.turnActions.Count)); 

        tp.ExecuteTurn(); 

    }

    void MovePieces() {
        List<PieceMovement> p = Board.pieceMovement; 
        for(int i = 0; i < p.Count; i++) {
            PieceMovement thisPM = p[i];
            Piece thisPiece = thisPM.thisPiece; 

            Board.allPieces[thisPiece.currentX, thisPiece.currentY] = null; 

            thisPiece.Move(thisPM.theseTiles, thisPM.theseLines); 

             
        }

        Board.pieceMovement = new List<PieceMovement>(); 
    }

    IEnumerator TimeoutEndTurnButton(float seconds, int numberOfActions)
    {
        turnOver.interactable = false;
        yield return new WaitForSeconds(seconds * numberOfActions);
        turnOver.interactable = true;
    }

    public void setInteractable() {
        turnOver.interactable = true; 
    }
}
