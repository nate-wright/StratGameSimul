using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogRegularAttack : Action {

    int attackDamage = 3; 
    int x; 
    int y; 
    bool forward; 
    public Board b; 

    List<GameObject> highlightedTiles = new List<GameObject>(); 



    public DogRegularAttack(int x, int y, bool forward, DogPBR dog) {
        this.x = x; 
        this.y = y; 
        this.forward = forward; 
        this.thisPiece = dog; 

    }

    public override void executeAction() {
        if(forward) {
            for(int i = x - 1; i <= x+ 1; i++) {
                if(i >=0 && i < Board.TILE_COUNT_X && (y + 1) < Board.TILE_COUNT_Y && (y + 1) >=0) {
                    if(Board.allPieces[i, y + 1] is Piece) {
                        Board.allPieces[i, y + 1].health -= attackDamage; 
                    }
                }
            }
        } else {
             for(int i = x - 1; i <= x+ 1; i++) {
                 if(i >=0 && i < Board.TILE_COUNT_X && (y - 1) < Board.TILE_COUNT_Y && (y - 1) >=0) {
                    if(Board.allPieces[i, y - 1] is Piece) {
                        Board.allPieces[i, y - 1].health -= attackDamage; 
                    }
                 }
                
            }
        }
        this.unhighlighAttackArea(); 
    }

    public void highlightAttackArea() {
        GameObject tile = Board.currentlyHoveringTile; 
        int tileX = Board.currentHoverX; 
        int tileY = Board.currentHoverY; 

        if(tileX >= this.x) {
            this.forward = true; 
        } else {
            this.forward = false; 
        }

        if(this.forward) {
            for(int i = x - 1; i <= x+ 1; i++) {
                if(i >=0 && i < Board.TILE_COUNT_X && (y + 1) < Board.TILE_COUNT_Y && (y + 1) >=0) {
                     if(Board.tiles[i, y + 1]){
                        GameObject thisTile = Board.tiles[i, y +1]; 
                        thisTile.layer = LayerMask.NameToLayer("Attack_Range");
                        this.highlightedTiles.Add(thisTile);         
                    }
                }  
            }
        }else {
            for(int i = x - 1; i <= x+ 1; i++) {
                if(i >=0 && i < Board.TILE_COUNT_X && (y - 1) < Board.TILE_COUNT_Y && (y - 1) >=0) {
                    if(Board.tiles[i, y - 1]) {
                        GameObject thisTile = Board.tiles[i, y - 1]; 
                        thisTile.layer = LayerMask.NameToLayer("Attack_Range");
                        this.highlightedTiles.Add(thisTile); 
                    }
                }
                
            }
        }
        
    }

    public void unhighlighAttackArea() {
        for(int i = 0; i < this.highlightedTiles.Count; i++) {
            GameObject thisTile = this.highlightedTiles[i]; 
            thisTile.layer = LayerMask.NameToLayer("Tile");
        }
        this.highlightedTiles = new List<GameObject>(); 
    }
}