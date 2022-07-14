using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        [SerializeField] bool isPlayer = false;
        [SerializeField] internal List<GameplayTag> tags = null;
        public bool IsPlayer { get { return isPlayer; } }

        public bool HasAnyTaggedRelation(Actor actor)
        {
            return HasAtleastOneTagOf(actor) || IsTaggedAncestorOf(actor) || IsTaggedDescendantOf(actor);
        }

        public bool IsTaggedDescendantOf(Actor actor)
        {
            bool isIt = false;
            
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (isIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = actor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (thisTag.ParentChain.Contains(oTag))
                                    {
                                        isIt = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isIt;
        }

        public bool IsTaggedAncestorOf(Actor actor)
        {
            bool isIt = false;
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (isIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = actor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (oTag.ParentChain.Contains(thisTag))
                                    {
                                        isIt = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isIt;
        }

        public bool HasCommonTaggedAncestor(Actor actor)
        {
            bool hasIt = false;
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (hasIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = actor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (thisTag.ParentChain.HasAtleastOneItemIn(oTag.ParentChain))
                                    {
                                        hasIt = true;
                                        break;
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            return hasIt;
        }

        public bool HasAtleastOneTagOf(Actor actor)
        {
            bool found = false;
            var acTags = actor.tags;
            if (acTags != null)
            {
                var tCount = acTags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        var t = acTags[i];
                        if (tags.Contains(t))
                        {
                            found = true;
                            break;
                        }
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