Names: Bradley Hammond, Jesse Segall, Patrick Humberto Sandoval, Tao Deneb Quan
Emails: bhammond33@gatech.edu, jsegall6@gatech.edu, , tquan8@gatech.edu
Canvas account name: bhammond33, jsegall6, , tquan8
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
		ii. Utilized Probuilder to 3D model the Assets: Entrance, Lava_Floor, Spike Room, ZombieHordeBuilding, Spike and Ceiling
		iii. Used prebuilt wall prefabs from the Prison Assets Pack to construct a maze with spike pits inside the Spike Room
		iv. Used the column prefabs from the Prison Assets Pack to create the Pillar Jump Blocks as well as the Column Jump sections
		v. Added Lava Area Light to simulate light emission from the lava floor. Light will only reflect off of items with default layer so it does not illuminate the inside of buildings.
		vi. Added background music to the Main Room scene and Room_1 scene.
		vii. Created a maze for a zombie horde to chase the player through.
	b. Checkpoint/Spawnpoint System
		i. Checkpoint.cs and Spawnpoint.cs are two scripts that control spawning the player at the most recent checkpoint that has been crossed when the player dies. The spawn point script can be used to create a spawn point where ever needed.
		ii. Added PlayerDeath.cs and PersistantObject.cs which attaches to the player. PlayerDeath will spawn the player at the most recent checkpoint on death, PersistantObject makes sure the player doesn't get destroyed when going between scenes.
	c. Persist Player Across scenes
		i. Created PlayerManager.cs script that handles spawning the player into a new scene as well as keeping track of current spawn points.
	d. Moving Platforms
		i. Created Platform prefabs that have a PlatformMovement.cs script attached to them
		ii. The script will show a gizmo outlining the movement path. The Move distance, speed, and direction can be adjusted as well as the Phase Offset so the platforms can start at different positions.
		iii. Created PlatformAttach.cs script so player moves with the platform and doesnt bounce around.
	e. Zombie Enemies
		i. Used a premade zombie model and created a custom 1D blend tree to blend between idle and attacking states
		ii. Created the AI for the zombie utilizing NavMesh and ZombieTest.cs script so the zombies will chase the player until they get in a certain range and the an attack animation will play. All animations are using root motion.
		iii. Added different sound effects for different types of zombies.
		iv. There are three different types of zombies that vary in their idle and chase animations.
		v. Zombies in the horde room will reset to their original starting position when the player dies. 
	f. List of all scripts originally authored scripts:
		i. AreaLightShine.cs, Checkpoint.cs, LightDecay.cs, OrbDamage.cs, OrbShooter.cs, PersistantObject.cs, PlatformAttach.cs, PlatformMovement.cs, PlayerDeath.cs, PlayerManager.cs,
		RoomLoaderTest.cs, SpawnPoint.cs, SpikeRoomZombieAudio.cs, SpotlightController.cs, ZombieTest.cs



3. Patrick
    a. Player Abilities
        i. Created scripts to modify the character’s jump height and speed
        ii. Created the orbs that allow the player to gain these abilities
        iii. Created UI elements to track these power-ups and their duration
        iv. Assisted with orb respawning and placement
    b. UI Elements
        i. Created the pause UI elements, with buttons and functionality for restart, resume, and quit
        ii. Created a light status bar that keeps track of the player’s health
    c. Player Design
        i. Created player design using existing mesh; added a light source that visually represents health
        ii. Created script that updates the light source based on player health
    d. Start and End Scene
        i. Created the start scene with art, music, and a start game button
        ii. Created the end scene triggered after boss level completion, including a restart button
    e. Bug Fixes
        i. Fixed bugs related to player death and health reset
        ii. Resolved camera issues such as clipping through walls
    f. Design Changes
        i. Added design elements like platforms to show loading zones for next scenes
        ii. Implemented light orbs to guide the player through the game
    g. Boss Room Enhancements
        i. Edited lighting and structure to prevent the player from falling
        ii. Updated boss appearance with a new skin and animation set
        iii. Added new AI states for the boss to chase the player
        iv. Fixed bugs with gates and room logic
    h. Main Room Design
        i. Enhanced room interactivity by adding design assets, such as starting room to segment off the player
        ii. Blocked off unused exits
		iii. Added images and lights to improve game feel
		iv. Added additional buildings and treasures to the main room for exploration
    i. Room 1 Enhancements
        i. Created a secondary path including rotating items, platforms, and spinning donuts
        ii. Added spike traps that damage the player
        iii. Fixed bugs like random spawning and lack of death triggers
    j. Score & Progression System
        i. Created a score system, timer, and supporting scripts
        ii. Added collectible trophies that increase player score
    k. Tutorial & Gate Logic
        i. Implemented gate control logic that responds to tutorial completion
        ii. Added decorative wall art assets for better game feel
    l. Room 1 Moving Assets
        i. Scripted movement logic for dynamic room elements
	m. ScriptsScripts Created: AmmoPickup.cs, AxisRotator.cs, CircularMovement.cs, Collectible.cs, 
		CylinderRotator.cs, MovingPlatform.cs, MovingSpike.cs, OpenGate.cs, PlayerShooting.cs, ScoreManager.cs, 
		TorusGenerator.cs, FlickeringLight.cs, HighJumpOrb.cs, LightOrbRespawn.cs, SpeedBoostOrb.cs, ElapsedTime.cs, 
		PauseMenuManager.cs, StartScreenManager.cs


4. Tao
	a. 3D Modeling and design for MainRoom and the BossRoom
		i. Created layout for MainRoom
		ii. Utilized Probuilder to 3D model the Assets: MainRoom, Elevator, Stairs, Room entrances, cell doors
		iii. Added torch lighting for the doors and elevator
		iv. Created the layout for the bossRoom
		v. Utilized Probuilder to 3D model the Assets: BossRoom, Columns, CellDoors, Elevators, Damage Orbs, Boss Projectile
		vi. Created prefabs for cell doors, corridors, cell doors, moving elevators
	b. Scene loading
		i. Created colliders with scripts which load in scenes from the main room to room_1 and the BossRoom
	c. Main Room doors
		i. Animated main room doors
		ii. Added scripts for colliders and doors so that the animations begin when the player completes room_1
	d. Elevator
		i. Animated the elevator to the BossRoom
		ii. Created colliders and scripts for triggering the elevator and transitioning to the BossRoom
		iii. Updated scripts to manage player light meter to disable while on the elevator and reenable in BossRoom
	e. Adjustable camera sensitivity
		i. Added a slider to the menu and linked it to the player to permanently update the player's camera sens.
		ii. Changed the object hierarchy so that the menu can piggy back off of the player's persistance across scenes
	f. Boss Combat
		i. Added purple light orbs that damage the Boss
		ii. Wrote scripts to damage the boss when the light connects with the boss and obstruct beam on some terrain
		iii. Updated the boss projectile logic to launch from the boss to the player. Updated speed and damage in scripts
		iv. Added collision sounds for the projectile hitting the player. Fixed logic around projectiles disappearing after hit
	g. Player Light Orb
		i. Created the orb that sits on top of the player
		ii. Created and adjusted scripts to affect the color of the orb and the area light behind the player when their
		health goes down or takes damage
		iii. Changed lighting settings to prioritize the light near the player by baking other light sources and using realtime
		for the light on the player
	h. List of all scripts originally authored:
		i. CamSense.cs, ColumnRotationTiming.cs, DoorControl.cs, DoorStarter.cs, ElevatorControl.cs, ElevatorStarter.cs
		LightBeamController.cs, RoomLoader.cs, BossRoomLoader.cs, PlayerLightOrb.cs
	i. Scripts functions contributed to
		i. Player motion - adjusted jump speed per feedback from playtest.
		ii. Fixed bug where speed up orbs have a multiplicative effects
		iii. Contributed towards fixing player death not working in multiple areas
		iv. Boss projectile mechanics


