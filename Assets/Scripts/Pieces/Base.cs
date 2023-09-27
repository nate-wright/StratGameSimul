using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Base : Piece {

    public override void SetPosition(Vector3 position, bool force = false) {
        base.SetPosition(position, force); 
        transform.position = transform.position + new Vector3(0.2f, 0.0f, -0.4f);
    }

    public override void Move(List<BoardTile> l, List<LineRenderer> lines) {
        Debug.Log("NOT MOVING"); 
    }
}