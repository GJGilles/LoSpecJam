using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRowController : MonoBehaviour
{
    public UnityEngine.UI.Image progress;
    public UnityEngine.UI.Image highlight;
    public TMPro.TMP_Text cost;

    public List<Sprite> grades = new List<Sprite>();
    public List<ShopTier> tiers = new List<ShopTier>();

    private int level = 0;

    private void Start()
    {
        SetCost();
    }

    public void UpdateSelect(bool selected)
    {
        highlight.gameObject.SetActive(selected);
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
        progress.sprite = grades[level];
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
            cost.text = tiers[level].cost;
        }
    }
}
