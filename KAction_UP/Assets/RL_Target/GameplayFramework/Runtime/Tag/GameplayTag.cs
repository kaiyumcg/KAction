using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GameplayFramework
{
    public class GameplayTag : Node
    {
        [SerializeField, Multiline] string tagContent;
        public string TagContent { get { return tagContent; } }

        [SerializeField] [Input] TagInput parentNode = new TagInput();
        [SerializeField] [Output] TagOutput childNode = new TagOutput();

        [SerializeField] GameplayTag parentTag;
        [SerializeField] List<GameplayTag> immediateChilds;
        [SerializeField] List<GameplayTag> parentChain, childChain;

        internal GameplayTag ParentTag { get { return parentTag; } set { parentTag = value; } }
        internal List<GameplayTag> ImmediateChilds { get { return immediateChilds; } set { immediateChilds = value; } }
        internal List<GameplayTag> ParentChain { get { return parentChain; } set { parentChain = value; } }
        internal List<GameplayTag> ChildChain { get { return childChain; } set { childChain = value; } }

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            parentTag = null;
            immediateChilds = null;
            parentChain = null;
            childChain = null;

            InitTagList(ref immediateChilds);
            InitTagList(ref parentChain);
            InitTagList(ref childChain);
            void InitTagList(ref List<GameplayTag> tags)
            {
                if (tags == null) { tags = new List<GameplayTag>(); }
                tags.RemoveAll((t) => { return t == null; });
                if (tags == null) { tags = new List<GameplayTag>(); }
            }

            var tg = (GTagAssetDefine)this.graph;
            if (tg != null)
            {
                tg.OnUpdateTagGraph();
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            var gTag_to = (GameplayTag)to.node;
            var gTag_from = (GameplayTag)from.node;
            if (gTag_from == null || gTag_to == null) { return; }
            if (from.IsOutput && to.IsInput && gTag_from == this)
            {
                if (gTag_to != null)
                {
                    if (gTag_to.parentTag != null || !PassedCyclicTest(gTag_from, gTag_to))
                    {
                        if (from.IsConnectedTo(to))
                        {
                            from.Disconnect(to);
                        }
                    }
                }

                var tg = (GTagAssetDefine)this.graph;
                if (tg != null)
                {
                    tg.OnUpdateTagGraph();
                }
            }
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            var tg = (GTagAssetDefine)this.graph;
            if (tg != null)
            {
                tg.OnUpdateTagGraph();
            }
        }

        #region HelperMethods
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