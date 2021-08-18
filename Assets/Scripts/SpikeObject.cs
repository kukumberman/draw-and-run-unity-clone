using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObject : MonoBehaviour
{
    public void UpdateState(bool active)
    {
        Vector3 size = Vector3.one;
        size.y = active ? 1 : 0.1f;
        transform.localScale = size;
    }
}
