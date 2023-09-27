using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilli : Piece
{
    [SerializeField]private bool attacking; 

    private List<BoardTile> highlightingTiles = new List<BoardTile>(); 

    private ChilliRegularAttack CRA; 

    protected override void Awake() {
        base.Awake();  
        this.CRA = new ChilliRegularAttack(this); 
    }

    protected override void Update() {
        base.Update(); 

        if(attacking) {
           this.CRA.highlightAttackArea(); 
           if(this.CRA.getLockedSpecial()) {
               this.attacking = false; 
               TurnPlanner.turnActions.Add(this.CRA); 
           }
        }


    }

    public override void regularAttack()
    {
        if(!this.attacking) {
            this.attacking = true; 
        }
    }

}
