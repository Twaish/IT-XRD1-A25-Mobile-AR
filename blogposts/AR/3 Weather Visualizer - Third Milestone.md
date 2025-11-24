# Weather Visualizer - Third Milestone
## Final UI ✓
Thomas Dan

The UI has been linked with the api and changes depending on the data given by it. The UI consists of 3 parts:
- Simple information: This displays the day, icon for forecast, and temperature in the top left corner of the app.
- Full information: This window can be toggled using a button with a sun icon and displays the simple information as well as humidity and other metrics for the upcoming days.
- Time changing slider: Using this you’re able to change the day and time to view the forecast for that day. The slider can be toggled on and off with an arrow button on the right and when on it can be toggled between 2 modes, either switch the day or switch the hour for that day.


## Visualizers for fog, snow, cloudy weather X
### Fog ✓
Jonas

Since the other weather effects were somewhat similar to how rain behaves with wind, just like leaves, snow etc, we decided to go with fog as that can be quite important to look into when driving. We simulate this by making a particle sphere with our own made “smoke” effects that encapsulates the player and thus make it hard to see past that sphere. 

Based on the view distance from the API the scale of it will then be changed to get smaller or bigger depending on the circumstances and if there is any fog at all. Currently there is however a limitation of the smoke particles making them look blocky and also limit how many there are since it overloads the graphics capability of the device itself. Something to look into in the future is giving them less polygons so it is easier to render. 


### Snow (✓)
Dan

The snow was already kind of implemented considering the effects made for the leaf particles and the wind particles are very similar and minor changes have to be done. To make this we simply had to change the texture of the particles and slow them down to simulate light snow particles. Although we have the effect, we haven’t actually added it to our scenes as part of the effects we show because of time-based difficulties.

### Cloudy weather X
Dan

The idea for this effect was to add clouds from the forecast and shade the surrounding area underneath. We thought of using something called light estimation, which figures out the light for the real environment making us able to apply shading to the area. However, we weren’t able to implement this as we didn’t have time and wanted to focus on the other effects which we thought were more important. 

But this could be done using perlin noise or the like to make a cloud map and creating volumetric shapes at areas where values peak or adding simple cloud models to the scene. These can then be added above the player and light estimation can be used to cast shadows onto the real environment.

## Compass and or relative position (✓)
Jonas

When the user enters the AR space it is crucial that the degrees align with the real world compass to ensure the user actually gets the correct sun position and wind direction among other future effects. With the current implementation the idea is to use the phones magnetometer that should fetch data for 0 degrees heading north and then align those values with unity objects that got those particle effects using the transformer coordinates. 

Unfortunately not all phones, especially android do not have a magnetometer thus we used a fallback that uses the gyrosensor to track rotation relative to opening the app. This will sadly never be as accurate and required for the user to open the app in the perfect north direction. It was discussed that it may be a good idea to provide a calibration button for the user but they will never know for sure the accuracy of it.

## Make weather follow user X
Dan Alexander

The plan was to make the weather follow the user wherever they went. But this was difficult to implement as the effects and particles were affected by the wind speed and wind degree making them offset from the player's position and in some rotation. We dropped this idea not because it was too complicated but because it wouldn’t look good when following the player. 

Another reason as to why we decided to not have this is because we thought a user might mainly would rather quickly look into the app to see how the weather would be at their current place/location, and might not walk too much around with the app open. If they were to go to another place and open the app again it would then as they reset the app show the effects at the new location.
