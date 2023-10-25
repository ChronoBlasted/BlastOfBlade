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

    bool _isStartDone = false;

    PlayerController _playerController;
    Camera _camera;

    private void Start()
    {
        _playerController = PlayerController.Instance;
        _camera = Camera.main;
    }

    private void Update()
    {
        if ((_playerController.IsGrounded() || _playerController.IsWalled(Vector2.left) || _playerController.IsWalled(Vector2.right)) && _playerController.CanCastBlade)
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began || _isStartDone == false)
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
        else
        {
            if (_isStartDone)
            {
                _lineRenderer.positionCount = 0;

                _isStartDone = false;

                TimeManager.Instance.SetTime(1);
            }
        }
    }

    void DragStart()
    {
        _lineRenderer.positionCount = 0;

        _startPoint = _camera.ScreenToWorldPoint(_touch.position);
        _startPoint.z = 0;
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _startPoint);

        TimeManager.Instance.SetTime();

        _isStartDone = true;
    }

    void Dragging()
    {
        Vector3 draggingPos = _camera.ScreenToWorldPoint(_touch.position);
        draggingPos.z = 0;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(1, draggingPos);
    }

    void DragRelease()
    {
        _lineRenderer.positionCount = 0;

        _endPoint = _camera.ScreenToWorldPoint(_touch.position);
        _endPoint.z = 0;

        Vector3 force = _startPoint - _endPoint;

        Vector3 clampedForce = Vector3.ClampMagnitude(force, _maxDrag) * _power;

        PlayerController.Instance.CastBlade(clampedForce);

        _isStartDone = false;

        TimeManager.Instance.SetTime(1);
    }
}
