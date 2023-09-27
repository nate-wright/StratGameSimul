using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AttackButton : MonoBehaviour
{
    public Button attack; 
    //public Board thisBoard; 
    // Start is called before the first frame update
    void Start()
    {
        //thisBoard = GetComponent<Board>(); 
        attack.onClick.AddListener(EndSpecialAttack); 

    }

    // Update is called once per frame
    void Update()
    {
        if(TurnPlanner.executing && attack.interactable) {
            attack.interactable = false; 
        } else if(!TurnPlanner.executing && attack.interactable == false) {
            attack.interactable = true; 
        }    
    }

    void EndSpecialAttack() {
        Debug.Log("Attacking"); 
        //Board.currentlySelected.EndSpecialAttack(); 
        Board.currentlySelected.regularAttack(); 
        

    }

}