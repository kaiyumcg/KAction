using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace GameplayFramework
{
	[CustomNodeGraphEditor(typeof(GTagAssetDefine))]
	public class GTagAssetDefineEditor : NodeGraphEditor
	{
		public override string GetNodeMenuName(System.Type type)
		{
			if (type.Namespace == "GameplayFramework" && type.Name == "GameplayTag")
			{
				return "Create tag";
			}
			else return null;
		}

        public override void OnCreate()
        {
            base.OnCreate();
			var tg = (GTagAssetDefine)target;
			this.window.titleContent = new GUIContent(tg.name + " Editor");
		}

        public override void OnOpen()
        {
            base.OnOpen();
			var tg = (GTagAssetDefine)target;
			this.window.titleContent = new GUIContent(tg.name + " Editor");
		}

        public override Node CreateNode(Type type, Vector2 position)
        {
			var tg = (GTagAssetDefine)target;
			tg.OnChangeGraph();
			return base.CreateNode(type, position);
        }

        public override void RemoveNode(Node node)
        {
			var tg = (GTagAssetDefine)target;
			tg.OnChangeGraph();
			base.RemoveNode(node);
        }
    }
}