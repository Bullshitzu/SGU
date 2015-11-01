using UnityEngine;
using System.Collections;

public class Star {

    public enum StarClass {
        O,
        B,
        A,
        F,
        G,
        K,
        M
    }

    [HideInInspector]
    public StarClass Class;
    [HideInInspector]
    public float SurfaceTemperature;
    [HideInInspector]
    public float SolarMass;
    [HideInInspector]
    public float SolarRadius;
    [HideInInspector]
    public float SolarLuminosity;
    [HideInInspector]
    public float HabitableZoneCenter;

    public Star () {

        float classChance = Random.Range(0f, 100f);

        if (classChance < 0.03f) Class = StarClass.O;
        else if (classChance < 0.13f) Class = StarClass.B;
        else if (classChance < 0.6f) Class = StarClass.A;
        else if (classChance < 3f) Class = StarClass.F;
        else if (classChance < 7.6f) Class = StarClass.G;
        else if (classChance < 12.1f) Class = StarClass.K;
        else Class = StarClass.M;

        switch (Class) {
            case StarClass.O:
                SurfaceTemperature = Random.Range(30000f, 50000f);
                SolarMass = Random.Range(16f, 50f);
                SolarRadius = Random.Range(6.6f, 15f);
                break;
            case StarClass.B:
                SurfaceTemperature = Random.Range(10000f, 30000f);
                SolarMass = Random.Range(2.1f, 16f);
                SolarRadius = Random.Range(1.8f, 6.6f);
                break;
            case StarClass.A:
                SurfaceTemperature = Random.Range(7500f, 10000f); // 7k - 2 AU habitable
                SolarMass = Random.Range(1.4f, 2.1f);
                SolarRadius = Random.Range(1.4f, 1.8f);
                break;
            case StarClass.F:
                SurfaceTemperature = Random.Range(6000f, 7500f); // 6k - 1.5 AU habitable
                SolarMass = Random.Range(1.04f, 1.4f);
                SolarRadius = Random.Range(1.15f, 1.4f);
                break;
            case StarClass.G:
                SurfaceTemperature = Random.Range(5200f, 6000f);
                SolarMass = Random.Range(0.8f, 1.04f);
                SolarRadius = Random.Range(0.96f, 1.15f);
                break;
            case StarClass.K:
                SurfaceTemperature = Random.Range(3700f, 5200f);
                SolarMass = Random.Range(0.45f, 0.8f);
                SolarRadius = Random.Range(0.7f, 0.96f);
                break;
            case StarClass.M:
                SurfaceTemperature = Random.Range(2400f, 3700f); // 3k - 0.2 AU habitable
                SolarMass = Random.Range(0.08f, 0.45f);
                SolarRadius = Random.Range(0.4f, 0.7f);
                break;
        }

        float a = Mathf.Clamp(20 - SolarMass, 5, 20) / 5f;
        SolarLuminosity = Mathf.Pow(SolarMass, a);

        HabitableZoneCenter = 0.016f * Mathf.Pow((SurfaceTemperature/1000f), 2) + 0.28f * (SurfaceTemperature/1000f) - 0.79f;


        Debug.Log(this.ToString());
    }

    public override string ToString () {
        return "Class: " + Class + ", Surface Temperature: " + SurfaceTemperature + ", Solar Mass: " + SolarMass + ", Solar Luminosity: " + SolarLuminosity + ", Solar Radius: " + SolarRadius + ", Goldilocks center: " + HabitableZoneCenter;
    }
}
