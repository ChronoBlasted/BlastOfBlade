using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> _points = new List<Transform>();
    [SerializeField] float _delayBetweenPoints = 2, _pauseOnPoints = 2;

    private void Start()
    {
        DoMove();
    }

    void DoMove()
    {
        if (_points.Count <= 0) return;

        transform.position = _points[0].position;

        var MoveSequence = DOTween.Sequence();

        for (int i = 1; i < _points.Count; i++)
        {
            MoveSequence.Append(transform.DOMove(_points[i].position, _delayBetweenPoints).SetEase(Ease.Linear).SetDelay(_pauseOnPoints));
        }

        MoveSequence.SetLoops(-1, LoopType.Yoyo);

        MoveSequence.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
