using System;
using UnityEngine;

public class GridPlane : MonoBehaviour
{
    public static event Action OnEnterAction;
    public static event Action OnExitAction;
    [SerializeField] private MatrixNode matrixNodePrefab;
    
    private MatrixNode[,] _matrixNodes = new MatrixNode[24,24];
    private bool _isEmpty;
    private int _stackedPiecesCount;
    private void Start()
    {
        Init();
        _isEmpty = true;
        _stackedPiecesCount = 0;
    }

    private void Init()
    {
        //for each ile matrix prefablerini initle 
       
    }

    //todo: matrix node içini oluşturulan matrix node prefeableri ile doldur ardından 
    //todo: matrix notlarını initialize et 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEmpty && _stackedPiecesCount == 0)
        {
            OnEnterAction?.Invoke();
        }
        _stackedPiecesCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _stackedPiecesCount--;
        if (_isEmpty && _stackedPiecesCount == 0)
        {
            OnExitAction?.Invoke();
        }
    }
}