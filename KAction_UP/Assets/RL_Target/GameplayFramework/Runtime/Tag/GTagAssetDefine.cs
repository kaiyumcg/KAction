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
			Debug.Log("will update tags. num: " + nodes.Count);

			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				if (node == null) { continue; }
				Debug.Log("<color='green'>We will now inspect node named: " + node.name + "</color>");

				if (node.Inputs != null)
				{
					foreach (var port in node.Inputs)
					{
						if (port != null && port.node != null && port.IsConnected)
						{
							if (port.fieldName == "parentNode")
							{
								var count = port.ConnectionCount;
								Debug.Log("<color='magenta'>Has " + count + " Inputs. </color>");
								foreach (var n in port.GetConnections())
								{
									if (n == null || n.node == null) { continue; }
									Debug.Log("" + n.node.name);
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
								Debug.Log("<color='cyan'>Has " + count + " Outputs. </color>");
								foreach (var n in port.GetConnections())
								{
									if (n == null || n.node == null) { continue; }
									Debug.Log("" + n.node.name);
								}
								break;
							}
						}
					}
				}
				Debug.Log("<color='red'>Inspection complete</color>");
			}
		}

		void UpdateNode(ref Node node)
		{
			var thisTag = (GameplayTag)node;
			if (thisTag == null) { return; }

			if (node.Inputs != null)
			{
				foreach (var inp in node.Inputs)
				{
					if (inp != null && inp.IsConnected) 
					{ 
						//nCount++; 
					}
				}
			}
		}
	}
}