class Capsule(PythonUnityPrimitive):
	def __init__(self, x, y, z, color):
		PythonUnityPrimitive.__init__(self,x, y, z, color, unity.PrimitiveType.Capsule)