using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _sprite;

    const string PLAYER_IDLE = "Idle";
    const string PLAYER_RUN = "Run";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_FALL = "Fall";
    const string PLAYER_DOUBLEJUMP = "DoubleJump";
    const string PLAYER_WALL = "Wall";
    const string PLAYER_HIT = "Hit";


    private void Update()
    {
        if (_playerController.IsWalled(Vector2.left) == false && _playerController.IsWalled(Vector2.right) == false)
        {
            if (_playerController.RB.velocity.y < -0.01f) PlayerFall();
            else if (_playerController.RB.velocity.y > 0.01f) PlayerJump();
        }

        if (_playerController.IsGrounded()) PlayerIdle();
        else if (_playerController.IsWalled(Vector2.left))
        {
            _sprite.flipX = true;
            PlayerWall();
        }
        else if (_playerController.IsWalled(Vector2.right))
        {
            _sprite.flipX = false;
            PlayerWall();
        }
    }

    void PlayerIdle() => ChangeAnimation(PLAYER_IDLE);
    void PlayerFall() => ChangeAnimation(PLAYER_FALL);
    void PlayerJump() => ChangeAnimation(PLAYER_JUMP);
    void PlayerWall() => ChangeAnimation(PLAYER_WALL);

    void ChangeAnimation(string animationName) => _animator.Play(animationName);

}
