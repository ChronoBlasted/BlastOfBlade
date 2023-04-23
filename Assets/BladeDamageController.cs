using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDamageController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            CameraManager.Instance.ShakeCamera();

            TimeManager.Instance.DoLagTime(0f, .05f);

            collision.GetComponent<IHealth>().TakeDamage(1);
        }
    }
}
