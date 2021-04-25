using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds all tools
public class ToolController : Controller
{
    public List<Tool> tools = new List<Tool>();

    public static ToolController toolController;

    public override void Init()
    {
        toolController = this;
    }

    public override void Tick()
    {
        
    }

    public Tool GetTool(int id)
    {
        for (int i = 0; i < tools.Count; i++)
        {
            if (tools[i].id == id)
                return tools[i];
        }

        return null;
    }
}
