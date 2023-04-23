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
            if (_playerController.HaveDoubleJump == false) PlayerDoubleJump();
            else if (_playerController.RB.velocity.y < 0) PlayerFall();
            else if (_playerController.RB.velocity.y > 0) PlayerJump();
        }


        if (_playerController.MoveValue.x != 0 && _playerController.IsGrounded()) PlayerRun();
        else if (_playerController.IsGrounded()) PlayerIdle();
        else if (_playerController.IsWalled(Vector2.left) || _playerController.IsWalled(Vector2.right)) PlayerWall();

        FlipSprite();
    }

    void FlipSprite()
    {
        if (_playerController.MoveValue.x < 0) _sprite.flipX = true;
        else if (_playerController.MoveValue.x > 0) _sprite.flipX = false;
    }

    void PlayerIdle() => ChangeAnimation(PLAYER_IDLE);
    void PlayerDoubleJump() => ChangeAnimation(PLAYER_DOUBLEJUMP);
    void PlayerFall() => ChangeAnimation(PLAYER_FALL);
    void PlayerJump() => ChangeAnimation(PLAYER_JUMP);
    void PlayerRun() => ChangeAnimation(PLAYER_RUN);
    void PlayerWall() => ChangeAnimation(PLAYER_WALL);
    public void PlayerHit() => ChangeAnimation(PLAYER_HIT);

    void ChangeAnimation(string animationName) => _animator.Play(animationName);

}
