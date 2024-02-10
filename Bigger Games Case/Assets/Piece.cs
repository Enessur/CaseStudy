using System.Collections.Generic;
using Lean.Touch;
using UnityEditor;
using UnityEngine;

public class Piece : PuzzleItem
{
    [SerializeField] protected LeanDragTranslate leanDragTranslate;
    [SerializeField] protected LeanSelectableByFinger leanSelectableByFinger;
    [SerializeField] protected Rigidbody2D rigid;
    private int _nodeCount;
    private List<Node> _nodes = new();
    

    public void Init(Transform parent, Color groupColor)
    {
        transform.parent = parent;
        gameObject.name = $"Piece_{groupColor.r}.{groupColor.g}.{groupColor.b}";
    }

    public void AddNode(Node node)
    {
        _nodes.Add(node);
        _nodeCount++;
    }

    public void SetNodesColor(Color newColor)
    {
        foreach (Node node in _nodes)
        {
            node.SetColor(newColor);
        }
    }

    public bool CanNodesPlace()
    {
        foreach (var node in _nodes)
        {
            if (!node.CanPlace())
            {
                return false;
            }   
        }
        return true;
    }
    public void Shift(Vector3 value)
    {
        transform.position += value;
        // bu parça node un kayması gereken yere gitmesi için bu fonksiyonu çağırması lazım
    }

    public bool HasSingleNode()
    {
        return _nodeCount == 1;
    }

    public void RemoveAllNodes()
    {
        foreach (var node in _nodes)
        {
            node.RemoveFromNode();
        }
        Destroy(gameObject);
    }
}