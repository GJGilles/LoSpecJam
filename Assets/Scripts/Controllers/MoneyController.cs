using System;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public UnityEngine.UI.Image spr;

    public List<Sprite> levels = new List<Sprite>();

    private int money = 0;

    public int Get() { return money; }

    public void Set(int amount)
    {
        money = amount;
        spr.sprite = levels[Math.Min(Mathf.FloorToInt(levels.Count * money / 90f), levels.Count - 1)];

        text.text = money.ToString();
    }
}
