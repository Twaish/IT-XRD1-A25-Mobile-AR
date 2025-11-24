# Milestones
## First Milestone
- Lightsaber
- Minibots that shoot the player
- Slicable
- Projectiles with visual aim
- Robots that attack the player close range

## Second Milestone
- Health and Point System
- Enemy spawner
- Saber homing/recall
- Simple block environment / low poly sci-fi
- Time based increase of enemy spawn

## Third Milestone
- Parry robot attacks
- Added various textures and particles
- Game over realitycheck
- Robot pathing (nav mesh)
- Points for slicing

# Extra Features
These are extra features which could be added if we achieve all the main features
- Shield + uncuttable obstacles you have to block
- Uncuttable obstacles that you have to dodge

# Terminator - First Milestone
## Lightsaber + Sliceable ✓
Jonas, Dan

The lightsaber is our game's main functionality and will be the player's main weapon. The lightsaber is able to cut objects thrown at the player, deflect projectiles, and cut enemies in half. 

For projectile parrying, the lightsaber will have two hitboxes, one for perfect parry so the projectile goes straight back to the enemy and one for just basic parry that will randomly launch away from the player. 

Using the demos ability to move objects back and forth it is now possible to also use the force to move the saber over to the drones so they can also be sliced. 

## Minibots that shoot the player ✓
Jonas

The mini bots also known as drones are currently flying around with fixed distance to the player and shoot with fixed interval. They do spawn within a boundary of the player at random with a maximum of 5 in total. They aim at the player with 100% accuracy where the goal is for the player to deflect the shot and hopefully hit the drone with it since the game is only supposed to have melee combat as of now. 

It is inspired by the Star Wars universe with the deflection of light sabers. The player will have in the future a small window to make a perfect parry that will cause it directly to hit the enemy, otherwise go in a random direction away from the player. 

## Projectiles with visual aim ✓
Jonas

The bots now also have a laser pointer at the player that should give them time to either have a chance to parry it or dodge it entirely by moving away from it. Using the very thin line rendered it will now point at the player's camera, while this looked fine in the unity scene builder with the different POV it caused them to be blinding and thus have an offset but that would also mean they could be hard to spot from certain angles. 

In the end it seems like the best position was around the chest. With the new improved spawner they will have a more well defined area to fix this. These later pointers can also be used to indicate what type of variant in the future that are about to attack as well as helping the player dealing with the 360 degrees of enemy spawns.

## Robots that attack the player close range ✓
Alexander

The grounded enemies are custom made robots with a weapon, they use navmesh for their AI to move and will move towards the player when in range and stop at a certain distance where they will try to hit the player with their light saber if the player moves towards them as well the enemies will back up to try and retain some distance from the player, though this is done with transform as the nav agent did not seem to want to do so and it still adheres to collisions, and does replicate as one would blindly move back as they do. The grounded enemies are made to be sliceable; this means when sliced by the player's sword they will split and cease to function and fall to the ground in pieces.

### Grounded enemies weapon ✓
Alexander

The grounded enemies will have weapons. Currently they have a light saber like the player that they swing down to try and hit the player. Here the weapons have a mesh so we can detect collisions with either the player or the players weapons. 

If the players and the enemies weapon collide as the enemies are attacking the enemies and their weapons will go back to their default position and be stunned so they will not be attacking for a slightly extended time, allowing a good window for the player to attack. If the enemy's weapon collides with the players they should take damage.
