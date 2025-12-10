using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public SkillInvoker invoker;

    public void OnClickSlow()
    {   
        var cmd = new SkillCommand(
            SkillType.SlowAll,
            slowMultiplier: 1.5f,
            slowDuration: 5f
        );

        invoker.UseSkill(cmd);
    }

    public void OnClickDamageAll()
    {
        var cmd = new SkillCommand(
            SkillType.DamageAll,
            damageAmount: 15
        );

        invoker.UseSkill(cmd);
    }
}

