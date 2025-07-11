using UnityEngine;

public class TextureGeneration : MonoBehaviour
{
	public enum PatternType { Noise, None, Mandelbrot, LocationColor, CenterDot, CheckerBoard, Rainbow, Sin , Water};

	public PatternType patternType;

	public float scale = 1;

	const int SIZE = 1024;

	Texture2D texture = null;
	Color[] cols = null;

	void Start()
	{
		// Create a texture and pass it to the material of this game object's renderer:
		Renderer rend = GetComponent<Renderer>();
		texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
		rend.material.mainTexture = texture;
		texture.wrapMode = TextureWrapMode.Clamp;

		Draw();
	}

	/// <summary>
	/// Returns the pixel color for texture coordinate (u,v), for a given pattern. 
	/// </summary>
	Color CalculatePixelColor(float u, float v, PatternType pattern)
	{
		// TODO Exercise 1: insert your own pattern creation code here:


		switch (pattern)
		{
			case PatternType.Noise: // white noise				
				return Random.value * Color.white;
			case PatternType.Mandelbrot:
				return Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f));
			case PatternType.LocationColor:
				return new Color(u, 0f, v);
			case PatternType.CenterDot:
				u -= 0.5f; v -= 0.5f;
				if (Mathf.Sqrt(u*u + v*v) < 0.5f) return Color.red;
				else return Color.green;
			case PatternType.CheckerBoard:
				int a = (int)Mathf.Floor(u * 10) % 2;
				int b = (int)Mathf.Floor(v * 10) % 2;
				int r = a + b;
				if (r == 0 || r == 2) return Color.white;
				return Color.black;
			case PatternType.Sin:
				if ((Mathf.Sin((u+0.25f) * 2 * Mathf.PI)+1) / 2 > v) return Color.red;
				return Color.black;
			case PatternType.Rainbow:
				Color result = Color.black;
				result.r = (Mathf.Cos(u * 2 * Mathf.PI) + 1) / 2;
				result.g = (Mathf.Cos(u * 2 * Mathf.PI + Mathf.PI * 2 / 3) + 1) / 2;
				result.b = (Mathf.Cos(u * 2 * Mathf.PI + Mathf.PI * 4 / 3) + 1) / 2;
				return result;
			case PatternType.Water:
				result = Color.black;

				float noise = Mathf.PerlinNoise(u / scale, v / scale);
				if (noise > 0.45 && noise < 0.6) result += new Color(noise, noise, noise);
				else if (noise < 0.45) result += new Color(0, noise / 2, noise);
				else result += new Color(0, noise / 2, noise);

				noise = Mathf.PerlinNoise(noise, noise);
				if (noise > 0.45 && noise < 0.6) result += new Color(noise * 0.7f, noise * 0.7f, noise * 0.7f);
				else if (noise < 0.45) result += new Color(0, noise / 2, noise);
				else result += new Color(0, noise / 3, noise);


				return result;
			default:
				return Color.blue;
		}
	}



	/// <summary>
	/// Draws a pattern given by the [pattern] number to the [cols] array, which
	/// should have size [width] * [height].
	/// </summary>
	void DrawPattern(Color[] cols, int width, int height, PatternType pattern)
	{
		for (int index = 0; index < width * height; index++)
		{
			// TODO: calculate UV coordinates and pass them to CalculatePixelColor:
			float a = index % width;
			float b = index / height;
			float u = a / SIZE;
			float v = b / SIZE;

			cols[index] = CalculatePixelColor(u, v, pattern);
		}
	}


	void Draw()
	{
		if (cols == null)
		{
			cols = texture.GetPixels();
		}
		DrawPattern(cols, SIZE, SIZE, patternType);

		texture.SetPixels(cols);
		texture.Apply();
	}


	// OnValidate is called whenever an inspector value is changed - even in edit mode!
	void OnValidate()
	{
		// To prevent calling Draw code in edit mode,
		// we check whether a texture has been created (in Start)
		if (texture == null) return;
		Draw();
	}


	#region Mandelbrot
	// Used for the Mandelbrot fractal:
	const int maxIterations = 30;
	const float escapeLengthSquared = 4;

	Color Mandelbrot(float cReal, float cImaginary)
	{
		int iteration = 0;

		float zReal = 0;
		float zImaginary = 0;

		while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations)
		{
			// Use Mandelbrot's magic iteration formula: z := z^2 + c 
			// (using complex number multiplication & addition - 
			//   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
			float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
			zImaginary = 2 * zReal * zImaginary + cImaginary;
			zReal = newZr;
			iteration++;
		}
		// Return a color value based on the number of iterations that were needed to "escape the circle":
		float grad = 1f * iteration / maxIterations; // between 0 and 1
													 // TODO: use a nicer gradient
		return new Color(grad, grad, grad);
	}
	#endregion


}
