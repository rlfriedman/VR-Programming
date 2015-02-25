class Cube():
	def __init__(self, x, y, z, color):
		self._x = x
		self._y = y
		self.z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.renderer.material.color = self._color
		self.moving = False
		self.moveSpeed = 5
		self.rotating = False
		self.rotateSpeed = 5

	def getObject(self):
		return self._object

	def move(self, speed):
		self.moveSpeed = speed
		self.moving = True

	def rotate(self, speed):
		self.rotateSpeed = speed
		self.rotating = True

	def stopRotating(self):
		self.rotating = False

	def moveForward(self, z):
		self._object.transform.position = unity.Vector3(self._object.transform.position.x, self._object.transform.position.y, self._object.transform.position.z + z)
		self.z = self._object.transform.position.z
		
	def changeColor(self, newColor):
		self._object.renderer.material.color = newColor

	def getColor(self):
		return self._object.renderer.material.color

	def update(self):
		if self.moving:
			newPos = unity.Vector3(self._object.transform.position.x, self._object.transform.position.y, self._object.transform.position.z + self.moveSpeed)
			self._object.transform.position = unity.Vector3.Lerp(self._object.transform.position, newPos, unity.Time.deltaTime)
			self.moving = False

		if self.rotating:
			self._object.transform.Rotate(unity.Vector3.up, self.rotateSpeed * unity.Time.deltaTime)