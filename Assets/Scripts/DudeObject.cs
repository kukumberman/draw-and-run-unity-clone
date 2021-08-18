using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DudeObject : MonoBehaviour
{
    public class OnSpikeCollisionEventArgs
    {
        public DudeObject Sender { get; }

        public OnSpikeCollisionEventArgs(DudeObject sender)
        {
            Sender = sender;
        }
    }

    public event Action<OnSpikeCollisionEventArgs> OnSpikeCollision = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<SpikeGroup>(out var spike))
        {
            if (spike.IsAcive)
            {
                OnSpikeCollision?.Invoke(new OnSpikeCollisionEventArgs(this));
            }
        }
        else if (other.TryGetComponent<PickableItem>(out var item))
        {
            item.Pickup();
        }
    }
}
