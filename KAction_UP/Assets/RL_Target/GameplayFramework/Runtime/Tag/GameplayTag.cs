using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GameplayFramework
{
    public class GameplayTag : Node
    {
        [SerializeField] string tagContent;
        public string TagContent { get { return tagContent; } }

        [SerializeField] [Input] TagInput parentNode = new TagInput();
        [SerializeField] [Output] TagOutput childNodes = new TagOutput();

        [SerializeField, HideInInspector] GameplayTag parentTag;
        [SerializeField, HideInInspector] List<GameplayTag> immediateChilds;
        [SerializeField, HideInInspector] List<GameplayTag> parentChain, childChain;

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            InitTagList(ref immediateChilds);
            InitTagList(ref parentChain);
            InitTagList(ref childChain);

            parentTag = null;
            immediateChilds = null;
            parentChain = null;
            childChain = null;
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            var gTag_to = (GameplayTag)to.node;
            var gTag_from = (GameplayTag)from.node;
            if (gTag_from == null || gTag_to == null) { return; }

            //ProcessConCreate(from, to);

            

            void ProcessConCreate(NodePort from, NodePort to)
            {
                if (from.IsInput && to.IsOutput)
                {
                    if (gTag_to != null)
                    {
                        if (gTag_to.parentTag != null)
                        {
                            if (from.IsConnectedTo(to))
                            {
                                from.Disconnect(to);
                            }
                        }
                        else
                        {
                            if (PassedCyclicTest(gTag_from, gTag_to))
                            {
                                if (gTag_from.immediateChilds == null) { gTag_from.immediateChilds = new List<GameplayTag>(); }
                                if (gTag_from.immediateChilds.Contains(gTag_to) == false)
                                {
                                    gTag_from.immediateChilds.Add(gTag_to);
                                }
                                gTag_to.parentTag = gTag_from;
                                AddToChildChainUpward(gTag_from, gTag_to);
                                AddToParentChainDownward(gTag_to, gTag_from);
                            }
                            else
                            {
                                if (from.IsConnectedTo(to))
                                {
                                    from.Disconnect(to);
                                }
                            }
                        }
                    }
                }
            }

            var tg = (GTagAssetDefine)this.graph;
            if (tg != null)
            {
                tg.OnChangeGraph();
            }
        }
        
        //removed port is wagyu and port is wagyu grade A
        public override void OnRemoveConnection(NodePort to, NodePort from)
        {
            base.OnRemoveConnection(to, from);
            var fromTag = (GameplayTag)from.node;
            var toTag = (GameplayTag)to.node;
            if (fromTag == null || toTag == null) { return; }

            //if (from.IsOutput && to.IsInput)
            //{
            //    Debug.Log("removal process!");
            //    Debug.Log("removed from: " + from.node.name + " and to: " + to.node.name);
            //}

            var tg = (GTagAssetDefine)this.graph;
            if (tg != null)
            {
                tg.OnChangeGraph();
            }
        }

        #region HelperMethods
        void InitTagList(ref List<GameplayTag> tags)
        {
            if (tags == null) { tags = new List<GameplayTag>(); }
            tags.RemoveAll((t) => { return t == null; });
            if (tags == null) { tags = new List<GameplayTag>(); }
        }

        bool PassedCyclicTest(GameplayTag fromSocalledParent, GameplayTag toSocalledChild)
        {
            var valid = true;
            if (fromSocalledParent != null && fromSocalledParent.parentChain != null &&
                fromSocalledParent.parentChain.Count > 0 && toSocalledChild != null)
            {
                for (int i = 0; i < fromSocalledParent.parentChain.Count; i++)
                {
                    var fp = fromSocalledParent.parentChain[i];
                    if (fp == null) { continue; }
                    if (fp == toSocalledChild)
                    {
                        valid = false;
                        break;
                    }
                }
            }
            return valid;
        }

        void AddToChildChainUpward(GameplayTag tag, GameplayTag addedChildTag)
        {
            if (tag != null)
            {
                InitTagList(ref tag.childChain);
                if (tag.childChain.Contains(addedChildTag) == false)
                {
                    tag.childChain.Add(addedChildTag);
                }
                AddToChildChainUpward(tag.parentTag, addedChildTag);
            }
        }

        void AddToParentChainDownward(GameplayTag tag, GameplayTag addedParentTag)
        {
            if (tag != null)
            {
                InitTagList(ref tag.parentChain);
                if (tag.parentChain.Contains(addedParentTag) == false)
                {
                    tag.parentChain.Add(addedParentTag);
                }

                if (tag.immediateChilds != null && tag.immediateChilds.Count > 0)
                {
                    for (int i = 0; i < tag.immediateChilds.Count; i++)
                    {
                        var pTag = tag.immediateChilds[i];
                        if (pTag == null) { continue; }
                        AddToParentChainDownward(pTag, addedParentTag);
                    }
                }
            }
        }

        public bool IsSubtypeOf(GameplayTag gameplayTag)
        {
            var isSub = false;
            for (int i = 0; i < parentChain.Count; i++)
            {
                if (parentChain[i] == gameplayTag)
                {
                    isSub = true;
                    break;
                }
            }
            return isSub;
        }

        public bool IsSuperTypeOf(GameplayTag gameplayTag)
        {
            var isSuper = false;
            for (int i = 0; i < immediateChilds.Count; i++)
            {
                if (immediateChilds[i] == gameplayTag)
                {
                    isSuper = true;
                    break;
                }
            }
            return isSuper;
        }

        public bool IsSameOrSimilarTo(GameplayTag gameplayTag)
        {
            return this == gameplayTag || this.IsSubtypeOf(gameplayTag);
        }
        #endregion
    }

    [System.Serializable]
    public class TagInput { }

    [System.Serializable]
    public class TagOutput { }
}