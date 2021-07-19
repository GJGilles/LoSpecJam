using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopTier
{
    public int cost;
    public float result;
}

public class ShopController : MonoBehaviour
{
    public MoneyController money;
    public RodController rod;

    public List<ShopTier> lengthTiers = new List<ShopTier>(); 
    public List<ShopTier> lineTiers = new List<ShopTier>();
    public List<ShopTier> reelTiers = new List<ShopTier>();
    public List<ShopTier> snapTiers = new List<ShopTier>();

    private int length = 0;
    private int line = 0;
    private int reel = 0;
    private int snap = 0;

    private bool selected = false;
    private int selection = 0;

    private void Update()
    {
        bool up = InputManager.GetVertAxis() == 1;
        bool down = InputManager.GetVertAxis() == -1;

        if (InputManager.GetFireB())
        {
            gameObject.SetActive(false);
        }
        else if ((InputManager.GetFireA() || InputManager.GetFireC()) && !selected)
        {
            Upgrade();
            selected = true;
        }
        else
        {
            selected = false;
        }

        if (up)
        {
            selection--;
            if (selection < 0) selection = 3;
        }
        else if (down)
        {
            selection++;
            if (selection > 3) selection = 0;
        }
    }

    private void Upgrade()
    {
        float result;
        switch (selection)
        {
            case 0:
                result = TryBuy(lengthTiers, ref length);
                if (result != 0)
                {
                    rod.maxLength = result;
                }
                break;
            case 1:
                result = TryBuy(lineTiers, ref line);
                if (result != 0)
                {
                    rod.elasticForce = result;
                }
                break;
            case 2:
                result = TryBuy(reelTiers, ref reel);
                if (result != 0)
                {
                    rod.reelForce = result;
                }
                break;
            case 3:
                result = TryBuy(snapTiers, ref snap);
                if (result != 0)
                {
                    rod.snapLength = result;
                }
                break;
        }
    }

    private float TryBuy(List<ShopTier> tiers, ref int idx)
    {
        float res = 0;
        if (money.money >= tiers[idx].cost)
        {
            res = tiers[idx].result;
            money.money -= tiers[idx].cost;
            idx++;
        }
        return res;
    }
}
