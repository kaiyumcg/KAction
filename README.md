# KAction
Gameplay framework inspired by UE4's actor/gameplay components for building action games using Unity Engine. The project is in alpha stage and not to be used in production yet. Unity version 2020.3.3f1 or up. 

Current documentation: https://docs.google.com/document/d/1vWu8L6ZGR5pWB9yHHOiJob_mIBthxz6s-_bF1yar2fI/edit?usp=sharing
A sample project that uses this framework: https://github.com/kaiyumcg/asteroid-game


#### Installation:
* Add the entry in your manifest.json as follows:
```C#
	"com.kaiyum.kaction": "https://github.com/kaiyumcg/KAction.git"
```

Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git",
"com.kaiyum.pathcreator": "https://github.com/kaiyumcg/Path-Creator.git",
"com.kaiyum.editorutil": "https://github.com/kaiyumcg/EditorUtil.git",
"com.github.siccity.xnode": "https://github.com/siccity/xNode.git",
"com.unity.playablegraph-visualizer": "https://github.com/kaiyumcg/graph-visualizer.git",
"com.kaiyum.adrenaline": "https://github.com/kaiyumcg/Adrenaline.git",
"com.kaiyum.kdata": "https://github.com/kaiyumcg/KData.git",
"com.kaiyum.kpoolmanager": "https://github.com/kaiyumcg/KPoolManager.git",
"com.kaiyum.ksavedatamanager": "https://github.com/kaiyumcg/KSaveDataManager.git",
"com.kaiyum.ktagsystem": "https://github.com/kaiyumcg/KTagSystem.git",
"com.kaiyum.ktaskgraph": "https://github.com/kaiyumcg/KTaskGraph.git",
"com.kaiyum.ktaskmanager": "https://github.com/kaiyumcg/KTaskManager.git",
"com.kaiyum.ktween": "https://github.com/kaiyumcg/KTween.git",
"com.kaiyum.neuron": "https://github.com/kaiyumcg/Neuron.git",
"com.kaiyum.uif": "https://github.com/kaiyumcg/UIF.git",
"com.kaiyum.uitween": "https://github.com/kaiyumcg/UITween.git",
"com.kaiyum.vortex": "https://github.com/kaiyumcg/Vortex.git#dev_rumman"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.