class Weather():
	def __init__(self):
		self._status = "Sunny"
		self._lastStatus = "Sunny"
		

	def setWeather(self, weatherString):
		self._lastStatus = self._status
		self._status = weatherString.lower()

	def getStatus(self):
		return self._status

	def update(self):
		rain.gameObject.SetActive(False)
		snow.gameObject.SetActive(False)
		
		if self._status != self._lastStatus:
			if self._status == "rain" or self._status == "rainy":
				rain.gameObject.SetActive(True)
				snow.gameObject.SetActive(False)
			elif self._status == "snow" or self._status == "snowy":
				snow.gameObject.SetActive(True)
				rain.gameObject.SetActive(False)
