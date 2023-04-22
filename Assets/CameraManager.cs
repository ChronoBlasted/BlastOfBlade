using BaseTemplate.Behaviours;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera _mainVCam;

    public void SwitchCameraTo(GameObject gameObject)
    {
        _mainVCam.Follow = gameObject.transform;
        _mainVCam.LookAt = gameObject.transform;
    }
}
