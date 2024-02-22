using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yuspinmi : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float speed = 2f;
    void Update()
    {
        cube.transform.Rotate(new Vector3(0,speed,0));
    }
}
