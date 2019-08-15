using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDestroyer
{
    float damage { get; set; }
    void Destroy();
}
