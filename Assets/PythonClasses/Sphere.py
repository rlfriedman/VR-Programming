class Sphere():
	def __init__(self, x, y, z, color):
		self._x = x
		self._y = y
		self._z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Sphere)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.GetComponent(unity.Renderer).material.color = self._color


	def changeColor(self, newColor):
		self._object.GetComponent(unity.Renderer).material.color = newColor

	def getTransform(self):
		return self._object.transform

	def update(self):
		pass

	def getObject(self):
		return self._object