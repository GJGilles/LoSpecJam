using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRowController : MonoBehaviour
{
    public GameObject progress;
    public UnityEngine.UI.Image button;
    public TMPro.TMP_Text cost;

    public List<ShopTier> tiers = new List<ShopTier>();

    private int level = 0;

    private void Start()
    {
        SetCost();
    }

    public void UpdateSelect(bool selected)
    {
        if (selected)
        {
            button.color = new Color32();
        }
        else
        {
            button.color = new Color32();
        }
    }

    public bool IsMax()
    {
        return level >= tiers.Count;
    }

    public int GetCost()
    {
        return tiers[level].cost;
    }

    public float Upgrade()
    {
        progress.transform.GetChild(level).gameObject.SetActive(true);
        level++;
        SetCost();
        return tiers[level - 1].result;
    }

    private void SetCost()
    {
        if (IsMax())
        {
            cost.text = "-";
        }
        else
        {
            cost.text = "$" + tiers[level].cost;
        }
    }
}
