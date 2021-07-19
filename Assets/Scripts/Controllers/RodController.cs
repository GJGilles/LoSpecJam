using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RodState
{
    public Sprite spr;
    public Vector2 origin;
}

public class RodController : MonoBehaviour
{
    public MoneyController money;
    public ShopController shop;
    public SpriteRenderer rod;
    public LineRenderer line;
    public SpriteRenderer hook;

    public List<RodState> rods = new List<RodState>();
    public float coolTime = 0.3f;

    public Sprite hookEmpty;

    public float waterLevel = -30f;

    public float dragForce = 0.7f;
    public float reelForce = 30f;
    public float elasticForce = 15f;
    public float gravityForce = 9f;

    public float maxLength = 100f;
    public float snapLength = 2f;
    public float snapTime = 1f;
    public float bounciness = 0.2f;

    private int state = 1;
    private float coolRemain = 0f;
    private float length = 10.5f;

    private Vector2 velocity = new Vector2(0, 0);

    private FishController fish;
    private float fishMagnitude = 0f;
    private Vector2 fishDirection = new Vector2(0, 1);
    private float snapRemain = 0f;

    private void Start()
    {
        ChangeState(state);
    }

    private void Update()
    {
        if (shop.isActiveAndEnabled) return;

        if (InputManager.GetFireC())
        {
            shop.gameObject.SetActive(true);
            return;
        }

        bool left = InputManager.GetHorzAxis() == -1;
        bool right = InputManager.GetHorzAxis() == 1;
        bool reel = InputManager.GetFireA();
        bool hold = InputManager.GetFireB();

        coolRemain -= Time.deltaTime;
        if (left && coolRemain <= 0 && state > 0)
        {
            ChangeState(state - 1);
        }
        else if (right && coolRemain <= 0 && state < (rods.Count - 1))
        {
            ChangeState(state + 1);
        }

        velocity += new Vector2(0, -gravityForce) * Time.deltaTime;

        var lineVect = ((Vector2)(line.GetPosition(0) - line.GetPosition(1)));
        if (reel)
        {
            velocity += lineVect.normalized * reelForce * Time.deltaTime;
        }

        if (lineVect.magnitude >= length)
        {
            velocity -= elasticForce * (length - lineVect.magnitude) * lineVect.normalized * Time.deltaTime;
        }

        if (fish != null)
        {
            fishMagnitude -= fish.decay * Time.deltaTime;
            if (fishMagnitude <= 0)
            {
                fishMagnitude = fish.force;
                float angle = UnityEngine.Random.Range(0, Mathf.PI);
                fishDirection = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));
                if (angle <= Mathf.PI / 2f)
                {
                    hook.flipX = true;
                }
                else
                {
                    hook.flipX = false;
                }
            }
            velocity += fishMagnitude * fishDirection * Time.deltaTime;
        }

        velocity += -dragForce * velocity * Time.deltaTime;

        MoveHook(hold, reel);
    }

    public void Bounce(Vector2 dir)
    {
        velocity -= (1 + bounciness) * Vector2.Dot(velocity, dir) * dir;
        MoveHook(InputManager.GetFireB(), InputManager.GetFireA());
    }

    public bool CanHook()
    {
        return fish == null;
    }

    public void HookFish(FishController fc)
    {
        fish = fc;
        hook.sprite = fc.spr.sprite;
        fishMagnitude = 0;
        fishDirection = new Vector2(0, 1);
    }

    private void CatchFish()
    {
        money.Set(money.Get() + fish.cost);
        Destroy(fish.gameObject);
        hook.sprite = hookEmpty;
    }

    private void SnapLine()
    {
        Destroy(fish.gameObject);
        snapRemain = snapTime;
        hook.sprite = hookEmpty;
        hook.transform.position = new Vector2(0, 0);
        velocity = new Vector2(0, 0);
    } 

    private void MoveHook(bool hold, bool reel)
    {
        var lineVect = ((Vector2)(line.GetPosition(0) - line.GetPosition(1)));
        hook.transform.position = (Vector2)hook.transform.position + velocity * Time.deltaTime;
        line.SetPosition(1, hook.transform.position - transform.position + new Vector3(0, 2));
        if ((!hold && !reel && lineVect.magnitude >= length) || (reel && lineVect.magnitude <= length))
        {
            length = Mathf.Min(lineVect.magnitude, maxLength);
            SetLineColor(new Color32(141, 144, 46, 1));
        }
        else if (lineVect.magnitude - length > snapLength)
        {
            SetLineColor(new Color32(246, 63, 76, 1));
            snapRemain -= Time.deltaTime;
            if (snapRemain <= 0)
            {
                SnapLine();
            }
        }
        else if (lineVect.magnitude - length > snapLength * 0.6)
        {
            SetLineColor(new Color32(253, 187, 39, 1));
            snapRemain = snapTime;
        }
        else
        {
            SetLineColor(new Color32(141, 144, 46, 1));
            snapRemain = snapTime;
        }

        if (fish && line.GetPosition(1).y > waterLevel)
        {
            CatchFish();
        }
    }

    private void ChangeState(int next)
    {
        line.SetPosition(0, rods[next].origin);
        rod.sprite = rods[next].spr;
        state = next;

        coolRemain = coolTime;
    }

    private void SetLineColor(Color32 col)
    {
        Gradient grad = new Gradient();
        grad.mode = GradientMode.Fixed;
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(col, 0f), new GradientColorKey(col, 1f) }, 
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );
        line.colorGradient = grad;
    }
}
