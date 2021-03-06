using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _cameraCinemachine;
    private Transform transformaMapaActual;

    private void Awake()
    {
        _cameraCinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetMapTransform(Transform newMap)
    {
        transformaMapaActual = newMap;
        _cameraCinemachine.Follow = transformaMapaActual;
    }
}
