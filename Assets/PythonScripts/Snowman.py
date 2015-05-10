class Snowman():
	def __init__(self, x, y, z):
		self._head = Sphere(x, y + 1, z, white)
		self._body = Sphere(x, y, z, white)
		self._bottom = Sphere(x, y - 1, z, white)
		self._object = unity.GameObject()
		self._object.name = "Snowman"
		self._head.getTransform().parent = self._object.transform
		self._body.getTransform().parent = self._object.transform
		self._bottom.getTransform().parent = self._object.transform

	def update(self):
		pass
		#self._obj.AddComponent(unity.Text)

	def getObject(self):
		return self._object