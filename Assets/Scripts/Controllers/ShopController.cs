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
    public float selectTime = 0.4f;

    private float selectRemain = 0f;
    private bool selected = false;
    private int selection = 0;

    private void Start()
    {
        rows[selection].UpdateSelect(true);
    }

    private void Update()
    {
        bool up = InputManager.GetVertAxis() == 1;
        bool down = InputManager.GetVertAxis() == -1;

        selectRemain -= Time.deltaTime;

        if (InputManager.GetFireB())
        {
            gameObject.SetActive(false);
        }
        else if ((InputManager.GetFireA()) && !selected)
        {
            Upgrade();
            selected = true;
        }
        else
        {
            selected = false;
        }

        if (up && selectRemain < 0)
        {
            rows[selection].UpdateSelect(false);
            selection--;
            if (selection < 0) selection = rows.Count - 1;
            rows[selection].UpdateSelect(true);

            selectRemain = selectTime;
        }
        else if (down && selectRemain < 0)
        {
            rows[selection].UpdateSelect(false);
            selection++;
            if (selection >= rows.Count) selection = 0;
            rows[selection].UpdateSelect(true);

            selectRemain = selectTime;
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
