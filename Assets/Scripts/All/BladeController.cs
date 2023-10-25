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

            }
            else if (collision.gameObject.tag == "Ice")
            {
                var slideForce = new Vector2(_rb.velocity.x / 2, 0);

                PlayerController.Instance.TeleportToBlade(slideForce);
            }
            else
            {
                PlayerController.Instance.TeleportToBlade(Vector3.zero);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 30)
        {
            if (collision.gameObject.tag == "Fan")
            {
                var fanController = collision.GetComponent<FanController>();

                switch (fanController.Direction)
                {
                    case Direction.NONE:
                        break;
                    case Direction.UP:
                        _rb.AddForce(Vector3.up / 2, ForceMode2D.Impulse);
                        break;
                    case Direction.RIGHT:
                        _rb.AddForce(Vector3.right / 2, ForceMode2D.Impulse);
                        break;
                    case Direction.DOWN:
                        _rb.AddForce(Vector3.down / 2, ForceMode2D.Impulse);
                        break;
                    case Direction.LEFT:
                        _rb.AddForce(Vector3.left / 2, ForceMode2D.Impulse);
                        break;
                }
            }
        }
    }
}
