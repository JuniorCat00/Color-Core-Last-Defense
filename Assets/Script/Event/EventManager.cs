using System.Collections;
using System.Collections.Generic;
using System;

public static class EventManager
{
    public static event Action<int> OnEnemyKilled;
    public static event Action<int> OnWaveReward;

    public static event Action<int> OnPlayerHpChanged;
    public static event Action OnPlayerDead;

    public static void EnemyKilled(int value)
    {
        OnEnemyKilled?.Invoke(value);
    }

    public static void WaveReward(int value)
    {
        OnWaveReward?.Invoke(value);
    }

    public static void PlayerHpChanged(int hp)
    {
        OnPlayerHpChanged?.Invoke(hp);
    }

    public static void PlayerDead()
    {
        OnPlayerDead?.Invoke();
    }
}


