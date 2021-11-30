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
		public void OnChangeGraph()
		{
			var nodes = this.nodes;
			if (nodes == null || nodes.Count == 0) { return; }
			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				if (node == null) { continue; }
				Debug.Log("node name: " + node.name);
			}
		}
	}
}