## Alexander Personal Reflections (331186@viauc.dk)

### AR:
- API for the Sun.
- Sun with location relative to the real world.
- Toggled sun based on sunset and sunrise.
- Helped others if needed.

#### Removed and Partial/Unsuccessful contributions:
- Rain effect.
- Skybox that removed/replaced the roof.
- Compass/magnetometer for real world orientation.

Started with a rough shared idea, unsure how to split the work. Also our experience of Unity differed.
Our project was made on the Unity AR foundation. Unity AR foundation uses the native AR SDKs of the device, it exposes features like "plane detection, anchors, device tracking" with Unity's agnostic API layer. Phones like Android or iOS are often VIO(Visual Inertial Odometry) based with SLAM(Simultaneous Localization and Mapping) like components for local environment understanding.
This gives Improved performance and latency, with tradeoffs like being less robust and less persistent mapping than full SLAM.
The rain I made, as I got used to Unity, became redundant. Someone made it quicker. I got more used to Unity and the particle effects. 
We wanted a transition to a sky, making the roof disappear, inside. I worked on the skybox. We dropped this after clarifying the essential features we wanted. The skybox became an extra idea for when we finished our prioritized ideas, if time allowed.
One of my main focuses was the sun. I found some different API’s, the best fit was “suncalc” it provided altitude, azimuth, distance among others, these were the ones we needed. With integration with our location servicemanager, and an object we had a sun relative to the real world. A bigger problem was the orientation. The app's opening was set as north, the rotation was based on distinct features, as was the default. When it lacked distinct features the orientation was not updated. To solve this, I looked into using the magnetometer in one of our devices. It would not work at all. Then someone else made a Gyroscope based one, not fully reliable, but worked and better than the default. I worked on more features for the sun, perfected it more, like making it disappear and reappear when under the horizon, to resolve overlayed on the ground. Helped my group members, we were all good at helping each other with problems and bugs. Amongst others I helped a few times with the location manager, which is more natural as the sun relied on it.

### XR:
- Robots
- Robots AI
- Robots movement
- Robots attack behavior
- Robots lightsaber
- Robots design
- Robots parry-able
- Robots stunnable
- Robots Spawner
- Insane asylum robots
- Defect drones and robots
- Shield regeneration (while intact)
- Mesh to shield for parrying.
- Helped others.

#### Removed and Unsuccessful contributions:
- Other asset

For the HMD(Head-mounted display) project, we had gotten more used to Unity, and task separating and delegating. We mainly used our own virtual headsets, therefore, able to work home and at school. We used Oculus Quest 1 and Meta Quest 3, both are HMD utilizing Inside-out tracking. They are easier to bring with us. There were problems with the Meta Quest 3, mainly wifi, laptop resource limitation, and others with the Meta Horizon app.
During development I mainly worked on grounded melee enemies, and spent a lot of time improving different aspects of them. They started as simple floating capsules, I used the Unity transform to manipulate them towards the player's XR Rig. I implemented movement behavior to ensure proper spacing from the player, based on the player movement and distance. I copied and modified the light saber for the enemies, made an attack script, so that they became able to swing at the player. I integrated it with the health system and player collider later. I also made behavior for the sword colliding with the player's sword or shield. Later I made a stun effect for the robots, both functional and visual. I made a simple design for them, so they were no longer capsules. I drew inspiration from R2D2 (Star Wars). I had an alternative inspired by Daleks (Doctor Who). I made the assets, textures, materials manually, through paint and combining Unity objects. I made an office building/sky scaper, before we began importing the assets. I later replaced the movement logic with the inbuilt navmesh in Unity. I made the robots' agents, baked the ground for a walkable surface, and automatically figured out how to navigate around objects. The transform would try to force through obstacles. I made a robot spawner repurposed and modified the drone spawner. I made scripts for defect robots and drones, for our asylum/reality check level. Made the drone spin, froze and dismantled the robots, gave them heartbeat like blinking eyes. I helped with slight modifications for some of our other elements, I made the shields regenerate slowly over time, even when not destroyed, made the shield work with and able to collide and parry enemies sword swings and helped other times as needed.

