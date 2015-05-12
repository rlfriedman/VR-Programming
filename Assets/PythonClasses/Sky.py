class Sky():
	def __init__(self):
		unity.RenderSettings.skybox = defaultSky

		self._object = None

	def setGreenPlanet(self):
		unity.RenderSettings.skybox = greenWithPlanetSky

	def setRedPlanet(self):
		unity.RenderSettings.skybox = redWithPlanetSky

	def setCloudy(self):
		unity.RenderSettings.skybox = cloudySky

	def setSunny(self):
		unity.RenderSettings.skybox = sunnySky

	def setStarry(self):
		unity.RenderSettings.skybox = starrySky

	def setSpace(self):
		unity.RenderSettings.skybox = spaceSky

	def update(self):
		pass

	def getObject(self):
		return self._object

