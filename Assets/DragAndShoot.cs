using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    [SerializeField] float _power = 10;
    [SerializeField] float _maxDrag = 5;
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] LineRenderer _lineRenderer;

    Vector3 _startPoint;
    Vector3 _endPoint;
    Touch _touch;

    private void Update()
    {
        if (PlayerController.Instance.IsGrounded() || PlayerController.Instance.IsWalled(Vector2.left) || PlayerController.Instance.IsWalled(Vector2.right))
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began)
                {
                    DragStart();
                }

                if (_touch.phase == TouchPhase.Moved)
                {
                    Dragging();
                }

                if (_touch.phase == TouchPhase.Ended)
                {
                    DragRelease();
                }
            }
        }
    }

    void DragStart()
    {
        _startPoint = Camera.main.ScreenToWorldPoint(_touch.position);
        _startPoint.z = 0;
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _startPoint);
    }

    void Dragging()
    {
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(_touch.position);
        draggingPos.z = 0;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(1, draggingPos);
    }

    void DragRelease()
    {
        _lineRenderer.positionCount = 0;

        _endPoint = Camera.main.ScreenToWorldPoint(_touch.position);
        _endPoint.z = 0;

        Vector3 force = _startPoint - _endPoint;

        Debug.Log(force);

        Vector3 clampedForce = Vector3.ClampMagnitude(force, _maxDrag) * _power;

        Debug.Log(clampedForce);

        PlayerController.Instance.CastBlade(clampedForce);
    }
}
