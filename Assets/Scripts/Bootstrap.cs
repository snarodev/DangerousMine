using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    List<Controller> controllers = new List<Controller>();

    private void Start()
    {
        GameObject.Find("WorldController").GetComponent<WorldController>().Init();
        controllers.Add(GameObject.Find("WorldController").GetComponent<WorldController>());


        ChunkRenderer chunkRenderer = CreateController<ChunkRenderer>();
        chunkRenderer.Init();

        GameObject.Find("UIController").GetComponent<UIController>().Init();
        controllers.Add(GameObject.Find("UIController").GetComponent<UIController>());


        GameObject.Find("Player").GetComponent<PlayerController>().Init();
        controllers.Add(GameObject.Find("Player").GetComponent<PlayerController>());


        GameObject.Find("WeaponController").GetComponent<ToolController>().Init();
        controllers.Add(GameObject.Find("WeaponController").GetComponent<ToolController>());

        UIController.ui.OpenPanel<MenuPanel>();
    }

    T CreateController<T>() where T : Controller
    {
        GameObject go = new GameObject("Controller", typeof(T));

        controllers.Add(go.GetComponent<Controller>());

        return go.GetComponent<T>();
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Tick();
        }
    }
}
