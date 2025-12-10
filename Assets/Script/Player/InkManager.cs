using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    public static InkManager main;
    public int ink = 500;

    void Awake()
    {
        main = this;
    }

    void OnEnable()
    {
        EventManager.OnEnemyKilled += AddInk;
        EventManager.OnWaveReward += AddInk;
    }

    void OnDisable()
    {
        EventManager.OnEnemyKilled -= AddInk;
        EventManager.OnWaveReward += AddInk;
    }

    void AddInk(int value)
    {
        ink += value;
    }
}

