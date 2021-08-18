using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wallet : MonoBehaviour
{
    public event Action OnCoinAmountChanged = null;

    private int m_CoinAmount = 0;

    public int CoinAmount
    {
        get => m_CoinAmount;
        set
        {
            m_CoinAmount = value;
            OnCoinAmountChanged?.Invoke();
        }
    }

    private void Start()
    {
        CoinAmount = 0;
    }
}
