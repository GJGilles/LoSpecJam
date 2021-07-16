using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public RodController rod;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        FishController fc;
        if (rod.CanHook() && collision.gameObject.TryGetComponent(out fc))
        {
            rod.HookFish();
            Destroy(fc.gameObject);
        }
        else
        {
            rod.Bounce(collision.GetContact(0).normal);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        rod.Bounce((new Vector2(0, 0) - (Vector2)transform.position).normalized);
    }
}
