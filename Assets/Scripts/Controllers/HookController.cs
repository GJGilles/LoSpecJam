using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public RodController rod;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FishController fc;
        if (rod.CanHook() && collision.gameObject.TryGetComponent(out fc))
        {
            rod.HookFish();
            Destroy(fc.gameObject);
        }
    }
}
