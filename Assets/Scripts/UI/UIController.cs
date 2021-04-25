using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Controller
{
    public Panel[] panels;

    public Transform panelParent;

    public Image uiBlocker;

    Stack<Panel> openPanels = new Stack<Panel>();

    public static UIController ui;

    public override void Init()
    {
        ui = this;
    }

    public override void Tick()
    {
        
    }

    public void OpenPanel<T>(PanelOpenInfo panelOpenInfo) where T : Panel
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].GetType() == typeof(T))
            {
                uiBlocker.gameObject.SetActive(true);

                GameObject go = Instantiate(panels[i].gameObject, panelParent);

                go.GetComponent<Panel>().Open(panelOpenInfo);

                openPanels.Push(go.GetComponent<Panel>());

                uiBlocker.transform.SetAsLastSibling();
                go.transform.SetAsLastSibling();


                return;
            }
        }

        Debug.LogError("No Panel found");
    }
     
    public void OpenPanel<T>() where T : Panel
    {
        OpenPanel<T>(null);
    }

    public void CloseCurrentPanel()
    {
        if (openPanels.Count == 0)
            return;

        Panel panel = openPanels.Pop();

        if (openPanels.Count == 0)
            uiBlocker.gameObject.SetActive(false);
        else
        {
            uiBlocker.transform.SetAsLastSibling();
            openPanels.Peek().transform.SetAsLastSibling();
        }

        panel.Close();
    }

    public Panel GetTopPanel()
    {
        if (openPanels.Count == 0)
            return null;

        return openPanels.Peek();
    }
 

    public T GetTopPanel<T>() where T : Panel
    {
        if (openPanels.Count == 0)
            return null;

        return openPanels.Peek() as T;
    }

    public int GetOpenPanelAmount()
    {
        return openPanels.Count;
    }
}
