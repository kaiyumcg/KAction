using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public delegate void OnDoAnything();
    public delegate void OnDoAnything<T>(T fValue);
    public delegate void OnDoAnything<T1, T2, T3>(T1 fValue1, T2 fValue2, T3 fValue3);
    public delegate void OnDoAnything<T1, T2>(T1 fValue1, T2 fValue2);
    public delegate void OnDoAnything2(string key, ActorData data);
}