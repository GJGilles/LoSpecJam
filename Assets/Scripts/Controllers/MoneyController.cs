using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public TMPro.TMP_Text text;

    private int money = 0;

    public int Get() { return money; }

    public void Set(int amount)
    {
        money = amount;
        text.text = money.ToString();
    }
}
