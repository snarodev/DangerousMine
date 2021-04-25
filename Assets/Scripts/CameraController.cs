using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float lerpAmount = 0.5f;
    
    void Update()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, lerpAmount),
            Mathf.Lerp(transform.position.y, player.position.y, lerpAmount),
            transform.position.z);
    }
}
