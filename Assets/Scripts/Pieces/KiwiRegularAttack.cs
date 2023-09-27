using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KiwiRegularAttack : Action {

    int attackDamage = 5; 
    int attackX; 
    int attackY; 
    public Board b; 
    private bool lockedSpecial; 

    GameObject highlightedTile = new GameObject(); 



    public KiwiRegularAttack(Kiwi kiwi) {
        this.attackX = 0; 
        this.attackY = 0; 
        this.thisPiece = kiwi; 

    }

    public override void executeAction() {
        if(Board.allPieces[this.attackX, this.attackY] is Piece) {
            Board.allPieces[this.attackX, this.attackY].health -= attackDamage;
        }
        this.unhighlighAttackArea(); 
    }

    public void highlightAttackArea() {


        if(Board.currentlyHoveringTile != null) {
             GameObject tile = Board.currentlyHoveringTile; 
            int tileX = Board.currentHoverX; 
            int tileY = Board.currentHoverY;

             tile.layer = LayerMask.NameToLayer("Attack_Range");

            if(tileX != this.attackX || tileY != this.attackY && !this.lockedSpecial) {
                this.highlightedTile.layer = LayerMask.NameToLayer("Tile");
                this.highlightedTile = tile; 
                this.attackX = tileX; 
                this.attackY = tileY; 
            }

            if(Input.GetMouseButtonDown(0)) {
                this.lockedSpecial = true; 
            }
        }
       
        
    }

    public void unhighlighAttackArea() {

        this.highlightedTile.layer = LayerMask.NameToLayer("Tile"); 

        this.highlightedTile = new GameObject();  
        this.lockedSpecial = false; 
    }


    public bool getLockedSpecial() {
        return this.lockedSpecial; 
    }
}