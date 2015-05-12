class PythonUnityPrimitive():
	def __init__(self, x, y, z, color, primType):
		self._x = x
		self._y = y
		self._z = z
		self._color = color
		self._object = unity.GameObject.CreatePrimitive(primType)
		self._object.transform.position = unity.Vector3(x, y, z)
		self._object.GetComponent(unity.Renderer).material.color = self._color

	def getObject(self):
		return self._object

	def setColor(self, newColor):
		self._object.GetComponent(unity.Renderer).material.color = newColor

	def getColor(self):
		return self._object.GetComponent(unity.Renderer).material.color

	def update(self):
		pass