using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PieceMovement : Action
{
    public List<BoardTile> theseTiles; 
    public List<LineRenderer> theseLines; 

    public override void executeAction() {
        Board.allPieces[thisPiece.currentX, thisPiece.currentY] = null; 

        thisPiece.Move(this.theseTiles, this.theseLines); 
    }

        
}



