using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInvoker : MonoBehaviour
{
    public void UseSkill(ISkillCommand command)
    {
        command.Execute();
    }
}



