class Tree():
	def __init__(self, x, y, z, depth, scale):
		self._object = unity.GameObject("Tree")
		if depth > 6:
			depth = 6
		self.draw(x, y, z, 90.0, depth, scale)

	def draw(self, x1, y1, z1, angle, depth, scale):
		if depth != 0:
			x2 = x1 + int(math.cos(math.radians(angle)) * depth * scale)
			y2 = y1 + int(math.sin(math.radians(angle)) * depth * scale)
			z2 = z1 + int(math.cos(math.radians(angle)) * depth * scale)
			self.drawBranch(x1, y1, z1, x2, y2, z2, red)
			self.draw(x2, y2, z2, angle - 20, depth - 1, scale)
			self.draw(x2, y2, z2, angle + 20, depth - 1, scale)

	def drawBranch(self, x1, y1, z1, x2, y2, z2, color):
		branch = unity.GameObject("Branch")
		branch.transform.parent = self._object.transform
		line = branch.AddComponent(unity.LineRenderer)
		line.SetPosition(0, unity.Vector3(x1, y1, z1))
		line.SetPosition(1, unity.Vector3(x2, y2, z2))
		line.SetWidth(.25, .25)
		
	def update(self):
		pass