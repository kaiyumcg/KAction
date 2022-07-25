using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public bool HasAnyTaggedRelation(Actor otherActor, bool useRefOptimization)
        {
            return HasAtleastOneTagOf(otherActor, useRefOptimization) || 
                IsTaggedAncestorOf(otherActor, useRefOptimization) || 
                IsTaggedDescendantOf(otherActor, useRefOptimization);
        }

        public bool IsTaggedDescendantOf(Actor otherActor, bool useRefOptimization)
        {
            bool isIt = false;
            var tags = this.tags;
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (isIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = otherActor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (useRefOptimization ? thisTag.ParentChain.ContainsOPT(oTag) : thisTag.ParentChain.Contains(oTag))
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

        public bool IsTaggedAncestorOf(Actor otherActor, bool useRefOptimization)
        {
            bool isIt = false;
            var tags = this.tags;
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (isIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = otherActor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (useRefOptimization ? oTag.ParentChain.ContainsOPT(thisTag) : oTag.ParentChain.Contains(thisTag))
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

        public bool HasCommonTaggedAncestor(Actor otherActor, bool useRefOptimization)
        {
            bool hasIt = false;
            var tags = this.tags;
            if (tags != null)
            {
                var tCount = tags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        if (hasIt) { break; }
                        var thisTag = tags[i];

                        var otherTags = otherActor.tags;
                        if (otherTags != null)
                        {
                            var otCount = otherTags.Count;
                            if (otCount > 0)
                            {
                                for (int j = 0; j < otCount; j++)
                                {
                                    var oTag = otherTags[j];
                                    if (thisTag.ParentChain.HasAtleastOneItemIn(oTag.ParentChain, useRefOptimization))
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

        public bool HasAtleastOneTagOf(Actor otherActor, bool useRefOptimization)
        {
            bool found = false;
            var tags = this.tags;
            var acTags = otherActor.tags;
            if (acTags != null)
            {
                var tCount = acTags.Count;
                if (tCount > 0)
                {
                    for (int i = 0; i < tCount; i++)
                    {
                        var t = acTags[i];
                        if (useRefOptimization ? tags.ContainsOPT(t) : tags.Contains(t))
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }
            return found;
        }

        public void AddUniqueTag(GameplayTag tag, bool useRefOptimization)
        {
            if (tag == null) { return; }
            if (this.tags == null) { this.tags = new List<GameplayTag>(); }
            if ((useRefOptimization ? this.tags.ContainsOPT(tag) : this.tags.Contains(tag)) == false) { this.tags.Add(tag); }
        }
    }
}