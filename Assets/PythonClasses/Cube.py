class Cube(PythonUnityPrimitive):
	def __init__(self, x, y, z, color):
		PythonUnityPrimitive.__init__(self, x, y, z, color, unity.PrimitiveType.Cube)
		self.spinning = False
		self.spinSpeed = 5

	def spin(self, speed = 5):
		self.spinSpeed = speed
		self.spinning = True

	def stopSpinning(self):
		self.spinning = False

	def update(self):
		if self.spinning:
			self._object.transform.Rotate(unity.Vector3.up, self.spinSpeed * unity.Time.deltaTime)

