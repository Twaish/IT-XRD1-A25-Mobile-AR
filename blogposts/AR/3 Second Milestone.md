# 2 Second Milestone
## Visualizers for clouds, sun and moon cycles/phases, fog, snow and hail
### Sun and moon cycles/phases X
@Alexander @JonasPHenriksen, @Twaish 
The sun is created and can appear in the sky and should be approximately in the position where the actual sun would be compared to the location of the user. Though there have been problems with the compass so it might not be in the correct direction.

This is one of the issues we have been stuck on for a lot of the development time, since, if we could get it to work, we would have a reliable way of rotating and positioning our other visualizers.

### Fog, Clouds, Snow, Hail X
@Twaish @Alexander
Because of our focus on the sun and moon cycles/phases, and some of the challenges with the compass, we have not had the time to focus on these visualizers. Having some of these visualizers such as fog and snow made until the next devblog is possible, as the fog could be made using Unity’s fog engine and the snow be a version of the visualizer for falling leaves modified with a snowflake texture. 

## Wind intensity and direction ✓
@Twaish, @Jonas
This was already added as part of the visualizer for wind mentioned in the previous devblog, which uses the windspeed and winddirection given by the weather api. 

Currently the API also shows the max windspend in 0 degree position for daily since it is not possible to provide a wind direction for the entire.

## Allow users to change weather visualization location to other cities and countries X
@Alexander
Currently the app finds your current location and displays the relevant weather for where the user is located. There is also the current feature for if it is unable to find the users location it sets it to a default, this initially was implemented mainly for testing purposes. 

This is also an idea we are not fully sure on yet as we are discussing that it might not make sense for seeing other locations weather in the AR app.

## Weather alerts and simulate them (Possibly scale based on interpreted alert severity) X
@Thomas, @Twaish, @Jonas
The weather alerts we have chosen to drop, as the amount of effort it would take to add both UI and AR effect, for events that rarely happens does not seem to be worth it. Simulating them would also be misleading to the severity of the alert and there would be no reliable way of simulating these. 

Weather is already very unpredictable and having these effects are not only hard to imagine but also computationally infeasible

## Compass and or relative position X
@Jonas
When the user enters the AR space it is crucial that the degrees align with the real world compass to ensure the user actually gets the correct sun position and wind direction among other future effects. 

With the current implementation the idea is to use the phones magnetometer that should fetch data for 0 degrees heading north and then align those values with unity objects that got those particle effects using the transformer coordinates. 

Unfortunately not all phones, especially android do not have a magnetometer thus we used a fallback that uses the gyrosensor to track rotation relative to opening the app. This will sadly never be as accurate and required for the user to open the app in the perfect north direction. 

It was discussed that it may be a good idea to provide a calibration button for the user but they will never know for sure the accuracy of it.

## UI for weather forecast and alerts ✓
@Thomas, @Twaish
UI that lets the user see the weather forecast has been made, but the UI has not yet been linked to any code as of yet. That means the UI shows default values, and a placeholder icon for the weather. 

The icons we use were found on: https://www.flaticon.com, where five icons were picked and added to the unity project. These icons have to be linked with the ui and weather api so that when the api gives the weather the relevant icon is shown in the UI.

The weather alert we have chosen to drop, as the amount of effort it would take to add both UI and AR effect, for events that rarely happens does not seem worth it. Considering that we’re dropping the weather alerts, we mark this as complete.

## Updated Third Milestone
- ~~Mapping shadows in the environment based on cloud formation~~
- ~~Visual distortions based on certain parameters~~
- ~~Season themes~~
- ~~Visualizers for stars~~
- ~~Temperature heatmap~~
- Final UI
+ Visualizers for fog, snow, cloudy weather
+ Compass
+ Make weather follow user
