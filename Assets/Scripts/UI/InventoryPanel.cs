using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : Panel
{
    public GameObject toolCellPrefab;

    public Transform toolGridParent;

    public Image toolVisualPreview;

    public Text toolNameText;

    public GameObject attributeSliderPrefab;
    public GameObject attributeTextPrefab;
    public Transform attributeParent;

    [HideInInspector]
    public GameObject selectedCell;

    ToolCell oldToolCell;

    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        foreach (Transform child in toolGridParent.transform)
        {
            Destroy(child.gameObject);
        }


        

        for (int i = 0; i < playerInventory.inventoryTools.Count; i++)
        {
            InventoryTool inventoryTool = playerInventory.inventoryTools[i];

            if (inventoryTool.durability > 0)
            {
                GameObject go = Instantiate(toolCellPrefab, toolGridParent);

                go.GetComponent<ToolCell>().Init(playerInventory.inventoryTools[i],i, this);
            }
        }


        



        selectedCell = toolGridParent.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(toolGridParent.GetChild(0).gameObject);
    }

    private void Update()
    {
        if (UIController.ui.GetTopPanel() != this)
            return;

        GameObject go = EventSystem.current.currentSelectedGameObject;

        if (go != null && go != selectedCell)
        {
            if (go.GetComponent<ToolCell>() != null)
                selectedCell = go;
        }
        else if (go == null)
        {
            EventSystem.current.SetSelectedGameObject(selectedCell);
        }

        ToolCell cell = selectedCell.GetComponent<ToolCell>();



        if (cell != null)
        {
            if (oldToolCell != cell)
            {
                InventoryTool inventoryTool = cell.tool;

                Tool tool = ToolController.toolController.GetTool(inventoryTool.id);

                toolNameText.text = tool.displayName;
                toolVisualPreview.sprite = tool.sprite;

                oldToolCell = cell;



                foreach (Transform child in attributeParent.transform)
                {
                    if (child.name != "ToolName")
                        Destroy(child.gameObject);
                }

                if (tool.maxDurability == 0)
                    CreateTextAtribute("Durability", "Unlimited");
                else
                    CreateSliderAtribute("Durability", inventoryTool.durability, tool.maxDurability);

                CreateTextAtribute("Damage", tool.damage.ToString());
                CreateTextAtribute("Recharge Time", tool.rechargeTime + "s");
            }
        }
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


    public void ToolCellClicked(ToolCell cell)
    {
        selectedCell = cell.gameObject;
    }

    public void EquipTool()
    {
        UIController.ui.OpenPanel<ToolWheelSelection>(new ToolWheelSelection.ToolWheelSelectionPanelOpenInfo(selectedCell.GetComponent<ToolCell>().inventoryId));
    }
}
