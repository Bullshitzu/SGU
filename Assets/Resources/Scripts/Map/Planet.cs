using UnityEngine;
using System.Collections;

public class Planet {

    public PlanetArchetypes.Archetype Blueprint;
    public int Seed;

    #region Basic Properties
    public float Distance { get; set; }
    public Vector2 TemperatureRange;
    public float Mass;
    public float AtmosphereDensity;
    public float DayLength;
    #endregion

    #region Surface Maps
    public const int textureSize = 128;

    public Texture2D Heightmap;
    public Texture2D textureDiffuse;
    public Texture2D textureIllumination;
    public Texture2D textureNormal;
    #endregion

    public Planet (StarSystem parentSystem, float starDistance, int seed) { // distance in AU (0.2 to 40)

        this.Distance = starDistance;

        this.Seed = seed;
        Random.seed = seed;

        this.Blueprint = GetPlanetBlueprint(parentSystem);
        
        GeneratePlanetProperties(this.Blueprint);
        Heightmap = GeneratePlanetHeightmap(this.Blueprint);

        this.textureDiffuse = GeneratePlanetDiffuseMap(this.Blueprint, this.Heightmap);
        this.textureIllumination = GeneratePlanetIlluminationMap(this.Blueprint, this.Heightmap);
        this.textureNormal = Utility.TextureHelper.GreyscaleToNormal(Heightmap);

        // add water to diffuse/specular

        GeneratePlanetResources(this.Blueprint);

        Debug.Log(this);
    }

    private PlanetArchetypes.Archetype GetPlanetBlueprint (StarSystem parentSystem) {
        
        PlanetArchetypes.Archetype[] archetypes = PlanetArchetypes.Generate();
        float goldilocksDistance = Distance / parentSystem.Star.HabitableZoneCenter;

        float[] chances = new float[archetypes.Length];

        float sum = 0;
        for (int i = 0; i < archetypes.Length; i++) {
            float temp = archetypes[i].GetOccuranceChanceAtDistance(goldilocksDistance);
            chances[i] = temp + sum;
            sum += temp;
        }

        float pick = Random.Range(0, sum);
        for (int i = 0; i < archetypes.Length; i++) {
            if (pick < chances[i] || i+1==archetypes.Length) {
                return archetypes[i];
            }
        }

        return archetypes[0];
    }

    private void GeneratePlanetProperties (PlanetArchetypes.Archetype blueprint) {
        this.Mass = Random.Range(blueprint.MassRange.x, blueprint.MassRange.y);
        this.TemperatureRange = new Vector2(
            Random.Range(blueprint.TemperatureRange.x, blueprint.TemperatureRange.y),
            Random.Range(blueprint.TemperatureRange.x, blueprint.TemperatureRange.y));
        if (this.TemperatureRange.x > this.TemperatureRange.y) 
            this.TemperatureRange = new Vector2(this.TemperatureRange.y, this.TemperatureRange.x);
        this.DayLength = Random.Range(blueprint.DayLengthRange.x, blueprint.DayLengthRange.y);
        this.AtmosphereDensity = Random.Range(blueprint.AtmospherePressureRange.x, blueprint.AtmospherePressureRange.y);
    }
    private Texture2D GeneratePlanetHeightmap (PlanetArchetypes.Archetype blueprint) {

        /*
         * SeamlessNoise( float x, float y, float dx, float dy, float xyOffset );
         * where: x, y are normalized coordinates (in [0..1] space).
         * dx, dy are noise scale in x and y axes.
         * xyOffset is noise offset (same offset will result in having the same noise).
         * The method returns a float value in [-1..1] space.
         */

        float[] height = GeneratePlanetHeightmapData(blueprint);

        // todo: add craters depending on atmosphere thickness and mass

        Color[] image = new Color[textureSize * textureSize];

        for (int x = 0; x < textureSize; x++) {
            for (int y = 0; y < textureSize; y++) {
                float currValue = 1 - height[x + y * textureSize];
                image[x + y * textureSize] = new Color(currValue, currValue, currValue, 1);
            }
        }

        Texture2D temp = new Texture2D(textureSize, textureSize);
        temp.SetPixels(image);
        temp.Apply();
        return temp;
    }
    private float[] GeneratePlanetHeightmapData (PlanetArchetypes.Archetype blueprint) {

        float[] height = new float[textureSize * textureSize];

        LibNoise.RidgedMultifractal Generator = new LibNoise.RidgedMultifractal();
        blueprint.SetNoiseGenerator(Generator);
        
        float correctMin = float.MaxValue;
        float correctMax = float.MinValue;

        float currValue;

        for (int y = 0; y < textureSize; y++) {
            for (int x = 0; x < textureSize; x++) {
                currValue = (float)Generator.GetValue((float)x / textureSize, (float)y / textureSize, 1);
                if (currValue < correctMin) correctMin = currValue;
                if (currValue > correctMax) correctMax = currValue;
                height[x + y * textureSize] = currValue;
            }
        }

        float oldValue;

        for (int y = 0; y < textureSize; y++) {
            for (int x = 0; x < textureSize; x++) {

                float mult = (float)x / textureSize;

                oldValue = height[x + y * textureSize];
                currValue = (float)Generator.GetValue(((float)x / textureSize) + 1, (float)y / textureSize, 1);

                float yMult = (float)y / textureSize;
                yMult = Mathf.Clamp01(11.11f * Mathf.Pow(yMult, 2) - 11.11f * yMult + 1);

                height[x + y * textureSize] = Mathf.Lerp(currValue, oldValue, mult);
                height[x + y * textureSize] = Mathf.Lerp(height[x + y * textureSize], 0, yMult);
            }
        }

        for (int y = 0; y < textureSize; y++) {
            for (int x = 0; x < textureSize; x++) {
                height[x + y * textureSize] = (height[x + y * textureSize] - correctMin) / (correctMax - correctMin);
            }
        }

        return height;
    }
    private Texture2D GeneratePlanetDiffuseMap (PlanetArchetypes.Archetype blueprint, Texture2D heightmap) {
        //todo: this

        Color[] palette = blueprint.ColorPalette;

        float v;
        Color[] pixelsHeightmap = heightmap.GetPixels();
        Color[] pixels = new Color[textureSize * textureSize];

        for (int y = 0; y < textureSize; y++) {
            for (int x = 0; x < textureSize; x++) {
                v = pixelsHeightmap[x + y * textureSize].grayscale;
                pixels[x + y * textureSize] = Color.Lerp(palette[0], palette[1], v);
            }
        }

        Texture2D temp = new Texture2D(textureSize, textureSize);
        temp.SetPixels(pixels);
        temp.filterMode = FilterMode.Point;
        temp.wrapMode = TextureWrapMode.Clamp;
        temp.Apply();

        return temp;
    }
    private Texture2D GeneratePlanetIlluminationMap (PlanetArchetypes.Archetype blueprint, Texture2D heightmap) {

        switch (blueprint.Type) {
            case PlanetArchetypes.PlanetType.Lava:
                // todo: add other types that have lava (inferno/toxic/protoplanet...)

                Color[] pixelsHeightmap = heightmap.GetPixels();
                Color[] pixels = new Color[textureSize * textureSize];

                for (int x = 0; x < textureSize; x++) {
                    for (int y = 0; y < textureSize; y++) {

                        float v = pixelsHeightmap[x + y * textureSize].grayscale;
                        v = Mathf.Clamp01((1 - v - 0.5f) * 2);

                        pixels[x + y * textureSize] = new Color(v * 4, v, 0, v);
                    }
                }

                Texture2D temp = new Texture2D(textureSize, textureSize);
                temp.SetPixels(pixels);
                temp.filterMode = FilterMode.Point;
                temp.wrapMode = TextureWrapMode.Clamp;
                temp.Apply();

                return temp;
            default: 
                return null;
        }
    }
    private void GeneratePlanetResources (PlanetArchetypes.Archetype blueprint) {
        // todo: generate resource maps based on blueprint
    }

    public override string ToString () {
        return "Type: " + Blueprint.Type + 
            ", Distance: " + Distance + 
            ", Mass: " + Mass + 
            ", Temperature: " + TemperatureRange + 
            ", Atmo. Pressure: " + AtmosphereDensity + 
            ", Day Length: " + DayLength;
    }
}
