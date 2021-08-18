using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableFinish : PickableItem
{
    public override void Pickup()
    {
        FindObjectOfType<DrawAndRun>()?.FinishGame(true);
    }
}
