using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetArchetypes {

    public enum PlanetType {
        BASE,
        Lava,
        Temperate,
        Barren,
        Ice
    }

    public static Archetype[] Generate () {
        List<Archetype> temp = new List<Archetype>();

        temp.Add(new Lava());
        temp.Add(new Temperate());
        temp.Add(new Barren());
        temp.Add(new Ice());

        return temp.ToArray();
    }

    public class Archetype {

        public Archetype () {
            Type = PlanetType.BASE;
            TemperatureRange = new Vector2(0, 0); // kelvin
            AtmospherePressureRange = new Vector2(0, 2.5f); // bars
            DayLengthRange = new Vector2(5, 90); // minutes
            MassRange = new Vector2(0.2f, 2.5f);
            ColorPalette = new Color[] {
                new Color(0,0,0,1),
                new Color(1,1,1,1)
            };
        }
        public PlanetType Type;
        public virtual float GetOccuranceChanceAtDistance (float distance) { return 0; }
        public virtual void SetNoiseGenerator (LibNoise.RidgedMultifractal Generator) {
            Generator.Seed = Random.Range(int.MinValue, int.MaxValue);
            Generator.OctaveCount = 5;
            Generator.NoiseQuality = LibNoise.NoiseQuality.High;
            Generator.Lacunarity = 1.4f;
            Generator.Frequency = 6f;
        }
        public Vector2 TemperatureRange;
        public Vector2 AtmospherePressureRange;
        public Vector2 DayLengthRange;
        public Vector2 MassRange;
        public Color[] ColorPalette;

        // amount of resources range (each individually)

    }

    public class Lava : Archetype {
        public Lava ()
            : base() {
            Type = PlanetType.Lava;
            TemperatureRange = new Vector2(1100, 1500);
            AtmospherePressureRange = new Vector2(0.8f, 4f);
            ColorPalette = new Color[] {
                (Color) new Color32(15, 12, 7, 255),
                (Color) new Color32(100, 80, 70, 255)
            };
        }
        public override float GetOccuranceChanceAtDistance (float distance) {
            return Mathf.Clamp01(-4 * distance * distance + 1);
            // 0 = 1
            // 0.5 = 0
        }
        public override void SetNoiseGenerator (LibNoise.RidgedMultifractal Generator) {
            base.SetNoiseGenerator(Generator);
            Generator.Lacunarity = 2f;
        }
    }
    public class Temperate : Archetype {
        public Temperate ()
            : base() {
            Type = PlanetType.Temperate;
            TemperatureRange = new Vector2(250, 330);
        }
        public override float GetOccuranceChanceAtDistance (float distance) {
            return Mathf.Clamp01(-4 * distance * distance + 8 * distance - 3);
            // 0.5 = 0
            // 1 = 1
            // 1.5 = 0
        }
    }
    public class Barren : Archetype {
        public Barren ()
            : base() {
            Type = PlanetType.Barren;
            TemperatureRange = new Vector2(150, 300);
        }
        public override float GetOccuranceChanceAtDistance (float distance) {
            return Mathf.Clamp01(-0.32f * distance * distance + 1.469f * distance - 0.65f);
            // 0.5 = 0
            // 2.25 = 1
            // 4 = 0
        }
    }
    public class Ice : Archetype {
        public Ice ()
            : base() {
            Type = PlanetType.Ice;
            TemperatureRange = new Vector2(0, 50);
            AtmospherePressureRange = new Vector2(0, 0.5f);
        }

        public override float GetOccuranceChanceAtDistance (float distance) {
            return Mathf.Clamp01((distance - 3.5f) / 3f);
            // 3.5 = 0
            // 6.5 = 1
        }
    }
}
