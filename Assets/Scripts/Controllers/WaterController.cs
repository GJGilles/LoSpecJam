using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FishData
{
    public GameObject obj;
    public int depth = 0;
}

public class WaterController : MonoBehaviour
{
    public List<int> levels = new List<int>() { 0, 3, 2, 1, 1 };
    public float refresh = 60f;
    public List<FishData> data = new List<FishData>();

    private List<List<FishController>> fish = new List<List<FishController>>();
    private float time = 0f;

    private int height = 100;
    private int width = 240;

    private void Start()
    {
        for (int i = 1; i < levels.Count; i++)
        {
            fish.Add(new List<FishController>());
            for (int j = 0; j < levels[i]; j++)
            {
                fish[i - 1].Add(CreateFish(i));
            }
        }
        time = refresh;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = refresh;
            for (int i = 1; i < levels.Count; i++)
            {
                for (int j = 0; j < levels[i]; j++)
                {
                    if (fish[i - 1][j] == null)
                    {
                        fish[i - 1][j] = CreateFish(i);
                    }
                }
            }
        }
    }

    private FishController CreateFish(int i)
    {
        float y = Mathf.Round(-height * i / (float)levels.Count + height / 2f + transform.position.y + UnityEngine.Random.Range(20-height / (float)levels.Count, -20+height / (float)levels.Count)) + 0.5f;
        GameObject fish = RandomFish(i - 1);
        int range = fish.GetComponent<FishController>().range;
        return Instantiate(fish, new Vector2(UnityEngine.Random.Range(range, width - range) - width / 2f, y), new Quaternion(), transform).GetComponent<FishController>();
    }

    private GameObject RandomFish(int level)
    {
        int i = 0;
        do
        {
            i = UnityEngine.Random.Range(0, data.Count);
        } while (data[i].depth > level);
        return data[i].obj;
    }
}
