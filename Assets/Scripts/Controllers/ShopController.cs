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

    public List<ShopRowController> rows = new List<ShopRowController>();

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
            rows[selection].UpdateSelect(false);
            selection--;
            if (selection < 0) selection = rows.Count - 1;
            rows[selection].UpdateSelect(true);
        }
        else if (down)
        {
            rows[selection].UpdateSelect(false);
            selection++;
            if (selection > rows.Count) selection = 0;
            rows[selection].UpdateSelect(true);
        }
    }

    private void Upgrade()
    {
        if (rows[selection].IsMax() || money.Get() < rows[selection].GetCost())
        {
            return;
        }

        money.Set(money.Get() - rows[selection].GetCost());
        float result = rows[selection].Upgrade();
        switch (selection)
        {
            case 0:
                rod.maxLength = result;
                break;
            case 1:
                rod.elasticForce = result;
                break;
            case 2:
                rod.reelForce = result;
                break;
            case 3:
                rod.snapLength = result;
                break;
        }
    }

}
