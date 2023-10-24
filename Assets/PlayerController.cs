using BaseTemplate.Behaviours;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] PlayerAnimatorController _playerAnimator;
    [SerializeField] LayerMask _isGroundedLayerMask;
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] CinemachineTargetGroup _targetGroup;

    [SerializeField] float _bladeSpeed, _maxFallSpeed;

    GameObject _lastBeam;
    bool _canCastBlade;

    public Rigidbody2D RB { get => _RB; }
    public bool CanCastBlade { get => _canCastBlade; }

    public void Init()
    {
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

        return raycastHit.collider != null;
    }

    public bool IsWalled(Vector2 direction)
    {
        float extraHeightText = .05f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, direction, extraHeightText, _isGroundedLayerMask);

        if (raycastHit.collider != null)
        {
            if (_RB.velocity.y <= 0f) _RB.velocity = new Vector2(_RB.velocity.x, -1);

            return true;
        }
        else return false;
    }

    public void CastBlade(Vector3 direction)
    {
        if (_canCastBlade == false) return;

        _canCastBlade = false;

        _lastBeam = PoolManager.Instance.SpawnFromPool("Blade", transform.position, Quaternion.identity);
        Rigidbody2D projectileRigidbody = _lastBeam.GetComponent<Rigidbody2D>();

        projectileRigidbody.velocity = direction * _bladeSpeed;

        _targetGroup.AddMember(_lastBeam.transform, 1, 0);
    }

    public void TeleportToBlade()
    {
        _RB.velocity = Vector3.zero;

        _lastBeam.SetActive(false);

        transform.DOMove(_lastBeam.transform.position, .1f).OnComplete(() =>
        {
            _targetGroup.RemoveMember(_lastBeam.transform);

            _canCastBlade = true;
        });
    }
}
