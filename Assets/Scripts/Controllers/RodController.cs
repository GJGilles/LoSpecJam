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
    public GameObject hook;

    public List<RodState> rods = new List<RodState>();
    public float cooldown = 1f;

    public float dragForce = 0.03f;
    public float reelForce = 0.1f;
    public float elasticForce = 0.1f;
    public float gravityForce = 0.05f;

    private int state = 1;
    private float time = 0f;
    private float length = 18f;

    private Vector2 velocity = new Vector2(0, 0);

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
            velocity += lineVect * reelForce * Time.deltaTime;
        }
        else if (hold && lineVect.magnitude >= length)
        {
            velocity -= elasticForce * (length - lineVect.magnitude) * lineVect.normalized * Time.deltaTime;
        }

        velocity += -dragForce * velocity * Time.deltaTime;

        hook.transform.position = (Vector2)hook.transform.position + velocity * Time.deltaTime;
        line.SetPosition(1, hook.transform.position - transform.position + new Vector3(2, 2));
        if ((!hold && lineVect.magnitude >= length) || (reel && lineVect.magnitude <= length))
        {
            length = lineVect.magnitude;
        }
    }

    private void ChangeState(int next)
    {
        line.SetPosition(0, rods[next].origin);
        rod.sprite = rods[next].spr;
        state = next;

        time = cooldown;
    }
}
