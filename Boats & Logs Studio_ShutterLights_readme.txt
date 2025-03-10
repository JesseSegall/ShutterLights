Names: Bradley Hammond, Jesse Segall, Patrick Humberto Sandoval, Tao Deneb Quan
Emails: bhammond33@gatech.edu, jsegall6@gatech.edu, ,
Canvas account name: bhammond33, jsegall6, ,
Start scene: StartScreen

How to play and what parts of the level to observe technology
requirements:

Player controls are wasd on mouse and keyboard. Shift allows the player to run. Space allows the player to jump.

1. Player starts in the MainScene, player must travel into open cell door.
2. Player will then transfer to Room_1.
3. Player will observe elevators with light orbs and green and red power ups. Collect orbs for effects.
White light orbs will replenish light meter.
4. Traverse elevators, if you fall into the lava or spikes you will respawn at the last checkpoint.
5. At the end of the first room you will encounter an ai enemy ghost. The ghosts will chase you if you get too close.
They will alert you will a sound when they aggro. If they collide with you the ghosts will die and steal your light.
6. At the end of this scene there is a doorway that will bring you back to the main room.
7. Have the player go up the stairs and into the big door for the final boss fight.
8. Once in the boss room, the player must collect light orbs to damage the boss while dodging projectiles summoned by the boss
9. Once the boss reaches 50% health the boss will start spinning and moving towards the player.
10. Once enough orbs are collected the boss will die and trigger a death animation and the player wins screen.


Known problem areas
1. Restart button does not work on you won screen
2. Ghosts were originally transparent, but when changing scenes it caused the alpha of the ghosts to decrease making them
hard to see. Ghosts have been made opaque until I can debug the issue.
3. We cannot put rigid bodies on the player or NPC because they will fall through the floor. Therefore the ai does not have a navmesh.

Manifest:
1. Bradley
	a. Ghosts in room_1
		i. Assets/prefabsBH/CowboyRIO_Normal.prefab
		ii. Script: Assets/ScriptsBH/GhostBehavior.cs
		iii. Waypoint associated with pathfinding
		iii. Animations: Assets/AnimationsBH
		iv. Recorded sounds for animations: Assets/AudioBH
	b. Final boss in BossRoom
		i. scaled up version of on Assets/prefabsBH/CowboyRIO_Normal.prefab
		ii. BossProjectile, script: Assets/ScriptsBH/ProjectileBehavior.cs
		iii. Boss Behavior: Assets/ScriptsBH/BossBehavior.cs
		iv. Light orb scripts to damage boss: Assets/ScriptsBH/BossLightOrb.cs
		v. Light orbs added in BossRoom
		vi. Animations: Assets/AnimationsBH
	c. You Win Screen canvas in BossRoom
		i. button script: Assets/ScriptsBH/RestartGame.cs
	d. Made light bar persist across scenes
		i. Assets/ScriptsBH/DontDestroy.cs
	e. Added canvas called BossHealthBar to track health of boss

2. Jesse
	a. 3D modeling and design of Room_1
		i. Created the layout of Room_1
		ii. Utilized Probuilder to 3D model the Assets: Entrance, Lava_Floor, Spike Room, TrapBuildingTwo, Spike and Ceiling
		iii. Used prebuilt wall prefabs from the Prison Assets Pack to construct a maze with spike pits inside the Spike Room
		iv. Used the column prefabs from the Prison Assets Pack to create the Pillar Jump Blocks as well as the Column Jump sections
		v. Added Lava Area Light to simulate light emission from the lava floor. Light will only reflect off of items with default layer so it does not illuminate the inside of buildings.
		vi. Added background music to the Main Room scene and Room_1 scene.
	b. Checkpoint/Spawnpoint System
		i. Checkpoint.cs and Spawnpoint.cs are two scripts that control spawning the player at the most recent checkpoint that has been crossed when the player dies. The spawn point script can be used to create a spawn point where ever needed.
		ii. Added PlayerDeath.cs and PersistantObject.cs which attaches to the player. PlayerDeath will spawn the player at the most recent checkpoint on death, PersistantObject makes sure the player doesn't get destroyed when going between scenes.
	c. Persist Player Across scenes
		i. Created PlayerManager.cs script that handles spawning the player into a new scene as well as keeping track of current spawn points.
	d. Moving Platforms
		i. Created Platform prefabs that have a PlatformMovement.cs script attached to them
		ii. The script will show a gizmo outlining the movement path. The Move distance, speed, and direction can be adjusted as well as the Phase Offset so the platforms can start at different positions.



3. Patrick

4. Tao
	a. 3D Modeling and design for MainRoom and the BossRoom
		i. Created layout for MainRoom
		ii. Utilized Probuilder to 3D model the Assets: MainRoom, Elevator, Stairs, Room entrances, cell doors
		iii. Added torch lighting for the doors and elevator
		iv. Created the layout for the bossRoom
		v. Utilized Probuilder to 3D model the Assets: BossRoom, Columns, CellDoors
		vi. Created prefabs for cell doors, corridors, cell doors
	b. Scene loading
		i. Created colliders with scripts which load in scenes from the main room to room_1 and the BossRoom
	c. Main Room doors
		i. Animated main room doors
		ii. Added scripts for colliders and doors so that the animations begin when the player completes room_1
	d. Elevator
		i. Animated the elevator to the BossRoom
		ii. created colliders and scripts for triggering the elevator and transitioning to the BossRoom
		iii. Updated scripts to manage player light meter to disable while on the elevator and reenable in BossRoom


