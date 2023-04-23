using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IHealth
{
    [SerializeField] Rigidbody2D _RB;
    [SerializeField] CapsuleCollider2D _collider;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Material _flashMaterial;
    [SerializeField] ParticleSystem _dieEffects;

    [SerializeField] int _health = 5;

    Material _baseMaterial;
    Coroutine _flashCoroutine;

    public void Start()
    {
        _baseMaterial = _sprite.material;
    }

    public bool TakeDamage(int amount)
    {
        _health -= amount;

        DoFlashSprite();

        if (_health <= 0)
        {
            _health = 0;

            _dieEffects.transform.parent = transform;
            _dieEffects.Play();
            _dieEffects.transform.parent = null;

            gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void TakeHeal(int amount)
    {
    }

    void DoFlashSprite(float duration = .125f)
    {
        _sprite.material = _baseMaterial;

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        _flashCoroutine = StartCoroutine(FLashCoroutine(duration));
    }

    IEnumerator FLashCoroutine(float duration)
    {
        _sprite.material = _flashMaterial;

        yield return new WaitForSeconds(duration);

        _sprite.material = _baseMaterial;
    }
}
