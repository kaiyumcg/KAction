using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public delegate void OnDoAnything();
    public delegate void OnDoAnything<T>(T fValue);
}