# Terminator - Second Milestone
## Health and Point System ✓
Dan

To not make this into a showcase of functionality, but into an actual game, a health and point system can be implemented. The player should start with some amount of health and will take damage from the projectiles shot by the minibots or by contact with the grounded robots. When the player eventually dies, a menu should show and they should be able to view their score along with their kills. 

Here we display how many kills of each type of enemy was achieved by the player along with how many points the enemy kills made up of total score (each enemy type could give different points). Since we have a time factor as part of the gameplay, we want to include this into the final scoring as well. The final score will then be based on the number of each enemy type killed and the total time survived. 


## Saber recall ✓
Jonas

Using the new unity input system we wanted a button that could recall the saber since everytime we threw it in a no gravity environment or supposed to be for the saber, it would fly off in the distance. The recall was made with interactors and interactables toolkit to make a velocity that would make it fly over to the position of the players hand and equid it again with the XR rigs right hand controller. 

Doing testing the saber can however get in a glitchy position between the activation point of holding at the and estimated position when recall activates so if the player moves around a lot this value needs to be updated frequently unless the player is fine with holding it using the default floating object interaction the comes with the XR rig.

## Saber homing ✓
Jonas

Another improvement to the saber is making it fly towards the drones fairly similar to the reflection of the drone shoots. While in principle it worked pretty similarly it was hard to properly trigger, since we wanted a throw mechanism to trigger it automatically when the object exits the vr controller. 

However, using XRGrabInteractable it was possible by having a listener look when the selected object “exists” and trigger this function. Since we already made various scripts before with rigidbody it was easy to make an object go towards another, but we didn’t want it to be deadlocked on the enemy. So we implemented a force and later we will look into an VR view angle required for it to work, So when the enemy is behind you but closest it would literally fly through you. Otherwise adding some force, spin and duration can help balance this ability out as well as strength of the homing that all were used to calculate the final trajectory. 

## Simple block environment / low poly sci-fi ✓
Dan 

Because the game is to be run on a head mounted display, this makes us limited on the amount of computational resources. To help resolve this issue, a simple environment made up of blocks and simple shapes, to reduce the rendering overhead. This also lets us use more of the remaining compute power for other important parts of the game, since we don’t want to focus so much on this part. 

## Enemy spawner + Time based increase of enemy spawns ✓
Thomas, Dan

The goal of the game should be to get as high a score as possible. This score could be involved with specific actions to increase the score, and the time the player is alive. 

To prevent the player from playing for too long and the game getting boring, the difficulty should increase over time as the game is played. The enemies spawning rate should be tied to the difficulty, so as time passes the more enemies are able to be spawned. If we make enemy variants we could also spawn more of these as the difficulty gets higher.

To get our enemies into the game scene in the first place, we implemented spawners for both the drone and robots. These spawners will continuously spawn our enemies in a designated area but with a limit to the amount of each. This limit will increase the longer the player plays the game.
