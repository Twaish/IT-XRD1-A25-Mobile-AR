## Jonas Personal Reflections (330943@viauc.dk)

### AR

- Location service fetched from weather.api
- Add slider for weather switching 
- Rotate weather according to API 
- Add compass manager to sync the angel with real degrees. 
- Update sun visuals and align it with real degrees. 
- Add fog effect particles and activation.

Under the development of the AR project, I initially thought that using the default template from Unity AR foundation would suffice for an immersive weather app. However, using the SLAM and VIO only detects where the camera is, how it moves in the space and relatively sparse feature points in the environment (with AR Foundation plane detection), so it would never reconstruct objects with volume. Even with the built in AR plane detection, it only gets simple surfaces such as Floor, Tabletop, walls etc. and only loads nearby focused areas. 

To make it much more realistic we need to map arbitrary shapes, like roof edges, uneven terrain like a field etc. This would require ongoing frame mesh reconstruction, dense depth maps and a stable world mesh that can update efficiently. While this is unrealistic for general smartphones, certain phones/devices that have LiDAR could improve this experience slightly with a low resolution, slow update, no semantic (OpenCV Segmentation model could be used to improve this) updates. Having depth fusion from a sensor could create a better world building using ARMeshManager. Since our device did not have this, if more time was provided we could extend the SLAM, with our own geometry using point clouds on top, that would create a lightweight mesh of the world to improve the collision of the effects instead of basic planes. 

Another possibility would be using raycasting instead of Unity physics with colliders to do particles like rain drops using depth as we can estimate the particles raycast and remove or splatter particles when depth is reached. With better world mapping the effects would also be more likely to trigger properly. Since our current planes were just flat, they rarely properly collided since the short time frame from a rain drop to splash effect on the ground was not very reliable often because of incomplete mesh renders on low end hardware. 

Since AR already processes camera video, SLAM, IMU fusion, Unity rendering, having the vision of large particle systems is simply unrealistic as a realistic look was the original idea. Especially under fog creation, the entire particle system caused the application to lower resolution/FPS, crash, remove high amounts of particles so the fog looked like dust clouds in the final project. However a different style and more future-powerful hardware, combined with external computation could make this great in the future.

### VR

- Laser sighting
- Drones 
- Drone spawner 
- Laser projectiles
- Saber deflect (later shield deflect)
- blade recall 
- blade homing
- shield, shield recharge (health)
- game-over scene initialization.

Under the development of XR I experienced problems with the Locomotion that was default to the game. Since there rarely was enough space, we mainly used the joysticks exclusively which could cause nausea. Improving this by physically walking on the spot could make it more enjoyable, since this could also allow the player to adjust for pacing dynamically instead of being stuck to one speed. A lot of the time the hits also felt unfair and unnatural, since the hitbox of the player cannot be a simple sphere as we have 6 DOF and only having the HMD being the target also feels unnatural. It would improve the experience a lot by making the body visible in-game, in some way by predicting the body based on the HMD and controllers position. 

Another problem I also faced was the inside-out tracking as we wanted to take advantage of the 360 degree enemy spawns to make it more immersive with the support of sound and UI elements.However, using the shield away from the cameras caused it to not appear correctly, which made it more difficult to deal with enemy attacks from all angels. The simplest solution would be to switch to outside-in but it might also be possible by keeping the shield positioned relative to tracked play space near the player and not just the cameras only. This was usually a problem near the end especially when the FPS started to drop significantly because of more enemies. 

While this could be solved by changing the game design by making different variations of enemies (More health, shoot more frequently etc.) and keep a fixed spawn limit, it does not provide the same amount of fun and as the game expands we should have looked more into our projects settings and textures when it comes to performance. This became especially relevant when also casting the experience which caused it to be nearly unplayable. Looking into if Multi-view is enabled instead of multi-pass, if Foveated rendering is enabled to only render important details and not edges and consider if Asynchronous Reprojection should be used as stuttering was noticeable near the end with the low framerate, which also caused the game to behave unexpectedly since certain mechanics were tied to framerate could make the experience better in the future.
