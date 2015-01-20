# VR-Programming
A Unity based virtual reality programming environment using Python

Currently in development. The idea behind this is that an immersive environment where you can see the effects of the code you write on the world around you could be great for people learning about programming. Python is a great starting language that can be used for many different real things. By combining this virtual environment with the ability to write Python code, users can learn and experiment with the language in an interesting totally immersive environment.

example script in the current version:

    import UnityEngine as unity

    def createCube(x, color):
      cube = unity.GameObject.CreatePrimitive(unity.PrimitiveType.Cube)
      cube.transform.position = unity.Vector3(x , 4, 2)
      cube.renderer.material.color = color
    createCube(2, unity.Color.blue)

