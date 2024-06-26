# rotating-3d-cube
A 3d rotating cube  in a 2d flat surface (console) using vector math and 3d rendering with matrices

<h2 align="center">First version release</h2>
<p align="center">
	<a href="https://lh3.googleusercontent.com/drive-viewer/AKGpihZTrl_0jkSHhoBQkY56HgXMy1ckPpGdYVeRf-PT9SoWsYYOwvbw5obE9Y4C4xa8EQIRDMfDEprPN3sXOErU2AnXR_Dg9EDtqO4=s2560?source=screenshot.guru"> <img src="https://lh3.googleusercontent.com/drive-viewer/AKGpihZTrl_0jkSHhoBQkY56HgXMy1ckPpGdYVeRf-PT9SoWsYYOwvbw5obE9Y4C4xa8EQIRDMfDEprPN3sXOErU2AnXR_Dg9EDtqO4=s2560"
		style="
			height: 200px;
			width:  200px;
		"
	/>
	</a>
</p>

- This virtual representation of a cube is created using 6 triangles with their edges conected in order to create the allusion of a cube's faces, only the triangle's vertices are setted, the remaining parts of the image are computated, the rendering process is composed of an infinite loop starting with defining verticies, gathering all existent vectors whitin the polygon using rasterization, printing each vector based on its x, y and z values, then transforming the vectors. The Z axis determinates the character light index in the scale: { .,~+:;%#$@ }
- The filling process of the triangle's faces (which complete the cube's) is called [triangle rasterization](https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html), the deformation is caused because the default windows terminal console's characters have a higher height size per char than width resulting in this stretching effect.
- In the other hand, rotation is calculated using 4x4 matrices that transform the triangle's vertice's vectors into a new angular position, in the end, the triangles predefined and tranformed vectors give the other coordinates within the rendering area
