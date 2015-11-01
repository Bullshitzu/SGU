using UnityEngine;
using System.Collections;

public class Planet {

    /* Properties
     * 
     * Mass (Gravity)
     * Temperature Range (Min/Max)
     * Atmosphere (Pressure, Composition)
     * Day Length
     * Geological Activity
     * 
     * Life
     * Valuables (ores/gases..)
     * Signals
    */

    readonly int[] fractals = new int[] { 3, 6, 9, 12, 15 };
    const int basicTextureXSize = 250;
    const int basicTextureYSize = 250;

    /* Calculations order
     * 
     * Distance (based on star heat)
     * Day Length (random)
     * Mass (partially based on distance)
     * Geo activity (random)
     * Atmosphere (based on geo activity, mass, day length, distance)
     * Temperature (based on distance, day length, geo activity, atmosphere)
     * Valuables (ore based on geo activity)
     * Life (amount random, type depends on all above)
     * Signals (random)
     * 
    */

    public float Distance; // AU
    public float DayLength; // Minutes
    public float Mass; // Earths
    public float GeoActivity; // Arbitrary, 0 to 1
    public float AtmoDensity; // Bars
    // todo: atmo composition?
    public float tempMin;
    public float TempMax;

    public Texture2D surfaceTexture;

    public Planet (StarSystem parentSystem, float starDistance) { // distance in AU (0.2 to 40)

        this.Distance = starDistance;

        SetDayLength();
        SetMass();
        SetGeoActivity();
        SetAtmoDensity();
        SetTemperature(parentSystem);
        
        // todo: resources and stuff


        
        SetSurfaceTexture();
        
        Debug.Log(this);
    }

    #region Basic Stat Generation
    void SetGeoActivity () {
        this.GeoActivity = Random.Range(0, 1f);
    }
    void SetDayLength () {
        float randDayFactor = Random.Range(-1f, 1f);
        if (randDayFactor < 0) randDayFactor = Mathf.Pow(Mathf.Abs(randDayFactor), 4f) * -25f;
        else randDayFactor = Mathf.Pow(randDayFactor, 4f) * 90f;
        this.DayLength = 30 + randDayFactor;
    }
    void SetMass () {
        this.Mass = 1 + Random.Range(-0.6f, 1f);
    }
    void SetAtmoDensity () {
        this.AtmoDensity = Mathf.Clamp(this.GeoActivity, 0.3f, 1) * Mathf.Pow(this.Mass, 2f) * Mathf.Clamp01(this.Distance);
        this.AtmoDensity = this.AtmoDensity * (1 - ((this.DayLength / 60) * (0.5f - Mathf.Clamp(this.Distance, 0, 0.5f))));
        this.AtmoDensity = Mathf.Pow(this.AtmoDensity, 3f);
    }
    void SetTemperature (StarSystem parentSystem) {
        float goldilocksFactor = parentSystem.Star.HabitableZoneCenter;
        if (goldilocksFactor < 0) goldilocksFactor = 0.1f;

        goldilocksFactor = (this.Distance) / goldilocksFactor;
        goldilocksFactor = 1 - Mathf.Clamp01(Mathf.Log(goldilocksFactor + 1));
        goldilocksFactor *= 10;

        float averageTemp = (goldilocksFactor + GeoActivity);
        averageTemp = AtmoDensity * averageTemp;

        averageTemp = Mathf.Log(averageTemp + 1) * 10;

        float tempDayFactor = (this.DayLength / 160) * (Mathf.Clamp01(1.5f - Mathf.Log(this.AtmoDensity + 1)));
        float maxTempFactor = (Mathf.Clamp(2 - averageTemp, 0.1f, 2)) * tempDayFactor * Mathf.Clamp((goldilocksFactor), 0.1f, 10f);

        TempMax = averageTemp + maxTempFactor; //averageTemp + (averageTemp - tempMin);
        tempMin = averageTemp * (averageTemp / TempMax); //averageTemp - (averageTemp * tempRangeFactor);

        tempMin *= 273;
        TempMax *= 273;

        tempMin = Mathf.Pow(tempMin, 0.9f);
        TempMax = Mathf.Pow(TempMax, 0.9f);
    }

    void SetSurfaceTexture () {
        surfaceTexture = new Texture2D(basicTextureXSize, basicTextureYSize);
        Color[] pixels = new Color[basicTextureXSize * basicTextureYSize];
        float offset = Random.Range(-10f, 10f);
        float[] heights = new float[basicTextureXSize * basicTextureYSize];
        float terrainRoughness = Random.Range(0.25f, 0.75f);

        for (int i = 0; i < fractals.Length; i++) {
            heights = RunBasicTerrainFractal(heights, new Vector2(basicTextureXSize, basicTextureYSize), fractals[i], offset);
        }

        // todo: generate craters on heightmap

        for (int y = 0; y < basicTextureYSize; y++) {
            for (int x = 0; x < basicTextureXSize; x++) {
                float val = 0.5f + heights[x + y * basicTextureXSize] * terrainRoughness;
                val = Mathf.Pow(val, 0.8f);
                pixels[x + y * basicTextureXSize] = new Color(val, val, val, 1);
            }
        }

        PaintSurfaceTexture(pixels);

        surfaceTexture.SetPixels(pixels);
        surfaceTexture.Apply();
    }
    void PaintSurfaceTexture (Color[] pixels) {



    }
    #endregion

    float[] RunBasicTerrainFractal (float[] data, Vector2 size, float scale, float offset) {

        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                data[x + y * (int)size.x] += (SimplexNoise.SeamlessNoise(x / size.x, y / size.y, scale, scale, offset)) / (scale*0.5f);
            }
        }

        return data;
    }

    public override string ToString () {
        return "Distance: " + this.Distance + ", Day Length: " + this.DayLength + ", Mass: " + this.Mass + ", Geological Activity: " + this.GeoActivity + ", Atmosphere Pressure: " + this.AtmoDensity + ", Temperature: " + tempMin + "K - " + TempMax + "K";
    }

}
