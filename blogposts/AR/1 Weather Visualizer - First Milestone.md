
# Milestones
## First Milestone
- Simple weather fetching setup
- Visualizers for rain and wind
- Simple UI for changing the day and time of day for weather visualization

## Second Milestone
- Visualizers for clouds, sun and moon cycles/phases, fog, snow and hail
- Wind intensity and direction
- Allow users to change weather visualization location to other cities and countries
- Weather alerts and simulate them (Possibly scale based on interpreted alert severity)
- UI for weather forecast and alerts
## Third Milestone
- Mapping shadows in the environment based on cloud formation
- Visual distortions based on certain parameters
- Season themes
- Visualizers for stars
- Temperature heatmap
- Final UI


# Weather Visualizer
When entering the weather app, it’s hard to visualize how the different parameters affect the weather. Things such as a wind speed of 15 km/h is difficult to imagine what impact a wind has on the surroundings and the degrees of where the wind is coming from translated to the real world. This app is to make it possible for users to bring these weather conditions into the real world and make it easier to visualize how they affect each other.

# Weather Visualizer - First Milestone
## Simple weather fetching setup ✓
Dan

Implementing scripts for this functionality was pretty straight forward. For fetching any weather data, we went with free options like weatherapi.com. This api provided us with different weather metrics such as the speed of the wind, the degree of the wind direction, the air quality, etc.. We are also able to fetch the forecast of upcoming days for up to 2 days in advance. Using this data, we should be able to implement many different weather visualizers and appropriately configure their behavior based on the data values. 

To prevent overloading the api (even though it does provide a million api calls monthly) and to improve performance, we made the script, cache the latest fetched weather data from calling the api and reuses it throughout the app’s lifetime. Although the app fetches the latest weather data on startup, making numerous api calls, we were ok with this behavior as users realistically would not do this with normal use of the app.


## Visualizers for rain and wind ✓
Dan

For creating a great amount of our visualizers, we decided on using Unity’s particle system, as this was easy to use with many tutorials and is very customizable. To not make the task of creating visualizers as time consuming, assets from the Unity store were used to make the rain visualizer. 

Using the emission on the particle system that came with the rain visualizer, we could control the amount of rain that was pouring down, and by hooking this value up to the weather data from the api, we ended up with rain that would change based on real time weather data. Finding wind effects was harder than finding rain effects as visualizing the wind itself was a bit niche. 

Games usually use other effects like blowing leaves and swaying the environment to give the effect of wind. We couldn’t find a proper wind visualizer so we made our own which was simply emitting particles with a trail to simulate the wind streaks. To add more to our wind we also added particle systems for the blowing leaves and could use both in conjunction.

Using the same approach as with the rain, we connected the weather data to the direction at which the wind came from and translated the values to force vectors and applied these to the particles. All particles for the wind effect were applied with some noise value to make it seem more natural.

## Simple UI for changing the day and time of day for weather visualization ✓
Thomas

The main goal of implementing the UI now, was to get the functionality working so that we can test the time changing feature on the phone. The UI features some text to explain its purpose, like the current hour set and the day. To change the hour the user uses a scroll bar to let them intuitively adjust the time of day by sliding back and forth within a minimum value of 1 to a maximum of 24. They can also toggle a button to adjust the day instead of the hour. Currently the UI is very barebones as our goal is on functionality rather than visual polish.
