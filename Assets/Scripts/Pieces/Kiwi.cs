using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : Piece
{
    [SerializeField]private bool specialAttack; 

    private bool lockedSpecial; 

    private bool attacking; 

    private KiwiRegularAttack KRA; 


    protected override void Awake() {
        base.Awake();  
        this.KRA = new KiwiRegularAttack(this); 
    }
    
    protected override void Update() {
        base.Update(); 

         if(attacking) {
           this.KRA.highlightAttackArea(); 
           if(this.KRA.getLockedSpecial()) {
               this.attacking = false; 
               TurnPlanner.turnActions.Add(this.KRA); 
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
