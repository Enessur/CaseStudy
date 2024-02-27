using System;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleGenerator : MonoBehaviour, IResetable
{
    public static PuzzleGenerator Instance;

    [SerializeField] private ShaderScriptable shaderScriptable;
    [SerializeField] private Piece piecePrefab;
    [SerializeField] private Node nodePrefab;
    [SerializeField] private Vector2 noise = new Vector2(0.5f, 0.8f);
    public GridTable GridTable => gridTable;
    [SerializeField] private GridTable gridTable;

    private List<int> _selectedMaterialIndices = new List<int>();
    public Vector2Int puzzleSize = new(4, 4);
    public static Action OnAllPiecesPlaced;
    public static Action OnNewPuzzleCreated;
    private List<PieceSize> _pieceSizeList = new();
    private PieceSize _pieceItem;
    private List<Piece> _pieces = new();
    private Color[] _pieceColors;
    private float _noiseScale;
    private Node[,] _nodeGrid = new Node[24, 24];
    private int _totalPieceCount;
    private int _placedPieceCount;
    private bool _first;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreatePuzzle();
    }

    private void CreatePuzzle()
    {
        GenerateGridTable();
        OnNewPuzzleCreated?.Invoke();
        GeneratePuzzle();
        _pieces.RemoveAll(x => x == null);
        _totalPieceCount = _pieces.Count;
    }

    public void OnEnable()
    {
        ((IResetable)this).Subscription();
        Piece.onPieceStateChanged += OnPieceStateChanged;
    }

    public void OnDisable()
    {
        ((IResetable)this).Unsubscription();
        Piece.onPieceStateChanged -= OnPieceStateChanged;
    }

    private void OnPieceStateChanged(bool state, Piece piece)
    {
        if (state)
        {
            _placedPieceCount++;
        }
        else
        {
            _placedPieceCount--;
        }

        if (_totalPieceCount == _placedPieceCount)
        {
            SoundManager.Instance.PlaySound("Complete");
            OnAllPiecesPlaced?.Invoke();
        }
    }


    private void GenerateGridTable()
    {
        gridTable.GenerateGrid(puzzleSize);
    }

    public void GeneratePuzzle()
    {
        _pieceColors = GenerateRandomColors((puzzleSize.x + puzzleSize.y) / 2);

        _noiseScale = Random.Range(noise.x, noise.y);
        for (int i = 0; i < puzzleSize.x; i++)
        {
            for (int j = 0; j < puzzleSize.y; j++)
            {
                float noiseValue = Mathf.PerlinNoise((i * _noiseScale) + Random.Range(noise.x, noise.y),
                    (j * _noiseScale) + Random.Range(-noise.x, noise.y));

                Color pieceColor = _pieceColors[CalculateColorIndex(noiseValue)];
                Quaternion rotation = Quaternion.Euler(-90, 0, 0);
                var node = Instantiate(nodePrefab, new Vector3(i, j, 0), rotation);
                node.Init();
                node.SetColor(pieceColor);
                _nodeGrid[i, j] = node;
            }
        }

        GroupPuzzle();
    }

    private void GroupPuzzle()
    {
        for (int i = 0; i < puzzleSize.x; i++)
        {
            for (int j = 0; j < puzzleSize.y; j++)
            {
                if (!_nodeGrid[i, j].HasPiece)
                {
                    CreateGroup(i, j);
                }
            }
        }

        RemoveParentFromSingleElements();
        RegroupSinglePieceBlocks();
        foreach (var piece in _pieces)
        {
            piece.RecalculateCoordinates();
        }

        Shuffle();
    }

    private void CreateGroup(int startX, int startY)
    {
        Color groupColor = _nodeGrid[startX, startY].Color;
        var piece = CreatePiece(groupColor);
        DFS(startX, startY, groupColor, piece);
    }

    private Piece CreatePiece(Color groupColor)
    {
        var piece = Instantiate(piecePrefab);
        _totalPieceCount++;
        piece.Init(transform, groupColor);

        _pieces.Add(piece);
        return piece;
    }

    private void DFS(int x, int y, Color groupColor, Piece piece)
    {
        if (x < 0 || x >= puzzleSize.x || y < 0 || y >= puzzleSize.y)
        {
            return;
        }

        var node = _nodeGrid[x, y];
        if (node.HasPiece ||
            node.Color != groupColor)
        {
            return;
        }

        node.SetPiece(piece);
        piece.AddNode(node);

        DFS(x + 1, y, groupColor, piece);
        DFS(x - 1, y, groupColor, piece);
        DFS(x, y + 1, groupColor, piece);
        DFS(x, y - 1, groupColor, piece);
    }

    private void RemoveParentFromSingleElements()
    {
        List<Piece> destroyablePieces = new List<Piece>();
        foreach (var piece in _pieces)
        {
            if (piece.HasSingleNode())
            {
                destroyablePieces.Add(piece);
                piece.RemoveAllNodes();
            }
        }

        foreach (var piece in destroyablePieces)
        {
            _pieces.Remove(piece);
            Destroy(piece.gameObject);
        }
    }

    private void RegroupSinglePieceBlocks()
    {
        for (int i = 0; i < puzzleSize.x; i++)
        {
            for (int j = 0; j < puzzleSize.y; j++)
            {
                if (!_nodeGrid[i, j].HasPiece)
                {
                    ReGroupSingleElements(i, j);
                }
            }
        }
    }

    private void ReGroupSingleElements(int startX, int startY)
    {
        var node = _nodeGrid[startX, startY];
        var piece = CreatePiece(node.Color);

        SingleGroupDFS(startX, startY, piece);
    }

    private void SingleGroupDFS(int x, int y, Piece piece)
    {
        if (x < 0 || x >= puzzleSize.x || y < 0 || y >= puzzleSize.y)
        {
            return;
        }

        var node = _nodeGrid[x, y];
        if (node.HasPiece)
        {
            return;
        }

        node.SetPiece(piece);
        piece.AddNode(node);

        SingleGroupDFS(x + 1, y, piece);
        SingleGroupDFS(x - 1, y, piece);
        SingleGroupDFS(x, y + 1, piece);
        SingleGroupDFS(x, y - 1, piece);
    }

    float placeX, placeY, offset = 0.3f;

    private void Shuffle()
    {
        float maxPieceHeight = 0f; 
    
        foreach (Piece p in _pieces)
        {
            float rn = Random.Range(-0.2f, 0.2f);
            Vector2Int pieceSize = p.ReturnPieceSize();
            maxPieceHeight = Mathf.Max(maxPieceHeight, pieceSize.y);  

            if (_pieces.IndexOf(p) > 0)
            {
                float startX = _pieces[_pieces.IndexOf(p) - 1].transform.position.x + _pieces[_pieces.IndexOf(p) - 1].ReturnPieceSize().x / 2f + pieceSize.x / 2f + 0.2f;
                float startY = _pieces[_pieces.IndexOf(p) - 1].transform.position.y;

                if (startX + pieceSize.x / 2f > puzzleSize.x + 2f)
                {
                    startX = -1f;
                    startY -= maxPieceHeight / 2f + pieceSize.y / 2f + 0.2f;
                }

                p.transform.position = new Vector3(startX, startY, rn);
            }
            else  
            {
                p.transform.position = new Vector3(-1f, -2-pieceSize.y / 2f, rn);
            }
        
            int randomMaterialIndex = GetRandomMaterialIndex();  
            p.ShiftNodesToOrigin();  
            p.SetNodesMaterial(shaderScriptable.Materials[randomMaterialIndex]);  
            p.StartPosition();  
            p.SpawnAnimation();
        }
    }


    // private Piece FindBiggestPiece(PieceSize pieceSize)
    // {
    //     for (int i = 0; i < _pieceSizeList.Count; i++)
    //     {
    //         piecePrefab = _pieceSizeList[i].piece;
    //         var currentPieceSize = _pieceSizeList[i].pieceSizeX + _pieceSizeList[i].pieceSizeY;
    //         int biggest = currentPieceSize;
    //         if (currentPieceSize>biggest)
    //         {
    //             biggest = currentPieceSize;
    //         }
    //     }
    //
    //     return piecePrefab;
    // }

    private int GetRandomMaterialIndex()
    {
        int maxIndex = shaderScriptable.Materials.Count;
        int randomIndex = UnityEngine.Random.Range(0, maxIndex);

        while (_selectedMaterialIndices.Contains(randomIndex))
        {
            randomIndex = UnityEngine.Random.Range(0, maxIndex);
        }

        _selectedMaterialIndices.Add(randomIndex);
        if (_selectedMaterialIndices.Count == maxIndex)
        {
            _selectedMaterialIndices.Clear();
        }

        return randomIndex;
    }

    private Color[] GenerateRandomColors(int n)
    {
        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
        {
            colors[i] = new Color(Random.value, Random.value, Random.value);
        }

        return colors;
    }

    private int CalculateColorIndex(float noiseValue)
    {
        return Mathf.FloorToInt(noiseValue * ((puzzleSize.x + puzzleSize.y) / 2));
    }

    public void CreateLevel(Vector2Int size)
    {
        puzzleSize = size;
        ResetPuzzle();
    }

    void IResetable.Reset()
    {
        ResetPuzzle();
    }

    private void ResetPuzzle()
    {
        foreach (var piece in _pieces)
        {
            Destroy(piece.gameObject);
        }

        gridTable.RemoveGrids();
        placeX = 0;
        placeY = 0;
        _totalPieceCount = 0;
        _placedPieceCount = 0;
        _pieces.Clear();
        CreatePuzzle();
    }
}