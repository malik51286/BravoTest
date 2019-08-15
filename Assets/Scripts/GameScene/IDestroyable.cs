using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDestroyable
{
    float health { get; set; }
    void Destroy();
    void SetDestroyAction(Action action);
    void SetExploding();
    void InflictDamage(float amount);
}
