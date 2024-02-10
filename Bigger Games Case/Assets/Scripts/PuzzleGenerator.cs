using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleGenerator : MonoBehaviour
{
    [SerializeField] private Piece piecePrefab;
    [SerializeField] private Node nodePrefab;
    [SerializeField] private GridTable gridTable;
    [SerializeField] private Vector2Int  puzzleSize = new(4,4);
    [SerializeField] private Vector2 noise = new Vector2(0.5f,0.8f);
    
    private List<Piece> _pieces = new();
    private Color[] _pieceColors;
    private float _noiseScale;
    private Node[,] _nodeGrid = new Node[24, 24];
    private static readonly Vector2 ShuffleMax = new(-5, -8);
    private static readonly Vector2 ShuffleMin = new(-2, 3);

    private void Start()
    {
        GenerateGridTable();
        GeneratePuzzle();
    }

    private void GenerateGridTable()
    {
        gridTable.GenerateGrid(puzzleSize);
    }

    public void GeneratePuzzle()
    {
        
        _pieceColors = GenerateRandomColors((puzzleSize.x +puzzleSize.y)/2);

        _noiseScale = Random.Range(noise.x, noise.y);
        Debug.Log(_noiseScale);
        for (int i = 0; i < puzzleSize.x; i++)
        {
            for (int j = 0; j < puzzleSize.y; j++)
            {
                float noiseValue = Mathf.PerlinNoise((i * _noiseScale) + Random.Range(noise.x, noise.y),
                    (j * _noiseScale) + Random.Range(-noise.x, noise.y));
              
                Color pieceColor = _pieceColors[CalculateColorIndex(noiseValue)];
                var node = Instantiate(nodePrefab, new Vector3(i, j, 0), Quaternion.identity);
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
        foreach (var piece in _pieces)
        {
            if (piece.HasSingleNode())
            {
                piece.RemoveAllNodes();
            }
        }
        _pieces.RemoveAll(x => x == null);
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

        ChangeGroupColor(piece);
    }

    private void ChangeGroupColor(Piece piece)
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);
        piece.SetNodesColor(newColor);
    }

    private void Shuffle()
    {
        foreach (Piece p in _pieces)
        {
            p.transform.position = new Vector3(Random.Range(ShuffleMin.x, ShuffleMin.y),
                Random.Range(ShuffleMax.x, ShuffleMax.y), 0f);
        }
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
        return Mathf.FloorToInt(noiseValue * puzzleSize.x);
    }
}