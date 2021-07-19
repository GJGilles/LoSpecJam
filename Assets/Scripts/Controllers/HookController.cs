using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public RodController rod;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FishController fc;
        if (collision.gameObject.TryGetComponent(out fc) && rod.CanHook())
        {
            rod.HookFish(fc);
            fc.gameObject.SetActive(false);
        }
        else if (fc == null)
        {
            rod.Bounce(collision.offset.normalized);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<FishController>(out _))
        {
            rod.Bounce((transform.parent.position - transform.position).normalized);
        }
    }
}
