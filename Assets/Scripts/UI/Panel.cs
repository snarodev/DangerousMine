using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public abstract void Open(PanelOpenInfo panelOpenInfo);

    public virtual void Close()
    {
        Destroy(gameObject);
    }
}

