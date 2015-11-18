using UnityEngine;
using System.Collections;

public partial class Utility {
    public class TextureHelper {

        public static Texture2D GreyscaleToNormal (Texture2D greyscale) {

            int width = greyscale.width;
            int height = greyscale.height;

            int lenght = width * height;

            Color[] pixelsNormal = new Color[lenght];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    float currValue = greyscale.GetPixel(x, y).grayscale;

                    float x1n = greyscale.GetPixel(x - 1, y).grayscale;
                    float x1 = greyscale.GetPixel(x + 1, y).grayscale;
                    float y1n = greyscale.GetPixel(x, y - 1).grayscale;
                    float y1 = greyscale.GetPixel(x, y + 1).grayscale;

                    float normalXValue = (currValue - x1n) + (x1 - currValue) * 10 + 0.5f;
                    float normalYValue = (currValue - y1n) + (y1 - currValue) * 10 + 0.5f;

                    pixelsNormal[x + y * width] = new Color(1, 1-normalYValue, 1, 1-normalXValue);
                }
            }

            Texture2D normalmap = new Texture2D(width, height);
            normalmap.SetPixels(pixelsNormal);
            normalmap.Apply();
            return normalmap;
        }
    }
}
