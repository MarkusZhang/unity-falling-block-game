using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    void Attack();
}

// interface for enemy that doesn't start attacking on start
public interface IControlledAttacker
{
    void StartAttack();
}