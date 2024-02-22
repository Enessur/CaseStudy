using Cinemachine;
using UnityEngine;

public class SmoothCam : MonoBehaviour
{
    [SerializeField] private CustomGridSize customGridSize;

    public CinemachineVirtualCamera virtualCamera;
    public Transform target;
    private CinemachineFramingTransposer framingTransposer;


    public void OnGridSizeChange(Vector2Int size)
    {
        int x = size.x / 2;
        int gridSize = Mathf.RoundToInt(size.x);

        if (gridSize % 2 == 0)
        {
            target.position = new Vector3(x - 0.5f, -5.5f, -10);
        }
        else
        {
            target.position = new Vector3(x, -5.5f, -10);
        }

        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(6, 17.5f, Mathf.InverseLerp(1, 12, size.y));
    }
}