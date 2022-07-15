using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static partial class ActorAPI
    {
        static bool _HasAnyTaggedRelation(Actor actor, Actor otherActor, bool useRefOptimization)
        {
            return _HasAtleastOneTagOf(actor, otherActor, useRefOptimization) || 
                _IsTaggedAncestorOf(actor, otherActor, useRefOptimization) || 
                _IsTaggedDescendantOf(actor, otherActor, useRefOptimization);
        }

        static bool _IsTaggedDescendantOf(Actor actor, Actor otherActor, bool useRefOptimization)
        {
            bool isIt = false;
            var tags = actor.tags;
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

        static bool _IsTaggedAncestorOf(Actor actor, Actor otherActor, bool useRefOptimization)
        {
            bool isIt = false;
            var tags = actor.tags;
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

        static bool _HasCommonTaggedAncestor(Actor actor, Actor otherActor, bool useRefOptimization)
        {
            bool hasIt = false;
            var tags = actor.tags;
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

        static bool _HasAtleastOneTagOf(Actor actor, Actor otherActor, bool useRefOptimization)
        {
            bool found = false;
            var tags = actor.tags;
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

        static void _AddUniqueTag(Actor actor, GameplayTag tag, bool useRefOptimization)
        {
            if (tag == null) { return; }
            if (actor.tags == null) { actor.tags = new List<GameplayTag>(); }
            if ((useRefOptimization ? actor.tags.ContainsOPT(tag) : actor.tags.Contains(tag)) == false) { actor.tags.Add(tag); }
        }
    }
}