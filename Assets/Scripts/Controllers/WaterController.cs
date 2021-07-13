using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public int levels = 10;
    public float refresh = 60f;
    public GameObject fishObject;

    private List<FishController> fish = new List<FishController>();
    private float time = 0f;

    private int height = 100;
    private int width = 240;

    private void Start()
    {
        for (int i = 1; i < levels; i++)
        {
            fish.Add(CreateFish(i));
        }
        time = refresh;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = refresh;
            for (int i = 1; i < levels; i++)
            {
                if (fish[i-1] == null)
                {
                    fish[i-1] = CreateFish(i);
                }
            }
        }
    }

    private FishController CreateFish(int i)
    {
        float y = height * i / (float)levels - height / 2f + transform.position.y;
        return Instantiate(fishObject, new Vector2(Random.Range(50, width - 50) - width / 2f, y), new Quaternion(), transform).GetComponent<FishController>();
    }
}
