using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : Panel
{
    public override void Open(PanelOpenInfo panelOpenInfo)
    {
        
    }

    public void StartGame()
    {
        UIController.ui.CloseCurrentPanel();
    }
}
