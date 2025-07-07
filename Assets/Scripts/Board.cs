using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap{get; private set;}
    public Piece activePiece{get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        // Make sure tetrominos array is not null and has elements
        if (tetrominos == null || tetrominos.Length == 0)
        {
            Debug.LogError("Tetrominos array is not set in the Inspector!");
            return;
        }

        // Initialize each tetromino
        for (int i = 0; i < tetrominos.Length; i++)
        {
            if (tetrominos[i].tile == null)
            {
                Debug.LogError($"Tetromino at index {i} has no tile assigned!");
                continue;
            }
            tetrominos[i].Initialize();
        }
    }

    private void Start() {
        SpawnPiece();
    }

    public void SpawnPiece(){
        if (tetrominos == null || tetrominos.Length == 0) {
            Debug.LogError("Tetrominos array is null or empty!");
            return;
        }
        
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];
        
        if (data.cells == null) {
            Debug.LogError($"Tetromino data cells are null for piece type {data.tetromino}! Make sure Initialize() was called.");
            return;
        }
        
        if (this.activePiece == null) {
            Debug.LogError("Active piece is null! Make sure there is a Piece component in children.");
            return;
        }
        
        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece){
        foreach (var cell in piece.cells){
            Vector3Int tilePosition = cell + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
}
