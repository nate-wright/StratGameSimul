using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum PieceTypes {
    None = 0, 
    Chilli = 1,
    Kiwi = 2,
    DogPBR = 3,
    Base = 4
}
public class Piece : MonoBehaviour
{
    [SerializeField] protected GameObject privateHealthText; 
    [SerializeField] protected GameObject privateAttackText; 
    public int team; 
    public PieceTypes type; 
    public int currentX; 
    public int currentY; 
    [SerializeField] public int health; 
    [SerializeField] public int attack; 

    public int speed; 

    [SerializeField] protected bool moveable; 

    public Board b; 

    private Vector3 desiredPosition; 
    //private Vector3 desiredScale = new Vector3(5, 5, 5); 
    private Vector3 desiredScale; 
    Text mTextOverHead;

    Text AttackOverhead; 

    protected GameObject prefab; 
    private TextMesh text; 

    private GameObject prefab2; 

    private TextMesh text2; 

    private Outline outline;

    protected virtual void Awake() {
        ShowAttack(); 
        ShowHealth(); 

        this.outline = gameObject.AddComponent<Outline>();

        this.outline.OutlineMode = Outline.Mode.OutlineAll;
        this.outline.OutlineColor = Color.green;
        this.outline.OutlineWidth = 5f;
        this.outline.enabled = false;
        
    }

    public bool getMoveable() {
        return this.moveable; 
    }

    private void ShowAttack() {
        if(privateAttackText) {
            prefab2 = Instantiate(privateAttackText, transform.position, Quaternion.identity); 
            
            text2 = prefab2.GetComponentInChildren<TextMesh>(); 
            text2.text = "5";
            prefab2.transform.rotation = Quaternion.Euler(90, 0, 0);  
            
            prefab2.transform.position = new Vector3(2.0f, 0.2f, 3.6f);  

            text2.transform.position = new Vector3(2.0f, 0.2f, 3.6f); 

            text2.transform.rotation = Quaternion.Euler(90, 0, 0); 

            text2.color = Color.blue; 
        }
    }
    private void ShowHealth() {
        if(privateHealthText) {
            prefab = Instantiate(privateHealthText, transform.position, Quaternion.identity); 
            
            text = prefab.GetComponentInChildren<TextMesh>(); 
            text.text = "20";
            prefab.transform.rotation = Quaternion.Euler(90, 0, 0);  
            
            prefab.transform.position = new Vector3(1.6f, 0.2f, 3.6f);  

            text.transform.position = new Vector3(1.6f, 0.2f, 3.6f); 

            text.transform.rotation = Quaternion.Euler(90, 0, 0); 

        }
    }
    protected virtual void Update() {
        if(privateHealthText && prefab && text) {
            text.transform.position = GetTileCenter(currentX, currentY) + new Vector3(-0.2f, 1f, 0.3f); 
            prefab.transform.position = GetTileCenter(currentX, currentY) +  new Vector3(0, 1f, 0); 
            text.text = health.ToString(); 
        }

        if(privateAttackText && prefab2 && text2 ) {
            text2.transform.position = GetTileCenter(currentX, currentY) + new Vector3(0.4f, 1f, 0.3f); 
            prefab2.transform.position = GetTileCenter(currentX, currentY) +  new Vector3(0.4f, 1f, 0); 
            text2.text = attack.ToString(); 
        }

        if(health <= 0) {
            Board.allPieces[currentX, currentY] = null; 
            Destroy(this.gameObject);
            Destroy(this.prefab); 
            Destroy(this.prefab2); 
            Destroy(this.text);
            Destroy(this.text2); 
            Destroy(this);  
        }
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        //transform.localScale = Vector3.Lerp(transform.position, desiredScale, Time.deltaTime * 10); 

    }
    public virtual void SetPosition(Vector3 position, bool force = false) {
        desiredPosition = position;
        if(force) {
            transform.position = desiredPosition;
        }
    }


    public virtual void SetScale(Vector3 scale, bool force = false) {
        desiredScale = scale;
        if(force) {
            transform.localScale = desiredPosition;
        }
    }

    public virtual void Move(List<BoardTile> l, List<LineRenderer> lines) {
        StartCoroutine(Movement());

        IEnumerator Movement()
        {
            BoardTile lastTile = l[l.Count - 1];
            bool invalidMove = true;  
            if(!(lastTile.x > this.currentX + 1 || lastTile.x < this.currentX - 1)) {
                if(lastTile.y == this.currentY + 1 || lastTile.y == this.currentY - 1) {
                    invalidMove = false;
                }
            }
            if(!invalidMove) {
                for(int i = 0; i < l.Count; i++) {
                    BoardTile thisTile = l[i]; 
                    yield return new WaitForSeconds(0.5f);
                    //transform.position = Vector3.Lerp(transform.position, GetTileCenter(thisTile.x, thisTile.y), Time.deltaTime * 10);
                    if(Board.allPieces[thisTile.x, thisTile.y] is Piece) {
                        StandardAttack(Board.allPieces[thisTile.x, thisTile.y]); 
                        break; 
                    }
                    
                    transform.position = GetTileCenter(thisTile.x, thisTile.y); 

                    if(i - 1 < lines.Count && (i-1) != -1) {
                        LineRenderer thisLine = lines[i - 1]; 
                        Destroy(thisLine); 
                    }

                    Board.allPieces[this.currentX, this.currentY] = null; 
                    Board.allPieces[thisTile.x, thisTile.y] = this;
                    this.currentX = thisTile.x; 
                    this.currentY = thisTile.y; 

                }
            } else {
                Board.allPieces[this.currentX, this.currentY] = this; 
            }

            for(int j = 0; j < lines.Count; j++) {
                Destroy(lines[j]); 
            }

            // this.currentX = l[l.Count - 1].x;
            // this.currentY = l[l.Count - 1].y;
        }
        
    }

    public virtual void EndSpecialAttack() {
        Debug.Log("Ending special attack"); 
    }

    public virtual void regularAttack() {
        Debug.Log("yo"); 
    }

    public void StandardAttack(Piece p) {
        p.health -= attack; 
        health -= p.attack; 
    }
    public Vector3 GetTileCenter(int x, int y) {
       Vector3 bounds = new Vector3(Board.TILE_COUNT_X / 2 * 1.0f, 0, Board.TILE_COUNT_Y / 2 * 1.0f); 
       return new Vector3(x + 0.6f, 0.2f, y + 0.6f) - bounds;
    }

    public void setOutline() {
        
        this.outline.enabled = true; 
    }

    public void removeOutline() {
        this.outline.enabled = false; 

    }
}
