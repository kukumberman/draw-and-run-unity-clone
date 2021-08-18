using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableCoin : PickableItem
{
    [SerializeField] private int m_Amount = 1;

    public override void Pickup()
    {
        var wallet = FindObjectOfType<Wallet>();
        if (wallet)
        {
            wallet.CoinAmount += m_Amount;
        }

        gameObject.SetActive(false);
    }
}
