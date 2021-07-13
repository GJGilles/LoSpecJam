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
    public SpriteRenderer rod;
    public LineRenderer line;
    public SpriteRenderer hook;

    public List<RodState> rods = new List<RodState>();
    public float cooldown = 0.3f;

    public Sprite hookEmpty;
    public Sprite hookLeft;
    public Sprite hookRight;

    public float waterLevel = -30f;

    public float dragForce = 0.7f;
    public float reelForce = 30f;
    public float elasticForce = 15f;
    public float gravityForce = 9f;
    public float fishForce = 10f;
    public float fishRate = 2f;

    private int state = 1;
    private float time = 0f;
    private float length = 10.5f;

    private Vector2 velocity = new Vector2(0, 0);

    private bool fish = false;
    private float fishMagnitude = 0f;
    private Vector2 fishDirection = new Vector2(0, 1);

    private void Start()
    {
        ChangeState(state);
    }

    private void Update()
    {
        bool left = InputManager.GetHorzAxis() == -1;
        bool right = InputManager.GetHorzAxis() == 1;
        bool reel = InputManager.GetFireA();
        bool hold = InputManager.GetFireB();

        time -= Time.deltaTime;
        if (left && time <= 0 && state > 0)
        {
            ChangeState(state - 1);
        }
        else if (right && time <= 0 && state < (rods.Count - 1))
        {
            ChangeState(state + 1);
        }

        velocity += new Vector2(0, -gravityForce) * Time.deltaTime;

        var lineVect = ((Vector2)(line.GetPosition(0) - line.GetPosition(1)));
        if (reel)
        {
            velocity += lineVect.normalized * reelForce * Time.deltaTime;
        }
        else if (hold && lineVect.magnitude >= length)
        {
            velocity -= elasticForce * (length - lineVect.magnitude) * lineVect.normalized * Time.deltaTime;
        }

        if (fish)
        {
            fishMagnitude -= fishRate * Time.deltaTime;
            if (fishMagnitude <= 0)
            {
                fishMagnitude = fishForce;
                float angle = UnityEngine.Random.Range(0, Mathf.PI);
                fishDirection = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));
                if (angle <= Mathf.PI / 2f)
                {
                    hook.sprite = hookRight;
                }
                else
                {
                    hook.sprite = hookLeft;
                }
            }
            velocity += fishMagnitude * fishDirection * Time.deltaTime;
        }

        velocity += -dragForce * velocity * Time.deltaTime;

        hook.transform.position = (Vector2)hook.transform.position + velocity * Time.deltaTime;
        line.SetPosition(1, hook.transform.position - transform.position + new Vector3(0, 2));
        if ((!hold && lineVect.magnitude >= length) || (reel && lineVect.magnitude <= length))
        {
            length = lineVect.magnitude;
        }

        if (fish && line.GetPosition(1).y > waterLevel)
        {
            CatchFish();
        }
    }

    public bool CanHook()
    {
        return !fish;
    }

    public void HookFish()
    {
        fish = true;
        fishMagnitude = 0;
        fishDirection = new Vector2(0, 1);
    }

    private void CatchFish()
    {
        fish = false;
        hook.sprite = hookEmpty;
    }

    private void ChangeState(int next)
    {
        line.SetPosition(0, rods[next].origin);
        rod.sprite = rods[next].spr;
        state = next;

        time = cooldown;
    }
}
