class Cube(PythonUnityPrimitive):
	def __init__(self, x, y, z, color):
		PythonUnityPrimitive.__init__(self,x, y, z, color, unity.PrimitiveType.Cube)
		self.moving = False
		self.moveSpeed = 5
		self.spinning = False
		self.spinSpeed = 5

	def move(self, speed):
		self.moveSpeed = speed
		self.moving = True

	def spin(self, speed = 5):
		self.spinSpeed = speed
		self.spinning = True

	def stopRotating(self):
		self.spinning = False

	def moveForward(self, z):
		self._object.transform.position = unity.Vector3(self._object.transform.position.x, self._object.transform.position.y, self._object.transform.position.z + z)
		self.z = self._object.transform.position.z

	def update(self):
		if self.moving:
			newPos = unity.Vector3(self._object.transform.position.x, self._object.transform.position.y, self._object.transform.position.z + self.moveSpeed)
			self._object.transform.position = unity.Vector3.Lerp(self._object.transform.position, newPos, unity.Time.deltaTime)
			self.moving = False

		if self.spinning:
			self._object.transform.Rotate(unity.Vector3.up, self.spinSpeed * unity.Time.deltaTime)

