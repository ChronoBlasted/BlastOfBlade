using BaseTemplate.Behaviours;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] GameObject _beamPrefab;
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] float _bladeSpeed, _forceJump, _movementSpeed,_maxFallSpeed;

    PlayerInputActions _playerInputActions;
    GameObject _lastBeam;
    bool _isBeamExist;
    Vector2 _moveValue;
    private Vector3 m_Velocity = Vector3.zero;


    public void Init()
    {
        _isBeamExist = false;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Jump.performed += Jump;
        _playerInputActions.Player.CastFirstBlade.performed += CastBlade;

        /*_playerInputActions.Player.Disable();
        _playerInputActions.Player.Jump.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnComplete(x =>
            {
                Debug.Log(x);
                x.Dispose();
                _playerInputActions.Player.Enable();

            })
            .Start();*/
    }

    private void FixedUpdate()
    {
        _moveValue = _playerInputActions.Player.Movement.ReadValue<Vector2>().normalized * _movementSpeed;

        Vector3 targetVelocity = new Vector2(_moveValue.x, _RB.velocity.y);

        _RB.velocity = Vector3.SmoothDamp(_RB.velocity, targetVelocity, ref m_Velocity, .05f);

        if (_RB.velocity.y < -_maxFallSpeed)
        {
            _RB.velocity = new Vector2(_RB.velocity.x, -_maxFallSpeed);
        }
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

        _lastBeam = PoolManager.Instance.SpawnFromPool("Blade", transform.position, Quaternion.identity);
        Rigidbody2D projectileRigidbody = _lastBeam.GetComponent<Rigidbody2D>();
        Vector2 shootDirection = (targetPosition - transform.position);

        projectileRigidbody.velocity = shootDirection.normalized * _bladeSpeed;


        _targetGroup.AddMember(_lastBeam.transform, 1, 0);
    }

    void TeleportToBeam()
    {
        _RB.velocity = Vector3.zero;

        _lastBeam.SetActive(false);

        transform.DOMove(_lastBeam.transform.position, .1f).OnComplete(() =>
        {
            _targetGroup.RemoveMember(_lastBeam.transform);

            _isBeamExist = false;
        });
    }

    void Jump(InputAction.CallbackContext context)
    {
        _RB.velocity = new Vector2(_RB.velocity.x, 0);

        _RB.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
    }
}
