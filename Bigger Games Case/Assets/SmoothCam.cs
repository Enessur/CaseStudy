using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SmoothCam : MonoBehaviour
{
    [SerializeField] private CustomGridSize customGridSize;
    
    public CinemachineVirtualCamera virtualCamera;
    public Transform target;
    private CinemachineFramingTransposer framingTransposer;
    private int gridXBaseValue;
    private int gridYBaseValue;

    

    public void OnGridSizeChange()
    {
        gridXBaseValue = (customGridSize.GridXValue)/2;
        gridYBaseValue = customGridSize.GridYValue;
        
        
        int x = gridXBaseValue / 2;
        
        target.position = new Vector3(x,0, -10);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(6,12.5f,Mathf.InverseLerp(4,12, gridYBaseValue));

    }
    
}
