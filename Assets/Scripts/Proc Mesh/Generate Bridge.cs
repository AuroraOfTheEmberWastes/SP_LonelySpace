using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handout
{
	public class GenerateBridge : MonoBehaviour
	{
		public int length = 10;
		// The dimensions of a single step of the staircase:
		public float width = 3;
		public float height = 1;
		public float depth = 1;

		MeshBuilder builder;

		void Start()
		{
			if (length % 2 == 0) length = length - 1;
			builder = new MeshBuilder();
			CreateShape();
			GetComponent<MeshFilter>().mesh = builder.CreateMesh(true);
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		void CreateShape()
		{
			builder.Clear();

			/**
			// V1: single step, hard coded:
			// bottom:
			int v1 = builder.AddVertex (new Vector3 (2, 0, 0), new Vector2 (1, 0));	
			int v2 = builder.AddVertex (new Vector3 (-2, 0, 0), new Vector2 (0, 0));
			// top front:
			int v3 = builder.AddVertex (new Vector3 (2, 1, 0), new Vector2 (1, 0.5f));	
			int v4 = builder.AddVertex (new Vector3 (-2, 1, 0), new Vector2 (0, 0.5f));
			// top back:
			int v5 = builder.AddVertex (new Vector3 (2, 1, 1), new Vector2 (0, 1));	
			int v6 = builder.AddVertex (new Vector3 (-2, 1, 1), new Vector2 (1, 1));

			builder.AddTriangle (v1, v2, v3);
			builder.AddTriangle (v2, v3, v4);
			builder.AddTriangle (v3, v4, v5);
			builder.AddTriangle (v4, v6, v5);

			/**/
			// V2, with for loop:
			for (int i = 0; i <= length; i++)
			{
				
				Vector3 offset = new Vector3(0, (Mathf.Sin(((float)i/length + 0.72f) * 1.29f) - 0.8f) * height * 5 - height/2, i * depth);
				float previousY = (Mathf.Sin((((float)i - 1) / length + 0.72f) * 1.29f) - 0.8f) * height * 5 - height/2;
				float nextY = (Mathf.Sin((((float)i + 1) / length + 0.72f) * 1.29f) - 0.8f) * height * 5 - height/2;

				float yBump = height - (offset.y - previousY);

				// bottom:
				int v1a = builder.AddVertex(offset + new Vector3(width, yBump, 0), new Vector2(1f, 0)); // front
				int v1b = builder.AddVertex(offset + new Vector3(width, yBump, 0), new Vector2(0, 0));  //left
				int v1c = builder.AddVertex(offset + new Vector3(width, yBump, 0), new Vector2(0f, 0)); // back
				int v2a = builder.AddVertex(offset + new Vector3(0, yBump, 0), new Vector2(0, 0));// front
				int v2b = builder.AddVertex(offset + new Vector3(0, yBump, 0), new Vector2(1, 0));//right
				int v2c = builder.AddVertex(offset + new Vector3(0, yBump, 0), new Vector2(1, 0)); // back
				// top front:
				int v3a = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0.5f));  // front
				int v3b = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0.5f));  //top
				int v3c = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(0, 0.5f));  //left
				int v4a = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(0, 0.5f));//front
				int v4b = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(0, 0.5f));  //top
				int v4c = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(1, 0.5f));  //right
				// top back:
				int v5a = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, 1)); //top
				int v5b = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1f, 0.5f)); //left
				int v5c = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(0f, 0.5f)); // back
				int v6a = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(0, 1));//top
				int v6b = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(0f, 0.5f));//right
				int v6c = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(1f, 0.5f)); //back

				if (i > length/2)
				{
					yBump = height - (offset.y - nextY);
					// bottom:
					v1a = builder.AddVertex(offset + new Vector3(width, yBump, depth), new Vector2(1f, 0)); // front
					v1b = builder.AddVertex(offset + new Vector3(width, yBump, depth), new Vector2(1, 0));  //left
					v1c = builder.AddVertex(offset + new Vector3(width, yBump, depth), new Vector2(0f, 0)); // back
					v2a = builder.AddVertex(offset + new Vector3(0, yBump, depth), new Vector2(0, 0));// front
					v2b = builder.AddVertex(offset + new Vector3(0, yBump, depth), new Vector2(0, 0));//right
					v2c = builder.AddVertex(offset + new Vector3(0, yBump, depth), new Vector2(1, 0)); // back
					// top front:
					v3a = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0.5f));  // front
					v3b = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(1, 0.5f));  //top
					v3c = builder.AddVertex(offset + new Vector3(width, height, 0), new Vector2(0, 0.5f));  //left
					v4a = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(0, 0.5f));//front
					v4b = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(0, 0.5f));  //top
					v4c = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(1, 0.5f));  //right
					// top back:
					v5a = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1, 1)); //top
					v5b = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(1f, 0.5f)); //left
					v5c = builder.AddVertex(offset + new Vector3(width, height, depth), new Vector2(0f, 0.5f)); // back
					v6a = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(0, 1));//top
					v6b = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(0f, 0.5f));//right
					v6c = builder.AddVertex(offset + new Vector3(0, height, depth), new Vector2(1f, 0.5f)); //back

				}




					// TODO 2: Fix the winding order (everything clockwise):
					builder.AddTriangle(v1a, v2a, v3a); // front bottom
				builder.AddTriangle(v3a, v2a, v4a); // front top
				builder.AddTriangle(v3b, v4b, v5a); // top bottom
				builder.AddTriangle(v4b, v6a, v5a); // top top

				// TODO 3: make the mesh solid by adding left, right and back side.
				builder.AddTriangle(v1b, v3c, v5b); // left (descending)
				builder.AddTriangle(v4c, v2b, v6b); // right
				builder.AddTriangle(v2c, v1c, v5c); // back lower
				builder.AddTriangle(v2c, v5c, v6c); // back upper

				// TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals (solve it by creating more vertices at the same position)
			}
			/**/
		}

	}
}