using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    public Image toolImage;
    public Text toolName;
    public Text toolPrice;

    int toolId;
    ShopPanel shopPanel;

    public void Init(Tool tool, ShopPanel shopPanel)
    {
        toolId = tool.id;
        this.shopPanel = shopPanel;

        toolImage.sprite = tool.sprite;
        toolName.text = tool.displayName;
        toolPrice.text = tool.price + "$";
    }

    public void CellClicked()
    {
        shopPanel.ShopCellClicked(this, toolId);
    }
}
