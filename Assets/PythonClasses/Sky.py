class Sky():
	def __init__(self):
		self._greenWithPlanet = unity.Resources.Load("Skyboxes/DSGWP")
		self._redWithPlanet = unity.Resources.Load("skyboxes/DSRWP")
		self._spaceSky = unity.Resources.Load("Skyboxes/Space1")
		self._cloudy = unity.Resources.Load("Skyboxes/cloudy")
		self._sunny = unity.Resources.Load("Skyboxes/sunny")
		self._starry =  unity.Resources.Load("Skyboxes/StarSkyBox")
		self._default =  unity.Resources.Load("Skyboxes/dawndusk")
		unity.RenderSettings.skybox = self._default

		self._object = None

	def setGreenPlanet(self):
		unity.RenderSettings.skybox = self._greenWithPlanet

	def setRedPlanet(self):
		unity.RenderSettings.skybox = self._redWithPlanet

	def setCloudy(self):
		unity.RenderSettings.skybox = self._cloudy

	def setSunny(self):
		unity.RenderSettings.skybox = self._sunny

	def setStarry(self):
		unity.RenderSettings.skybox = self._starry

	def setSpace(self):
		unity.RenderSettings.skybox = self._spaceSky

	def update(self):
		pass


