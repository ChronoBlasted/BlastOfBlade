using BaseTemplate.Behaviours;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] CinemachineVirtualCamera _mainVCam;
    [SerializeField] CinemachineShake _mainShake;

    public void SwitchCameraTo(GameObject gameObject)
    {
        _mainVCam.Follow = gameObject.transform;
        _mainVCam.LookAt = gameObject.transform;
    }

    public void ShakeCamera(float intensity = 4, float duration = .125f)
    {
        _mainShake.ShakeCamera(intensity, duration);
    }
}
