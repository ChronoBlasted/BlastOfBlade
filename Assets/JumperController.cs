using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperController : MonoBehaviour
{
    [SerializeField] Animator _jumpAnim;
    public void DoJump()
    {
        _jumpAnim.Play("JumperJump");
    }
}
