using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GameplayFramework
{
	[CreateAssetMenu(fileName = "Tag Define Asset", menuName = "KAction/Create gameplay tag defines", order = 0)]
	public class GTagAssetDefine : NodeGraph
	{
		public void OnUpdateTagGraph()
		{
			var nodes = this.nodes;
			if (nodes == null || nodes.Count == 0) { return; }
			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				if (node == null) { continue; }
				UpdateParticularNode(ref node);
			}

			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				if (node == null) { continue; }
				UpdateChains(ref node);
			}
		}

		void UpdateParticularNode(ref Node node)
		{
			GameplayTag thisTag =null, parentTag = null;
			List<GameplayTag> childTags = new List<GameplayTag>();
			GetTags(node, ref thisTag, ref parentTag, ref childTags);
			if (thisTag == null) { throw new Exception("Invalid condition. BUG!"); }
			thisTag.ParentTag = parentTag;
			thisTag.ImmediateChilds = childTags;
		}

		void UpdateChains(ref Node node)
		{
			GameplayTag thisTag = null;
			GetTag(node, ref thisTag);
			if (thisTag == null) { return; }
			thisTag.ParentChain = new List<GameplayTag>();

			GameplayTag parentT = thisTag.ParentTag;
			while (parentT != null)
			{
				if (thisTag.ParentChain.Contains(parentT) == false)
				{
					thisTag.ParentChain.Add(parentT);
				}
				parentT = parentT.ParentTag;
			}

			List<GameplayTag> chChain = new List<GameplayTag>();
			AddToChildTagList(thisTag, ref chChain);
			thisTag.ChildChain = chChain;

			void AddToChildTagList(GameplayTag gTag, ref List<GameplayTag> childTags)
			{
				var immCh = gTag.ImmediateChilds;
				if (immCh != null && immCh.Count > 0)
				{
					foreach (var i in immCh)
					{
						if (i == null) { continue; }
						if(childTags.Contains(i) == false)
                        {
							childTags.Add(i);
							AddToChildTagList(i, ref childTags);
                        }
					}
				}
			}
		}

		void GetTag(Node node, ref GameplayTag thisTag)
		{
			thisTag = (GameplayTag)node;
		}

		void GetTags(Node node, 
			ref GameplayTag thisTag, ref GameplayTag parentTag, ref List<GameplayTag> childTags)
        {
			thisTag = (GameplayTag)node;
			parentTag = null;
			childTags = new List<GameplayTag>();
			if (thisTag == null) { return; }

			if (node.Inputs != null)
			{
				foreach (var port in node.Inputs)
				{
					if (port != null && port.node != null && port.IsConnected)
					{
						if (port.fieldName == "parentNode")
						{
							var count = port.ConnectionCount;
							foreach (var n in port.GetConnections())
							{
								if (n == null || n.node == null) { continue; }
								parentTag = (GameplayTag)n.node;
								break;
							}
							break;
						}
					}
				}
			}

			if (node.Outputs != null)
			{
				foreach (var port in node.Outputs)
				{
					if (port != null && port.node != null && port.IsConnected)
					{
						if (port.fieldName == "childNode")
						{
							var count = port.ConnectionCount;
							foreach (var n in port.GetConnections())
							{
								if (n == null || n.node == null) { continue; }
								var cTag = (GameplayTag)n.node;
								if (childTags.Contains(cTag) == false)
								{
									childTags.Add(cTag);
								}
							}
							break;
						}
					}
				}
			}
		}
	}
}