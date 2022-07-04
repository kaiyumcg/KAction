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

        //Bird is similar to Dinosour? 
        //White peacock is similar to Green peacock?
        //Fish is similar to Blue fin tuna?
        //Blue fin tuna is similar to Fish?
        //Cow is similar to Wagyu?
        //Wagyu is similar to Sahiwal?
        //Wagyu is similar to Cow?

        public bool HasCommonAncestor(Actor actor)
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
        //similarly has child or has parent or immediate parent etc methods
        public bool HasMatchedTag(Actor actor)
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
            if (tag == null) { return; }
            if (tags == null) { tags = new List<GameplayTag>(); }
            if (tags.Contains(tag) == false) { tags.Add(tag); }
        }
    }
}