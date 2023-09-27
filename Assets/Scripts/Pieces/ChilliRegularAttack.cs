using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChilliRegularAttack : Action {

    int attackDamage = 2;  
    public Board b; 
    private bool lockedSpecial; 


    List<BoardTile> highlightingTiles = new List<BoardTile>(); 



    public ChilliRegularAttack(Chilli chilli) {
        this.thisPiece = chilli; 
    }

    public override void executeAction() {
        for(int i = 0; i < highlightingTiles.Count; i++) {
            BoardTile b = highlightingTiles[i]; 
            if(Board.allPieces[b.x, b.y] is Piece) {
                Board.allPieces[b.x, b.y].health -= attackDamage; 
            }
        }
        //clear the highlighed tiles
        for(int i = 0; i < highlightingTiles.Count; i++) {
            BoardTile b = highlightingTiles[i]; 
            GameObject thisTile = Board.tiles[b.x, b.y]; 
            thisTile.layer = LayerMask.NameToLayer("Tile"); 
        }
        highlightingTiles = new List<BoardTile>();
        this.unhighlighAttackArea(); 
        this.lockedSpecial = false; 
    }

    public void highlightAttackArea() {
        GameObject tile = Board.currentlyHoveringTile; 
        int tileX = Board.currentHoverX; 
        int tileY = Board.currentHoverY; 

        bool newRow = true; 

        if(Input.GetMouseButtonDown(0)) {
            lockedSpecial = true; 
        }

        if(!lockedSpecial) {
            for(int i = 0; i < highlightingTiles.Count; i++) {
            BoardTile thisTile = highlightingTiles[i]; 
            if(tileX == thisTile.x && tileY == thisTile.y) {
                newRow = false; 
            }
            //thisTile.layer = LayerMask.NameToLayer("Tile"); 
            }

            if(newRow) {
                for(int i = 0; i < highlightingTiles.Count; i++) {
                    BoardTile b = highlightingTiles[i]; 
                    GameObject thisTile = Board.tiles[b.x, b.y]; 
                    thisTile.layer = LayerMask.NameToLayer("Tile"); 
                }
                highlightingTiles = new List<BoardTile>();
            }
        }

        //looks to find current highlighed tiles in tiles covered 
        

         

        if(newRow && !lockedSpecial) {
            if(tileX == thisPiece.currentX && tileY >  thisPiece.currentY) {
            for(int i =  thisPiece.currentY + 1; i < Board.TILE_COUNT_Y; i++) {
                Board.tiles[ thisPiece.currentX, i].layer = LayerMask.NameToLayer("Attack_Range");
                BoardTile b = new BoardTile( thisPiece.currentX, i); 
                highlightingTiles.Add(b); 
            }
            } else if(tileX ==  thisPiece.currentX && tileY <  thisPiece.currentY) {
                for(int i =  thisPiece.currentY - 1; i > -1; i--) {
                    Board.tiles[ thisPiece.currentX, i].layer = LayerMask.NameToLayer("Attack_Range");
                    BoardTile b = new BoardTile( thisPiece.currentX, i); 
                    highlightingTiles.Add(b); 

                }
            } else if(tileY ==  thisPiece.currentY && tileX <  thisPiece.currentX) {
                for(int i =  thisPiece.currentX - 1; i > -1; i--) {
                    Board.tiles[i, thisPiece.currentY].layer = LayerMask.NameToLayer("Attack_Range");
                    BoardTile b = new BoardTile(i,  thisPiece.currentY); 
                    highlightingTiles.Add(b); 
                }
            } else if(tileY ==  thisPiece.currentY && tileX >  thisPiece.currentY) {
                for(int i =  thisPiece.currentX + 1; i < Board.TILE_COUNT_X; i++) {
                    Board.tiles[i, thisPiece.currentY].layer = LayerMask.NameToLayer("Attack_Range");
                    BoardTile b = new BoardTile(i,  thisPiece.currentY); 
                    highlightingTiles.Add(b); 
                }
            }
        }
        
    }

    public void unhighlighAttackArea() {
        for(int i = 0; i < this.highlightingTiles.Count; i++) {
            BoardTile thisTile = this.highlightingTiles[i]; 
            GameObject gameTile = Board.tiles[thisTile.x, thisTile.y]; 
            gameTile.layer = LayerMask.NameToLayer("Tile");
        }
        this.highlightingTiles = new List<BoardTile>(); 
        this.lockedSpecial = false;
    }

    public bool getLockedSpecial() {
        return this.lockedSpecial; 
    }

    public void lockSpecial() {
        this.lockedSpecial = true; 
    }
}