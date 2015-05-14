import math
import UnityEngine as unity

red = unity.Color.red
blue = unity.Color.blue
green = unity.Color.green
yellow = unity.Color.yellow
white = unity.Color.white
black = unity.Color.black
magenta = unity.Color.magenta
grey = unity.Color.grey
clear = unity.Color.clear
cyan = unity.Color.cyan

# sky materials, load first
greenWithPlanetSky = unity.Resources.Load("Skyboxes/DSGWP")
redWithPlanetSky = unity.Resources.Load("skyboxes/DSRWP")
spaceSky = unity.Resources.Load("Skyboxes/Space1")
cloudySky = unity.Resources.Load("Skyboxes/cloudy")
sunnySky = unity.Resources.Load("Skyboxes/sunny")
starrySky =  unity.Resources.Load("Skyboxes/StarSkyBox")
defaultSky =  unity.Resources.Load("Skyboxes/dawndusk")

# reset weather
weatherRain = unity.Object.Instantiate(unity.Resources.Load("Weather/rain"))
weatherSnow = unity.Object.Instantiate(unity.Resources.Load("Weather/snow"))
weatherRain.gameObject.SetActive(False)
weatherSnow.gameObject.SetActive(False)

# reset skybox
unity.RenderSettings.skybox = defaultSky
