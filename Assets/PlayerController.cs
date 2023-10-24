using BaseTemplate.Behaviours;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoSingleton<PlayerController>, IHealth
{
    [SerializeField] PlayerAnimatorController _playerAnimator;
    [SerializeField] LayerMask _isGroundedLayerMask;
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] float _bladeSpeed, _forceJump, _movementSpeed, _maxFallSpeed;
    [SerializeField] int _health = 5;

    GameObject _lastBeam;
    bool _isBeamExist, _canCastBlade, _haveDoubleJump;
    Vector2 _moveValue, m_Velocity = Vector2.zero;

    public Rigidbody2D RB { get => _RB; }
    public Vector2 MoveValue { get => _moveValue; }
    public bool HaveDoubleJump { get => _haveDoubleJump; }

    public void Init()
    {
        _isBeamExist = false;
        _canCastBlade = true;
    }

    private void Update()
    {
        IsGrounded();
        IsWalled(Vector2.left);
        IsWalled(Vector2.right);
    }

    public bool IsGrounded()
    {
        float extraHeightText = .05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, extraHeightText, _isGroundedLayerMask);

        if (raycastHit.collider != null) _haveDoubleJump = true;

        return raycastHit.collider != null;
    }

    public bool IsWalled(Vector2 direction)
    {
        float extraHeightText = .05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, direction, extraHeightText, _isGroundedLayerMask);

        if (raycastHit.collider != null)
        {
            if (_RB.velocity.y <= 0f) _RB.velocity = new Vector2(_RB.velocity.x, -1);

            _haveDoubleJump = true;

            return true;
        }
        else return false;
    }

    public void CastBlade(Vector3 direction)
    {
        if (_canCastBlade == false) return;

        TimeManager.Instance.SetTime(1);

        if (_isBeamExist) return;

        _isBeamExist = true;

        _lastBeam = PoolManager.Instance.SpawnFromPool("Blade", transform.position, Quaternion.identity);
        Rigidbody2D projectileRigidbody = _lastBeam.GetComponent<Rigidbody2D>();

        projectileRigidbody.velocity = direction * _bladeSpeed;

        _targetGroup.AddMember(_lastBeam.transform, 1, 0);

    }

    public void TeleportToBlade()
    {
        if (_isBeamExist == false) return;

        _canCastBlade = false;

        _RB.velocity = Vector3.zero;

        _lastBeam.SetActive(false);

        transform.DOMove(_lastBeam.transform.position, .1f).OnComplete(() =>
        {
            _targetGroup.RemoveMember(_lastBeam.transform);

            _isBeamExist = false;

            _canCastBlade = true;
        });
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded() || _haveDoubleJump && IsWalled(Vector2.right) == false && IsWalled(Vector2.left) == false)
        {
            if (IsGrounded() == false) _haveDoubleJump = false;

            _RB.velocity = new Vector2(_RB.velocity.x, 0);

            _RB.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
        }
    }

    public bool TakeDamage(int amount)
    {
        _playerAnimator.PlayerHit();

        _health -= amount;

        if (_health <= 0)
        {
            _health = 0;
            /*
                        _dieEffects.transform.parent = transform;
                        _dieEffects.Play();
                        _dieEffects.transform.parent = null;*/

            gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void TakeHeal(int amount)
    {
        throw new System.NotImplementedException();
    }
}
