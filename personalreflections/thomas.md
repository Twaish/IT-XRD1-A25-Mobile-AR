## Thomas Personal Reflections (331850@viauc.dk)

### AR

- UI
- Current Weather UI
- Forecast UI
- UI and API connection
- UI Toggle buttons

I feel like the AR app was a bit of a mess, in the sense that we struggled finding good ways of incubating unique features. We wanted to find features that could be represented much better in an AR environment, as opposed to simple text and graphs. 
The main thing we ended up trying to do was to visualise weather in a more intuitive way, since looking at the numbers of a normal forecast doesn’t mean as much as being able to see it. 
However, a lot of ideas were not really achievable for us with the tools Unity gave us and the limitations of the phones we worked on. 
We had a lot of issues with making and tracking surfaces on the white background of the campus walls. This is because, to make a surface it creates a point cloud, these points are made by finding differences, like the grain in a wooden table or the edge of said table.

I relatively quickly began working on the UI for the AR app since I had prior experience building an UI for an AR app in unity, which meant I had an idea of where I could start. 
And since I had started on it I made it my main task to look over. 
That means I did not do as much AR specific development as I feel like I should have done, but I did help here and there with other features if it was needed. 
For instance, I helped a bit with the particle system to make the fog. For the UI I incorporated the API that we had found, and some of the features the others had that needed UI elements.

### VR

- Shield (model, collision, deflection)
- Saber (trail, collision)
- Shaders (Laser projectiles, Saber, Shield, trails)
- Drones
- Laser sight
- Laser projectiles
- Game environment
- a little UI

During the the development of our VR project I had a lot of fun experimenting with shaders, and i am very proud of the shield i made the model of the shield itself is made using probuilder, and the shader is using various elements like a hexagon texture found on https://www.cgbookcase.com/textures/white-hexagonal-tiles-01, a fresnel effect for an edge glow and UV distortion for at wave effect.
We had a lot of issues working around the pre-made XR rig in unity. It would probably have been harder to build it ourselves, but since we didn’t we also didn’t know how it’s inner workings. An example would be when we tried to change the scene, but keep the rig. the rig would not be moved to 0.0.0 if it was with the unity playview, but if it was moved in VR i worked for some reason.
We also faced some weird framerate issues, where some collisions didn’t work on my old mac because of low frame rate, but worked fine when we tried it on the headset. I spent an annoying amount of time working on the hit detection, even though it works well enough on the headset.



