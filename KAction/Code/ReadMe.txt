Game consists of 'boot level' and 'game level'

Boot level:
A boot level is an unity scene named 'boot'.
On boot level, there are 'GameService'. They are managed by 'GameServiceManager'.
A game service called 'LevelManager' in boot level automatically loads the next proper level 'configured'.
Game service's lifetime starts from app start to exit. They can be accessed from anywhere through GameServiceManager.

Game level:
A game level is an unity scene named whatever you may wish and added to level list of 'LevelManager' Service in boot scene.
Game level technically is one polymorphic instance of 'GameLevel' class.
Game level manages one or multiple 'LevelModule'(can be polymorphic as GameLevel itself).
At least one of them is 'ActorLevelModule', this manages the actors in a level.
ActorLevelModule can not be polymorphic, it is sealed. 

Actors are conceptually things placed in a level. Actors can have two kinds of components attached to it.
1. Gameplay components. Statically attached in the editor and can not be added or removed in runtime.
2. Actor Components. Can be added or removed in runtime.

Actors can have child actors. Child-Parent relationship can be changed in runtime.
Actor manages execution of child actor and components.
ActorLevelModule in turns manages execution of root actors(one which is not child of any actor) in a level.

So execution of framework classes are like two parallel streams.
1. Game Service Stream 
		Game Service->Game Service Manager

2. Actor Stream
		Child Actor----------------------------------->
		GameplayComponent------------------------------->Actor........Root Actor----->ActorLevelModule---->
		DynamicGameplayComponent or ActorComponent---->								  SomeOtherModule1------->GameLevel
																					  SomeOtherModule2---->

There are certain builtin actors that have interesting relationship.
The inheritance chains of them are:
	   Actor--->ReactorActor--->CharacterActor--->RPG Character Actor
												  Adventure Character Actor
												  FPS Character Actor
												  Hack n slash Character Actor

Reactor actors can react to another reactor actor having physical shapes(volume or hard physics objects).
Consequently the related events are raised(OnHit, OnEnterVolume etc)

CharacterActor can be Player or NPC depending upon which Controller they are possessing.
Controller is a C# class derived from a common interface.


Editor tool support is necessary 
		---to easily create boot scene with addressable support.
		---to create game level.
		---to create overridden game level script and attach it and delete old one, all at one click.
		---to create actor script.
		---to create gameplay component script.
		---to bake data into 'GameLevel', 'ActorLevelModule' and 'Actor' upon Save operation, Play mode enter and Build process
		---to smart inspector operation(custom editor)
		---to set necessary conditional compilation flags through project setting window
		---to ensure there is no invalid data or list anywhere in gamelevel or boot level


A game creator's guide:
1. Prepare unity project by installing KAction and necessary prerequisites
2. Prepare boot level
3. Prepare one or many game level and assign them in 'LevelManager'
4. Think about the game that what are things or entity or stuff which can be placed.
5. These are actors and create proper actor scripts and develop them. 
	For character actor, develop PlayerController or AIController class and assign them in inspector.
6. Create necessary plug and play components that work well with the actors.
7. Actually create the actor prefabs or gameobjects with actor and gameplay component scripts.
8. Place them in the game level.
9. Test game with debug mode.
10. Before doing release build, make sure to turn off debug mode.

For now the assumption is that, the game has no UI! Which is mostly not the case.
When the UIF framework is ready, this doc will be updated.