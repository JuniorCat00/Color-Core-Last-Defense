using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : ISkillCommand
{
    private SkillType type;

    // ค่าพื้นฐานที่ใช้กับทุกสกิล
    private float slowMultiplier;
    private float slowDuration;
    private int damageAmount;
    private int healAmount;

    public SkillCommand(
        SkillType type,
        float slowMultiplier = 1.5f,
        float slowDuration = 5f,
        int damageAmount = 20,
        int healAmount = 50)
    {
        this.type = type;
        this.slowMultiplier = slowMultiplier;
        this.slowDuration = slowDuration;
        this.damageAmount = damageAmount;
        this.healAmount = healAmount;
    }

    public void Execute()
    {
        switch (type)
        {
            case SkillType.SlowAll:
                ExecuteSlowAll();
                break;

            case SkillType.DamageAll:
                ExecuteDamageAll();
                break;

            default:
                Debug.LogWarning("Skill not implemented.");
                break;
        }
    }

    private void ExecuteSlowAll()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
        {
            enemy.ApplySlow(slowMultiplier, slowDuration);
        }
    }

    private void ExecuteDamageAll()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(damageAmount);
        }
    }
}

