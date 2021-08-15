# Unity Geometry Grass Shader
A Geometry shader written for Unity's build-in Render Pipeline

![Geometry Grass Shader](https://i.imgur.com/qHsTCqm.png)

## What was the Problem?
There are many games that render tons of grass without the GPU breaking as much as a sweat (Genshin Impact, Breath of the Wild, etc.). While researching the topic, I came across the solutions for rendering large amounts of objects without straining the CPU by  off-loading most / all of the work to the GPU. One such solution was to use Unity's [`Graphics.DrawMeshInstanced`](https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstanced.html) and [`Graphics.DrawMeshInstancedIndirect`](https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstancedIndirect.html), which renders provided `Meshes` with a `Material` that supports GPU Instancing. The `Shader` on the `Material` would be used as a way to render the `Mesh` on the right position, by getting location data through a `ComputeBuffer`.

Using this method, [Colin Leung](https://github.com/ColinLeung-NiloCat) created a great [example](https://github.com/ColinLeung-NiloCat/UnityURP-MobileDrawMeshInstancedIndirectExample) of how to combine the `Graphics.DrawMeshInstancedIndirect` with a generated grass `Mesh` and a complicated `Material` to show a large field of grass (10 million instances) with great performance, even supporting bending of the grass when an object runs through the generated grass.

This would fix the problem of generating a great amount of grass while utilizing GPU Instancing, however this solution would not work when taking into account the height of the terrain. In the example the grass is generated at a certain height, which is not variable. You could add support for a height map to tell the grass `Material` at which height it should be offset, but this would not work when there are two platforms above each other, which both need grass rendered on them (technically it could work, though it would require quite a workaround or several GPU Instancers to make it work).

## What was the Solution?
Geometry shaders were introduced in Direct3D 10 and OpgenGL 3.2, which is a type of shader that can generate points, lines and triangles and are generated after the vertex shader and before the vertices are processed for the fragment shader. The geometry shader can use the vertex as input, so it contains the world position of said vertex, removing the need to create an offset that was needed with GPU Instancing.

The tutorial from [Erik Roystan Ross](https://roystan.net/) is amazing at explaining exactly how to utilize the a geometry shader to generate grass on a model in his tutorial ["Grass Shader"](https://roystan.net/articles/grass-shader.html). In the tutorial they also combine the vertex shader with a custom tessellation script, which was adapted from the [Catlike Coding](https://catlikecoding.com/unity/tutorials/advanced-rendering/tessellation/) article on Advanced Rendering. Both of these articles were an extremely interesting read. I used Erik Roystan Ross' shader code to generate the grass in a geometry shader and extended it to support the features that I wanted to have in my shader.

## What does the shader feature?
The shader features everything contained in Erik Roystan Ross' tutorial on how to write a grass geometry shader and adds extra features on top of it to generate grass similar to previously said games.

**Roystan's Features**
* Generate Grass using a Geometry Shader
* Utilize Tessellation to increase Grass Density
* Support for Shadows from Directional Light
* Use a Texture to offset the Grass Blades (Wind)

**Added Features**
* Color the Grass using a Ground Texture (this uses the original Model's UV)
* Influence the height of the Grass using a Mask
    * This also works as a mask to hide grass from certain parts of your model

## How to use the shader?
You can clone the project and use the `CustomTessellation.cginc` and `GeometryGrassShader.shader`, which is the barebone to create use the geometry shaders.

The repository is an example of how to use the shader in combination with all the properties provided. In the scene `Scenes/SampleScene` there's the example shown in the screenshot on the top of this Readme.

![Properties](https://i.imgur.com/qvWDxCs.png)

## Future Features?
If I've got time I would love to update the shader to support more features. Though this would require time, since I'm completely new to writing shaders, let alone geometry shaders. I couldn't write this shader without the amazing tutorials and information that is already written for me (like the articles from Catlike Coding and Roystan).

* Full Light Support
    * Multiple Directional Lights
    * Point- and Spotlights
* Local Wind Masks
* Grass Displacement based on Objects
