using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] bool isPlayer = false;
        [SerializeField] List<GameplayTag> tags = null;
        public bool IsPlayer { get { return isPlayer; } }

        public bool IsSimilarTo(Actor actor, bool useHierarchy = false)
        {
            bool found = false;
            if (tags.Count > 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    var t = tags[i];
                    if (actor.tags.Contains(t))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

        public void AddUniqueTag(GameplayTag tag)
        {
            if (tags == null) { tags = new List<GameplayTag>(); }
            if (tag == null) { return; }
            if (tags.Contains(tag) == false) { tags.Add(tag); }
        }
    }
}