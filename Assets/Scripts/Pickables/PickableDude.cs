using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableDude : PickableItem
{
    public override void Pickup()
    {
        gameObject.SetActive(false);

        FindObjectOfType<DudeSpawner>().SpawnAt(transform.position);
    }
}
