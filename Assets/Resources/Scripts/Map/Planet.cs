using UnityEngine;
using System.Collections;

public class Planet {

    public PlanetArchetypes.Archetype Blueprint;

    public float Distance { get; set; }
    public Vector2 TemperatureRange;
    public float Mass;
    public float AtmosphereDensity;
    public float DayLength;

    public Planet (StarSystem parentSystem, float starDistance) { // distance in AU (0.2 to 40)

        this.Distance = starDistance;

        this.Blueprint = GetPlanetBlueprint(parentSystem);
        GeneratePlanetProperties(this.Blueprint);
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
    private void GeneratePlanetResources (PlanetArchetypes.Archetype blueprint) {
        // todo: generate resources based on blueprint
    }

    public override string ToString () {
        return "Type: " + Blueprint.Name + 
            ", Distance: " + Distance + 
            ", Mass: " + Mass + 
            ", Temperature: " + TemperatureRange + 
            ", Atmo. Pressure: " + AtmosphereDensity + 
            ", Day Length: " + DayLength;
    }
}
