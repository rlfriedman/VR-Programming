red = unity.Color.red
blue = unity.Color.blue
green = unity.Color.green
yellow = unity.Color.yellow

def createCube(x, y, z, color):
	c = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
	c.renderer.material.color = color
	c.transform.position = unity.Vector3(x, y, z)

class Cube():
	def __init__(self, x, y, z, color):
		self._x = x
		self._y = y
		self._z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.renderer.material.color = self._color


	def changeColor(self, newColor):
		self._object.renderer.material.color = newColor

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