using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public SpriteRenderer spr;
    public float speed = 0.01f;
    public int range = 50;

    public int cost;
    public float force;
    public float decay;

    private float location = 0;
    private int destination = 0;

    void Update()
    {
        while(Mathf.RoundToInt(location) == destination)
        {
            destination = UnityEngine.Random.Range(-range, range);
        }

        float next = location + speed * Time.deltaTime * 400 * Math.Sign(destination - Mathf.RoundToInt(location));
        int diff = Mathf.RoundToInt(next) - Mathf.RoundToInt(location);
        while (diff > 0)
        {
            spr.flipX = true;
            transform.position = transform.position + new Vector3(1, 0);
            diff--;
        }
        while (diff < 0)
        {
            spr.flipX = false;
            transform.position = transform.position - new Vector3(1, 0);
            diff++;
        }

        location = next;
    }
}
