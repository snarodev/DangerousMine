using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public GameObject toolShopCellPrefab;

    public Transform toolShopListParent;

    public Text toolNameText;

    public GameObject attributeSliderPrefab;
    public GameObject attributeTextPrefab;
    public Transform attributeParent;

    public Text buyButtonText;

    ShopCell currentlySelectedShopCell;
    int currentlySelectedToolId = 0;

    public class ShopPanelPanelOpenInfo : PanelOpenInfo
    {
        public int[] toolsToSell;

        public ShopPanelPanelOpenInfo(int[] toolsToSell)
        {
            this.toolsToSell = toolsToSell;
        }
    }

    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        ShopPanelPanelOpenInfo info = (ShopPanelPanelOpenInfo)panelOpenInfo;

        for (int i = 0; i < info.toolsToSell.Length; i++)
        {
            GameObject go = Instantiate(toolShopCellPrefab, toolShopListParent);

            go.GetComponent<ShopCell>().Init(ToolController.toolController.GetTool(info.toolsToSell[i]), this);

            if (currentlySelectedShopCell == null)
            {
                currentlySelectedShopCell = go.GetComponent<ShopCell>();
                currentlySelectedToolId = info.toolsToSell[i];
                ShopCellClicked(currentlySelectedShopCell, currentlySelectedToolId);
            }
        }


    }

    public void ShopCellClicked(ShopCell shopCell, int toolId)
    {
        currentlySelectedShopCell = shopCell;
        currentlySelectedToolId = toolId;

        foreach (Transform child in attributeParent.transform)
        {
            if (child.name != "ToolName")
                Destroy(child.gameObject);
        }

        Tool tool = ToolController.toolController.GetTool(toolId);




        if (PlayerInventory.playerInventory.goldAmount >= tool.price)
        {
            buyButtonText.text = "Buy";
        }
        else
        {
            buyButtonText.text = "Not enough gold";
        }

        toolNameText.text = tool.displayName;


        CreateTextAtribute("Price", tool.price + "$");
        CreateTextAtribute("Damage", tool.damage.ToString());
        CreateTextAtribute("Recharge Time", tool.rechargeTime + "s");
    }

    public void Buy()
    {
        Debug.Log(currentlySelectedToolId);
        PlayerInventory.playerInventory.goldAmount -= ToolController.toolController.GetTool(currentlySelectedToolId).price;
        PlayerInventory.playerInventory.inventoryTools.Add(new InventoryTool(currentlySelectedToolId, ToolController.toolController.GetTool(currentlySelectedToolId).maxDurability));
        UIController.ui.CloseCurrentPanel();
    }

    void CreateSliderAtribute(string attributeName, int attributeValue, int attributeMax)
    {
        GameObject durabilityAttribute = Instantiate(attributeSliderPrefab, attributeParent);
        durabilityAttribute.transform.Find("Slider/SliderMain").GetComponent<Image>().fillAmount = attributeValue / (float)attributeMax;
        durabilityAttribute.transform.Find("ToolNameText").GetComponent<Text>().text = attributeName;
    }

    void CreateTextAtribute(string attributeName, string value)
    {
        GameObject durabilityAttribute = Instantiate(attributeTextPrefab, attributeParent);
        durabilityAttribute.transform.Find("AttributeValue").GetComponent<Text>().text = value;
        durabilityAttribute.transform.Find("ToolNameText").GetComponent<Text>().text = attributeName;
    }
}
