## Dan Personal Reflections (331188@viauc.dk)

### AR

- Weather data fetching from API
- Rain, Leaves, Snow, Wind Streaks
- Scale and Rotate Effects Based on Weather Data

Developing the AR app was a learning experience.

The app uses video see-through in order to render the AR environment. The weather visualizer is markerless as it doesn't use patterns or images like QR codes or logos to initialize the experience, but initializes as soon as the app has started and applies the weather to the see through view. 

To get the weather for the current location, data was fetched from the weather api, which provides a lot, but required an api key to use. As much information as the api provided, it also made parts of the development difficult. An example could be having to deploy the app on a phone, but would not include the key correctly, essentially breaking it.

We also had problems with tracking on white backgrounds. AR often uses Simultaneous Localization and Mapping (SLAM) to create feature points, making a map for the view to determine its position. Creating feature points on white backgrounds is hard for SLAM and using the Visual Intertial Odometry for localization wasn't good enough, and unstable, because of the IMU drift. LiDAR could be used to create the reference environment even on white backgrounds and better the tracking on low texture surfaces, although with limitations on the range, but we didn’t have a phone with this capability. 

Unity’s particle system was used to make effects and some were imported. This made it easy to create effects like leaves and wind streaks, and have them work together. The weather data provides the wind-direction and -speed allowing for rotating and moving each effect collectively:

```cs
// WindEffectHandler.cs
public void SetWind(float windKph, float windDegree) {
  lastWindKph = windKph;
  lastWindDegree = windDegree;

  Vector3 windVec = WindUtils.CalculateWindVector(windKph, windDegree, windModifier);

  if (CompassManager.Instance != null) {
    float deviceHeading = CompassManager.Instance.GetHeading();
    Quaternion rotation = Quaternion.Euler(0, -deviceHeading, 0);
    windVec = rotation * windVec;
  }

  var forceModule = windParticleSystem.forceOverLifetime;
  forceModule.enabled = true;
  forceModule.x = new ParticleSystem.MinMaxCurve(windVec.x);
  forceModule.z = new ParticleSystem.MinMaxCurve(windVec.z);

  windParticleSystem.transform.rotation = Quaternion.LookRotation(currentWindVec.normalized);
}
```

But this rotation and movement is relative to the Unity scene initialized when starting the app, so a CompassManager was created for “syncing” the direction to the real world rotation. It was important as the app had a Sun to be placed correctly and the directional movement of the effects, be adjusted to match the real world as well.

The fog effect also had problems like a size limit for the particles, but also introduced performance issues, because of having to render many large transparent particles, so this was scaled down. Unlike the fog, many other effects in the app can collide with the environment. The AR experience template includes a Unity scene with scene reconstruction already enabled, so the app would reconstruct the scene, gradually letting each effect interact with it. 

The app became somewhat of a template scene with effects added to it, and we couldn’t find good ways of including more AR-based technologies without going out of scope for the idea in mind.

### VR

- Saber
- Saber Mesh Slicing
- Point and Health System
- Timer
- GUI for Health, Points and Timer
- Sound Effects
- Wiring Scripts Together

Developing the VR game was fun, but like the AR app, came with challenges. 
The saber uses a library to slice the objects using references to 2 points on the saber and use a line cast between the 2 points to create slice hulls from a direction projected by a velocity estimator.

Having 2 displays introduced an issue for the GUI, because it would be located at the edge of the view for just 1 of the displays. To resolve this we moved the UI elements to objects in the scene, having the health anchored to the left hand controller, the points on the right hand and the timer in front of the player at all times. This solved the issue of looking at the edge of the optical lens in order to view your stats, and made the UI visible in both displays. Having the timer in front of the player made it possible to clip it behind other objects, but we were fine with this tradeoff.

The headset uses inside-out tracking which has issues when tracking the controllers since they can move outside the tracking view or be obscured behind other objects. This is not an issue I personally faced as much since the game is to be played in an open space and the interactables are objects placed in front of you most of the time. Outside-in tracking could still be used as this lets you perform more sophisticated moves like blocking laser shots or slice enemies from behind your back and would get rid of these issues. 

Many scripts use an event-based approach for implementing their functionalities. The idea is to have many “generic” components which can be added to gameobjects and have the developer create scripts handling interactions between the events. An example of this is, could be for sound effects when slicing objects.

```cs
[RequireComponent(typeof(SoundPlayer))]
[RequireComponent(typeof(BladeSlicer))]
public class BladeSliceSfxPlayer : MonoBehaviour {
  private BladeSlicer bladeSlicer;
  private SoundPlayer soundPlayer;

  private void Start() {
    bladeSlicer = GetComponent<BladeSlicer>();
    soundPlayer = GetComponent<SoundPlayer>();
    bladeSlicer.OnSlice += HandleSlice;
  }

  private void HandleSlice(GameObject slicedTarget, Vector3 velocity) {
    soundPlayer.PlaySound("slice");
  }
}
```

The script retrieves a `BladeSlicer`- and `SoundPlayer`-component from its gameobject and subscribes to the `OnSlice` event with `HandleSlice` as the event handler. `HandleSlice` calls `soundPlayer.PlaySound` for playing the `“slice”` sound, added inside the inspector. We do the same for the health and point system, having UI controllers listen to change events and updating provided UI elements accordingly, as well as `OnDeath` events on enemies to tally points. 

Parts of the game were difficult to develop as the behavior inside Unity wasn’t consistent and behaved differently, deployed on a headset. Other than that, the game required a bit of practice but was fun to play after getting to know the controls.
