using BaseTemplate.Behaviours;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] GameObject _beamPrefab;
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] float _bladeSpeed, _forceJump, _movementSpeed;

    PlayerInputActions _playerInputActions;
    GameObject _lastBeam;
    bool _isBeamExist;

    public void Init()
    {
        _isBeamExist = false;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Jump.performed += Jump;
        _playerInputActions.Player.CastFirstBlade.performed += CastBlade;

        _playerInputActions.Player.Disable();
        _playerInputActions.Player.Jump.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnComplete(x =>
            {
                Debug.Log(x);
                x.Dispose();
                _playerInputActions.Player.Enable();

            })
            .Start();
    }

    private void FixedUpdate()
    {
        _RB.AddForce(_playerInputActions.Player.Movement.ReadValue<Vector2>().normalized * _movementSpeed, ForceMode2D.Force);
    }

    void CastBlade(InputAction.CallbackContext context)
    {
        if (_isBeamExist)
        {
            TeleportToBeam();
            return;
        }


        _isBeamExist = true;


        Vector3 mousePosition = Mouse.current.position.value;
        mousePosition.z = 10f;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        _lastBeam = Instantiate(_beamPrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRigidbody = _lastBeam.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (targetPosition - transform.position);

        projectileRigidbody.velocity = shootDirection.normalized * _bladeSpeed;


        _targetGroup.AddMember(_lastBeam.transform, 2, 0);

    }

    void TeleportToBeam()
    {
        _RB.velocity = Vector3.zero;
        transform.position = _lastBeam.transform.position;

        _targetGroup.RemoveMember(_lastBeam.transform);

        Destroy(_lastBeam);

        _isBeamExist = false;
    }

    void Jump(InputAction.CallbackContext context)
    {
        _RB.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
    }
}
