red = unity.Color.red
blue = unity.Color.blue
green = unity.Color.green
yellow = unity.Color.yellow
white = unity.Color.white

def createCube(x, y, z, color):
	c = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
	c.renderer.material.color = color
	c.transform.position = unity.Vector3(x, y, z)

class Cube():
	def __init__(self, x, y, z, color):
		self._x = x
		self._y = y
		self.z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.renderer.material.color = self._color

	def moveForward(self, z):
		self._object.transform.position = unity.Vector3(self._object.transform.position.x, self._object.transform.position.y, self._object.transform.position.z + z)
		self.z = self._object.transform.position.z
		
	def changeColor(self, newColor):
		self._object.renderer.material.color = newColor

	def getColor(self):
		return self._object.renderer.material.color


class Sphere():
	def __init__(self, x, y, z, color):
		self._x = x
		self._y = y
		self._z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Sphere)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.renderer.material.color = self._color


	def changeColor(self, newColor):
		self._object.renderer.material.color = newColor

	def getTransform(self):
		return self._object.transform


class Snowman():
	def __init__(self, x, y, z):
		self._head = Sphere(x, y + 1, z, white)
		self._body = Sphere(x, y, z, white)
		self._bottom = Sphere(x, y - 1, z, white)
		self._obj = unity.GameObject()
		self._obj.name = "Snowman"
		self._head.getTransform().parent = self._obj.transform
		self._body.getTransform().parent = self._obj.transform
		self._bottom.getTransform().parent = self._obj.transform
		#self._obj.AddComponent(unity.Text)

