using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnPlanner {

    public static List<Action> turnActions = new List<Action>();

    public static bool executing = false; 

    MonoBehaviour mono; 

    public TurnPlanner(MonoBehaviour mono) {
        this.mono = mono; 
    }

    public void ExecuteTurn() {
        TurnPlanner.executing = true; 
        Debug.Log("TURN ACTIONS COUNT"); 
        Debug.Log(TurnPlanner.turnActions.Count); 
        //Array.Sort(this.turnActions, delegate(Action x, Action y) { return x.thisPiece.speed.CompareTo(y.thisPiece.speed); });
        TurnPlanner.turnActions.Sort((x, y) => x.thisPiece.speed.CompareTo(y.thisPiece.speed));
        mono.StartCoroutine(ExampleCoroutine()); 
        
        
    }

    
   IEnumerator ExampleCoroutine(){
        //Print the time of when the function is first called.
        TurnPlanner.executing = true; 
        WaitForSeconds wait = new WaitForSeconds( 2f ) ;
        //yield on a new YieldInstruction that waits for 5 seconds.

         for(int i = 0; i < TurnPlanner.turnActions.Count; i++) {
            Action a = TurnPlanner.turnActions[i]; 
            a.executeAction();
            yield return wait;
            
        }

        Board.pieceMovement = new List<PieceMovement>(); 

        TurnPlanner.turnActions.Clear(); 

        

        //After we have waited 5 seconds print the time again.
        TurnPlanner.executing = false; 
    }

}

 
