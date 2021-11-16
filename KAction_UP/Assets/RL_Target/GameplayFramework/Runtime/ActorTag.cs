using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    [CreateAssetMenu(menuName = "KAction/Create actor tag", fileName = "Actor tag", order = 0)]
    public class ActorTag : ScriptableObject
    {
        [SerializeField] string tagContent;
        public string TagContent { get { return tagContent; } }
    }
}