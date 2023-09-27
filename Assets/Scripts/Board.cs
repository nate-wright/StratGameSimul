using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial; 
    [SerializeField] private float tileSize  = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = new Vector3(1f, 0, 1f); 

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs; 
    [SerializeField] private Material[] teamMaterials; 

    public static Piece[,] allPieces; 
    static public int TILE_COUNT_X = 5; 
    static public int TILE_COUNT_Y = 6; 
    public static GameObject[,] tiles; 
    private Camera currentCamera;
    private Vector3 bounds; 

    private Piece currentlyDragging; 

    public static Piece currentlySelected;

    private BoardTile curDragTile; 

    List<BoardTile> curDragTiles = new List<BoardTile>(); 

    List<LineRenderer> curDragLines = new List<LineRenderer>(); 

    public static List<PieceMovement> pieceMovement = new List<PieceMovement>(); 

    private Vector2Int currentHover;  

    static public GameObject currentlyHoveringTile = null; 

    static public int currentHoverX = -1; 
    static public int currentHoverY = -1;

    private RenderPath pathRenderer = new RenderPath(); 

    private Vector3 lineOffset = new Vector3(0, 0.2f, 0); 

   private void Awake() {
       GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y); 

        Vector3 pos = new Vector3(0f, 8f, 0f); 

        currentlySelected = null; 
        

        Camera.main.transform.SetPositionAndRotation(pos, Quaternion.Euler(90, 0, 0)); 

        SpawnAllPieces(); 
        PositionAllPieces(); 

   }

   private void Update() {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "Attack_Range")))
        {
            // Get the indexes of the tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            // If we're hovering a tile after not hovering any tiles
            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                if(tiles[currentHover.x, currentHover.y].layer != LayerMask.NameToLayer("Attack_Range")) {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                }
                
                currentlyHoveringTile = tiles[hitPosition.x, hitPosition.y]; 
                currentHoverX = hitPosition.x; 
                currentHoverY = hitPosition.y; 
            }

            // If we were already hovering a tile, change the previous one
            if (currentHover != hitPosition)
            {
                if(tiles[currentHover.x, currentHover.y].layer != LayerMask.NameToLayer("Attack_Range")) {
                    tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");

                }

                
                currentHover = hitPosition;
                if(tiles[hitPosition.x, hitPosition.y].layer == LayerMask.NameToLayer("Tile")) {
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");

                }
                currentlyHoveringTile = tiles[hitPosition.x, hitPosition.y]; 
                currentHoverY = hitPosition.y; 
                currentHoverX = hitPosition.x; 
            }

            if(Input.GetMouseButton(0)) {
                BoardTile thisTile = new BoardTile(hitPosition.x, hitPosition.y);

                if(currentlyDragging) {
                    if(curDragTiles.Count > 0) {
                        if(!(thisTile.isEqualTo(curDragTiles[curDragTiles.Count - 1]))) {
                            BoardTile lastTile = curDragTiles[curDragTiles.Count - 1]; 
                            curDragLines.Add(pathRenderer.RenderALine(GetTileCenter(lastTile.x, lastTile.y) + lineOffset, GetTileCenter(hitPosition.x, hitPosition.y) + lineOffset)); 
                            curDragTiles.Add(thisTile);  
                        }
                    } else {
                        curDragTiles.Add(thisTile); 
                    }
                }
                
            
                if(allPieces[hitPosition.x, hitPosition.y] != null && currentlyDragging == null) {
                    //is it planning stage 
                    if(allPieces[hitPosition.x, hitPosition.y].getMoveable()) {
                        currentlyDragging = allPieces[hitPosition.x, hitPosition.y];
                    }
                }
            }

            if(Input.GetMouseButtonDown(0)) {
                if(allPieces[hitPosition.x, hitPosition.y] is Piece) {
                    if(currentlySelected is Piece) {
                        currentlySelected.removeOutline(); 
                    }
                    currentlySelected = allPieces[hitPosition.x, hitPosition.y]; 
                    currentlySelected.setOutline(); 
                }
            }

            //move piece when mouse is done clicking 
            
        }
        else
            {
                if (currentHover != -Vector2Int.one)
                {
                    if(tiles[currentHover.x, currentHover.y].layer != LayerMask.NameToLayer("Attack_Range")) {
                        tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                    }
        
                    currentlyHoveringTile = null; 
                    currentHoverX = -1; 
                    currentHoverY = -1; 
                    currentHover = -Vector2Int.one;
                }

            }

        //if we're dragging want visuals 

        if(currentlyDragging) {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset); 
        }
        if(currentlyDragging != null && Input.GetMouseButtonUp(0)) {

            PieceMovement m = new PieceMovement(); 
            m.theseLines = curDragLines; 
            m.theseTiles = curDragTiles; 
            m.thisPiece = currentlyDragging; 

            pieceMovement.Add(m); 

            

            curDragTiles = new List<BoardTile>();  

            curDragLines = new List<LineRenderer>(); 

            currentlyDragging = null; 
        }
        
    
    }

    private bool MoveTo(Piece currentlyDragging, int x, int y) {
        Vector2Int previousePosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY); 

        if(allPieces[x, y] != null) {
            Piece ocp = allPieces[x, y]; 

            if(ocp.team == currentlyDragging.team) {
                return false; 
            }
        }

        allPieces[x, y] = currentlyDragging; 
        allPieces[previousePosition.x, previousePosition.y] = null; 

        PositionSinglePiece(x, y); 

        return true; 
    }

   private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY) {
       yOffset += transform.position.y; 
       bounds = new Vector3(tileCountX / 2 * tileSize, 0, tileCountY / 2 * tileSize) + boardCenter; 

       tiles = new GameObject[tileCountX, tileCountY];
       for(int i = 0; i< tileCountX; i++) {
           for(int y = 0; y < tileCountY; y++) {
               tiles[i, y] = GenerateSingleTile(tileSize, i, y);
           }
       }
   }

   private GameObject GenerateSingleTile(float tileSize, int x, int y) {
       GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
       tileObject.transform.parent = transform; 

        Mesh mesh = new Mesh(); 
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial; 

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds ; 
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;  
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;  

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices; 
        mesh.triangles = tris; 

        mesh.RecalculateNormals(); 

        tileObject.layer = LayerMask.NameToLayer("Tile");

        tileObject.AddComponent<BoxCollider>(); 
       return tileObject; 
   }

    //testing spawing pieces 

    private Piece SpawnSinglePiece(PieceTypes type, int team) {
        Piece p = Instantiate(prefabs[(int) type - 1], transform).GetComponent<Piece>(); 

        p.type = type; 
        p.team = team; 
        p.GetComponent<MeshRenderer>().material = teamMaterials[team];  

        return p; 
    }

    private void SpawnAllPieces() {
        allPieces = new Piece[TILE_COUNT_X, TILE_COUNT_Y]; 
        int player = 0; 
        int enemy = 1; 

        //player; 
        allPieces[0, 1] = SpawnSinglePiece(PieceTypes.DogPBR, player);
        allPieces[1, 1] = SpawnSinglePiece(PieceTypes.Chilli, player);
        allPieces[3, 1] = SpawnSinglePiece(PieceTypes.Kiwi, player);
        allPieces[2, 0] = SpawnSinglePiece(PieceTypes.Base, player); 

        //enemy 
        allPieces[0, 4] = SpawnSinglePiece(PieceTypes.DogPBR, enemy);
        allPieces[1, 4] = SpawnSinglePiece(PieceTypes.Chilli, enemy);
        allPieces[3, 4] = SpawnSinglePiece(PieceTypes.Kiwi, enemy);  
        allPieces[2, 5] = SpawnSinglePiece(PieceTypes.Base, enemy);      

    }


    //Positioning 
    private void PositionAllPieces() {
        for(int i = 0; i < TILE_COUNT_X; i++) {
            for(int y = 0; y < TILE_COUNT_Y; y++) {
                if(allPieces[i, y] != null) {
                    PositionSinglePiece(i, y, true); 
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false) {
        allPieces[x, y].currentX = x; 
        allPieces[x, y].currentY = y; 
        allPieces[x, y].SetPosition(new Vector3(x * tileSize + 0.6f, yOffset, y * tileSize + 0.6f) - bounds, force); 
    }
   public static Vector2Int LookupTileIndex(GameObject hitInfo) {
       for(int i = 0; i < TILE_COUNT_X; i++) {
           for(int y = 0; y < TILE_COUNT_Y; y++) {
               if(tiles[i, y] == hitInfo) {
                   return new Vector2Int(i, y); 
               }
           }
       }

        //invalid
       return -Vector2Int.one; 
   }

   public Vector3 GetTileCenter(int x, int y) {
       return new Vector3(x * tileSize + 0.6f, yOffset, y * tileSize + 0.6f) - bounds;
   }
}
