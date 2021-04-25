using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header ("Mining")]
    public float miningRadius = 2.5f;
    public Color mouseOutsideRadiusColor;
    public Color mouseInsideRadiusColor;
    public Image toolCooldownImage;
    public GameObject tileBreakEffect;

    public Text DepthText;

    [Header("Bow")]
    public GameObject arrowPrefab;

    [Header("Laser")]
    public GameObject laserLinePrefab;
    public LayerMask laserLayerMask;

    [Header("Bomb")]
    public GameObject bombPrefab;

    [Header("Gold")]
    public Text goldText;
    public GameObject droppedGoldPrefab;

    [Header("Tool Management")]
    public Image currentToolImage;

    public Image toolDurabilitySlider;

    public GameObject checkPointReachedEffect;

    [HideInInspector]
    public List<InventoryTool> inventoryTools = new List<InventoryTool>();

    [HideInInspector]
    public int currentWheelSlot = 0;

    //[HideInInspector]
    public int[] toolWheelIds;

    public static PlayerInventory playerInventory;

    SpriteRenderer miningTilePreview;
    SpriteRenderer miningTileRadius;

    float toolLastUseTime;

    Camera cam;

    [HideInInspector]
    public int goldAmount = 0;

    [HideInInspector]
    public List<Vector2> checkPoints = new List<Vector2>();

    private void Start()
    {
        inventoryTools.Add(new InventoryTool(0, 10));

        //goldAmount = 100;

        playerInventory = this;

        toolWheelIds = new int[4] { 0, -1, -1, -1 };


        cam = Camera.main;

        miningTilePreview = transform.Find("MiningTilePreview").GetComponent<SpriteRenderer>();
        miningTileRadius = transform.Find("MiningTileRadius").GetComponent<SpriteRenderer>();

        miningTileRadius.transform.localScale = new Vector3(miningRadius * 2, miningRadius * 2, 1);
    }

    private void Update()
    {
        if (ToolController.toolController == null)
            return;

        goldText.text = goldAmount.ToString();
        DepthText.text = "Depth: " + (int)transform.position.y + " Meter";

        if (UIController.ui.GetOpenPanelAmount() == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                UIController.ui.OpenPanel<ToolWheel>();
            }
        }
        


        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryPanel inventoryPanel = UIController.ui.GetTopPanel<InventoryPanel>();

            if (inventoryPanel == null)
            {
                if (UIController.ui.GetOpenPanelAmount() == 0)
                    UIController.ui.OpenPanel<InventoryPanel>();
            }
            else
            {
                UIController.ui.CloseCurrentPanel();
            }
        }

        if (toolWheelIds[currentWheelSlot] == -1)
        {
            currentToolImage.sprite = null;
            return;
        }

        Tool currentTool = ToolController.toolController.GetTool(inventoryTools[toolWheelIds[currentWheelSlot]].id);

        currentToolImage.sprite = currentTool.sprite;


        if (UIController.ui.GetOpenPanelAmount() == 0)
        {
            if (currentTool.id == 0)    // Pickaxe
                Mining();
            else
            {
                miningTileRadius.enabled = false;
                miningTilePreview.enabled = false;
                toolCooldownImage.fillAmount = 0;
            }



            if (currentTool.id == 1)    // Bow
            {
                Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

                toolCooldownImage.fillAmount = 0;

                toolCooldownImage.transform.position = new Vector3(worldPos.x + 1f, worldPos.y - 1f, 0);

                if (toolLastUseTime + currentTool.rechargeTime > Time.time)
                {
                    toolCooldownImage.fillAmount = (toolLastUseTime + currentTool.rechargeTime - Time.time) / currentTool.rechargeTime;
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        GameObject go = Instantiate(arrowPrefab, transform.position + new Vector3(0.5f, 0.5f), Quaternion.identity);
                        go.GetComponent<Arrow>().damage = currentTool.damage;


                        Vector2 direction = worldPos - transform.position;

                        direction.Normalize();

                        go.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);

                        toolCooldownImage.fillAmount = 0;
                        toolLastUseTime = Time.time;

                        InventoryTool tool = inventoryTools[toolWheelIds[currentWheelSlot]];
                        tool.durability--;
                        inventoryTools[toolWheelIds[currentWheelSlot]] = tool;
                    }
                }
            }

            if (currentTool.id == 2)    // Laser
            {
                Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

                toolCooldownImage.fillAmount = 0;

                toolCooldownImage.transform.position = new Vector3(worldPos.x + 1f, worldPos.y - 1f, 0);

                if (toolLastUseTime + currentTool.rechargeTime > Time.time)
                {
                    toolCooldownImage.fillAmount = (toolLastUseTime + currentTool.rechargeTime - Time.time) / currentTool.rechargeTime;
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        GameObject go = Instantiate(laserLinePrefab, transform.position, Quaternion.identity);

                        Vector3 direction = worldPos - transform.position;
                        direction.z = 0;
                        direction.Normalize();
                        RaycastHit2D hit = Physics2D.Raycast (transform.position + new Vector3 (0.5f,0.5f), direction, 20, laserLayerMask);

                        Vector3 hitPosition;

                        if (hit.collider == null)
                            hitPosition = transform.position + new Vector3(0.5f, 0.5f) + direction * 20;
                        else
                            hitPosition = hit.point;




                        go.GetComponent<LineRenderer>().SetPositions(new Vector3[2] { transform.position, hitPosition });

                        Destroy(go, 0.5f);

                        if (hit.collider != null)
                        {
                            if (hit.collider.tag == "Enemy")
                            {
                                hit.collider.gameObject.GetComponent<Monster>().TakeDamage(currentTool.damage);
                            }
                        }


                        toolCooldownImage.fillAmount = 0;
                        toolLastUseTime = Time.time;

                        InventoryTool tool = inventoryTools[toolWheelIds[currentWheelSlot]];
                        tool.durability--;
                        inventoryTools[toolWheelIds[currentWheelSlot]] = tool;
                    }
                }
            }


            if (currentTool.id == 3)    // Bomb
            {
                Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

                toolCooldownImage.fillAmount = 0;

                toolCooldownImage.transform.position = new Vector3(worldPos.x + 1f, worldPos.y - 1f, 0);

                if (toolLastUseTime + currentTool.rechargeTime > Time.time)
                {
                    toolCooldownImage.fillAmount = (toolLastUseTime + currentTool.rechargeTime - Time.time) / currentTool.rechargeTime;
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        
                        


                        Vector2 direction = worldPos - transform.position;
                        direction.Normalize();


                        GameObject go = Instantiate(bombPrefab, transform.position + new Vector3(0.5f, 0.5f) + new Vector3 (direction.x,direction.y), Quaternion.identity);
                        go.GetComponent<Bomb>().damage = currentTool.damage;
                        go.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);


                        toolCooldownImage.fillAmount = 0;
                        toolLastUseTime = Time.time;

                        InventoryTool tool = inventoryTools[toolWheelIds[currentWheelSlot]];
                        tool.durability--;
                        inventoryTools[toolWheelIds[currentWheelSlot]] = tool;
                    }
                }
            }
        }




        InventoryTool inventoryTool = inventoryTools[toolWheelIds[currentWheelSlot]];
        toolDurabilitySlider.fillAmount = inventoryTool.durability / (float)currentTool.maxDurability;
        if (inventoryTool.durability <= 0)
        {
            int inventoryToolId = toolWheelIds[currentWheelSlot];

            // Don't remove it will break everything
            //inventoryTools.RemoveAt(inventoryToolId);
            toolWheelIds[currentWheelSlot] = -1;

            InfoText.infoText.DisplayMessage(ToolController.toolController.GetTool (inventoryTool.id).displayName + " broke");
        }
    }
    void Mining()
    {
        miningTileRadius.enabled = true;
        miningTilePreview.enabled = true;

        Tool currentTool = ToolController.toolController.GetTool(inventoryTools[toolWheelIds[currentWheelSlot]].id);

        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int tilePos = Tile.MakeTilePos(new Vector2(worldPos.x, worldPos.y));

        miningTilePreview.transform.position = new Vector3(tilePos.x, tilePos.y, 0);


        float distance = Vector2.Distance(worldPos, transform.position + new Vector3(0.5f, 0.5f));

        toolCooldownImage.transform.position = new Vector3(worldPos.x + 1f, worldPos.y - 1f, 0);

        if (toolLastUseTime + currentTool.rechargeTime > Time.time)
        {
            toolCooldownImage.fillAmount = (toolLastUseTime + currentTool.rechargeTime - Time.time) / currentTool.rechargeTime;
        }
        else
        {
            toolCooldownImage.fillAmount = 0;

            if (distance > miningRadius)
            {
                miningTileRadius.color = mouseOutsideRadiusColor;
                miningTilePreview.color = Color.clear;
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    Chunk chunk = WorldController.controller.GetOrCreateChunk(tilePos);

                    Tile tile = chunk.GetTile(tilePos);


                    if (!tile.isAir && !tile.stone)
                    {
                        for (int i = 0; i < tile.goldAmount; i++)
                        {
                            GameObject go = Instantiate(droppedGoldPrefab, new Vector3(tilePos.x + Random.Range(0f, 1f), tilePos.y + Random.Range(0f, 1f), 0), Quaternion.identity);
                            go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(1f, 3f)));
                        }

                        GameObject effect = Instantiate (tileBreakEffect, new Vector3(tilePos.x + Random.Range(0f, 1f), tilePos.y + Random.Range(0f, 1f), 0), Quaternion.identity);
                        Destroy(effect, 3);


                        tile.isAir = true;
                        chunk.SetTile(tilePos, tile);

                        WorldController.controller.SetChunk(chunk);

                        ChunkRenderer.chunkRenderer.AddDirtyChunk(tilePos);

                        toolLastUseTime = Time.time;
                    }
                }

                miningTileRadius.color = mouseInsideRadiusColor;
                miningTilePreview.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
    public void GoldCollected(int amount)
    {
        goldAmount += amount;
    }

    public void AddCheckpoint(Vector2 checkpoint)
    {
        if (!checkPoints.Contains(checkpoint))
        {
            InfoText.infoText.DisplayMessage("Checkpoint reached | " + checkpoint.y + " Meters");

            checkPoints.Add(checkpoint);
        }
    }
}

public struct InventoryTool
{
    public int id;
    public int durability;

    public InventoryTool(int id, int durability)
    {
        this.id = id;
        this.durability = durability;
    }
}

