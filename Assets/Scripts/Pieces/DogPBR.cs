using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPBR : Piece
{
    [SerializeField]private bool specialAttack; 

    private bool lockedSpecial; 

    protected override void Update() {
        base.Update(); 
    }

    public override void regularAttack() {
        DogRegularAttack DRA = new DogRegularAttack(this.currentX, this.currentY, true, this); 
        DRA.highlightAttackArea(); 
        TurnPlanner.turnActions.Add(DRA); 
    }

}
