using BaseTemplate.Behaviours;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] GameObject _beamPrefab;
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] float _speed;

    GameObject _lastBeam;
    bool _isBeamExist;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isBeamExist) CastBeam();
            else TeleportToBeam();
        }
    }

    void CastBeam()
    {
        _isBeamExist = true;


        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; 
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        _lastBeam = Instantiate(_beamPrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRigidbody = _lastBeam.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (targetPosition - transform.position);

        projectileRigidbody.velocity = shootDirection.normalized * _speed;


        _targetGroup.AddMember(_lastBeam.transform, 2, 0);
    }

    void TeleportToBeam()
    {
        _RB.velocity= Vector3.zero;
        transform.position = _lastBeam.transform.position;

        _targetGroup.RemoveMember(_lastBeam.transform);

        Destroy(_lastBeam);

        _isBeamExist = false;
    }
}
