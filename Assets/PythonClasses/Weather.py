class Weather():
	def __init__(self):
		self._status = "Sunny"
		self._lastStatus = "Sunny"
		self._object = None

	def setWeather(self, weatherString):
		self._lastStatus = self._status
		self._status = weatherString.lower()

	def getStatus(self):
		return self._status

	def getObject(self):
		return self._object

	def update(self):
		weatherRain.gameObject.SetActive(False)
		weatherSnow.gameObject.SetActive(False)
		
		if self._status != self._lastStatus:
			if self._status == "rain" or self._status == "rainy":
				weatherRain.gameObject.SetActive(True)
				weatherSnow.gameObject.SetActive(False)
			elif self._status == "snow" or self._status == "snowy":
				weatherSnow.gameObject.SetActive(True)
				weatherRain.gameObject.SetActive(False)
