# Terminator - Third Milestone
## Parry robot attacks ✓
Alexander

If the player blocks the sword as it attacks it will immediately begin to draw back its sword and become stunned. Stunned means that the enemy will cease to move or attack for a short amount of time defined in the unity inspector. A visual change will also take place so it is possible for the player to see when an enemy is stunned.

## Shield ✓
Jonas Dan Thomas

Since the shield’s being held in VR, it’s important to find the right size and make it somewhat transparent to see the shots deflected. The deflection on the saber is no longer needed as it would be better to have that on a shield and it makes more sense and fixes issues with the two deflect zones for the saber that overlaps which was not reliable for separate deflection detection.

We made a custom model and shader for the shield. The design is inspired from Overwatch and will have an amount of hit points before recharging is needed. During this, the player must use their real movement to avoid being hit, go around map obstacles or use the saber to cut the lasers/enemies.

The size of the shield, regeneration time and hitpoints  is uncertain, since it depends on how difficult the increase (enemy spawns) curve is going to be for the final game. The shield can be grabbed using the XR grab interactable like the lightsaber. The shield can still be locked into the user's hand or flown around using the force (The comes with the XR rig controller preset) letting you push enemies away. 

## Added various textures and particles ✓
Alexander

We make some custom shaders to get the right glow on objects, like the sword and the lasers. The lasers are just spheres, so we are using trail effects to sell the speed of the lasers and make it look longer than just a sphere.
Our main game uses lightsabers and robots therefore we have decided to go with a more sci-fi style for our game as we thought that fit well with our current ideas.
### Imports
Alexander

We imported the overall design for our drones and modified it slightly to fit our ideas.
We have imported some sci-fi objects like interior objects and walls.
We have also imported a death scene where the player spawn in an insane asylum that is also taken from the asset store. This scene is supposed to be a form of waking up to reality. 

### Custom
Alexander

We also have also made some simple custom designs for some of our objects that we use.
The ground enemies are made using some of the default objects from unity combined and placed in various locations and a simple texture drawn in paint.
With the simple unity shapes we have also added some legs inspired by R2D2 from star wars and some small stationary arms to hold the swords.
The office building is also a prefab made of planes where a custom image made in paint is added onto the surface.
We have also made our lightsabers with default unity objects where we have added textures.
The projectiles are simple capsules with a custom texture.

## Robot pathing (nav mesh) ✓
Dan Alexander

To avoid making the robot try to simply pass through walls directly towards the player, we create a pathing environment (nav mesh). This was easy to implement as you simply had to add a Nav Mesh Surface component to the environment and make it target a certain layer for it to compute it. 

There was an issue with the robots walking through certain walls which was fixed by assigning the wall the layer used for baking the nav mesh pathing environment.

The robots are assigned to be the nav agents and therefore no longer use transform to move, but the nav mesh. Though there was a problem with making the robots retreat when the player came too close using nav mesh so that is the only part of their movement that still uses transform on the enemies and force them back. 

Though logic wise this does make somewhat sense as they can still not go through objects but might back into them, and this does somewhat mimic a person blindly moving backwards as they do.


## Points for slicing ✓
Dan

Because our script for slicing objects in the game emits an event when an object has been sliced, we can listen for this event to fire inside a separate script and call different functions in order for us to tally points. A .Die() function was added to each implementation of our enemies and would be called through different interactions. 

A drone could die from a deflected laser or, like the robot, be sliced in half. Using an event based approach, each enemy and each spawner would subsequently emit an event when this happens for a global score manager to pick up and add points using our point system. 


## Player’s hit box + Game over realitycheck (✓)
Jonas Dan

We quickly found that a fixed hitbox (capsule collision) attached directly to the player is not a great solution when the player has a lot of free movement. Often the shots got “unfarily” detected as a hit, so we made a script to adjust for the player's movement in VR e.g. height on runtime, so if the player crouches the hitbox will better follow the player. This hitbox will overlap the existing from the XR rig since it cannot be adjusted the same way. Another problem was the hitbox often hitting the ground and the controllers causing lots of collision calls, while this was solved by a simple tag solution.

When the player dies we wanted passthrough to simulate the game being over, but it was not possible on quest 1, since it was system- and not application-layer. Using a newer system and having to install a big package just for this, was not worth it. Instead the player will be moved to an insane asylum and they can then restart the game on a button press. 

An issue with transporting the player to the asylum is that we’re unable to move them so they remain in the coordinates they were at before being transported making it possible to spawn outside the asylum room.
