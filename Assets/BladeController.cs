using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 25)
        {
            if (collision.gameObject.tag == "Gum")
            {
                Vector2 tempVector = _rb.velocity;
                _rb.velocity = Vector2.zero;

                _rb.AddForce(tempVector, ForceMode2D.Impulse);
            }
            else if (collision.gameObject.tag == "Ice")
            {
                _rb.gravityScale = 0;
            }
            else
            {
                PlayerController.Instance.TeleportToBlade();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 25)
        {
            if (collision.gameObject.tag == "Ice")
            {
                _rb.gravityScale = 1;
            }
        }
    }
}
