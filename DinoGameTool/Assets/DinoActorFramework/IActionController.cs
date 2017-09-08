using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制器不要继承Mono
/// </summary>
public interface IActionController {

    Creature Target { get; set; }

    bool Enable { get; set; }

    void OnTakeAuthority();
    void OnLoseAuthority();

    void UpdateAction();
    void FixedUpdateAction();
    void LatedUpdateAction();
}
