Names: Bradley Hammond, Jesse Segall, Patrick Humberto Sandoval, Tao Deneb Quan
Emails: bhammond33@gatech.edu, , , 
Canvas account name: bhammond33, , , 
Start scene: StartScreen 

How to play and what parts of the level to observe technology
requirements: 

Player controls are wasd on mouse and keyboard. Shift allows the player to run. Space allows the player to jump.

1. Player starts in the MainScene, player must travel into open cell door.
2. Player will then transfer to Room_1.
3. Player will oberve elevators with light orbs and green and red power ups. Collect orbs for effects. 
White light orbs will replenish light meter.
4. Traverse elevators, if you fall into the lava or spikes you will respawn at the last checkpoint. 
5. At the end of the first room you will encounter an ai enemy ghost. The ghosts will chase you if you get too close.
They will alert you will a sound when then aggro. If they collide with you the ghosts will die and steal your light. 
6. At the end of this scene there is a doorway that will bring you back to the main room.
7. Have the player go up the stairs and into the big door for the final boss fight.
8. Once in the boss room, the player must collect light orbs to damage the boss while dodging projectiles summoned by the boss
9. Once the boss reaches 50% health the boss will start spinning and moving towards the player. 
10. Once enough orbs are collected the boss will die and trigger a death animation and the player wins screen. 


Known problem areas
1. Restart button does not work on you won screen
2. Ghosts were orginally transparent, but when changing scenes it caused the alpha of the ghosts to decrease making them
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

3. Patrick

4. Tao
	