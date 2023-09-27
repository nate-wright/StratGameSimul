using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile
{
    public int x; 
    public int y; 
    public BoardTile(int x, int y) {
        this.x = x; 
        this.y = y; 
    }
    
    public bool isEqualTo(BoardTile b) {
        if(b.x == this.x && b.y == this.y) {
            return true; 
        }
        return false; 
    }
}
