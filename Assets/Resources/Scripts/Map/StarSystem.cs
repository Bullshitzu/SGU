﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSystem {

    // Instance vars

    private int _seed;
    public int Seed {
        get {
            return _seed;
        }
    }
    public Star Star { get; set; }

    public List<Planet> Planets;

    // Constructors

    public StarSystem () {

        //todo: check with gameLoadManager if this star system already exists (ie has been saved), if yes load seed

        this._seed = 634809511; 
        // Random.Range(int.MinValue, int.MaxValue); 
        // 634809511 
        Random.seed = _seed;

        Debug.Log("Star system seed: " + this._seed);

        this.Star = new Star();
        Planets = new List<Planet>();

        int planetCount = Random.Range(0, 12);

        float systemSize = Random.Range(5f, 15f);
        float internalDensityFactor = Random.Range(1.2f, 2.5f);

        List<float> planetDistances = new List<float>();
        int[] planetSeeds = new int[planetCount];

        for (int i = 0; i < planetCount; i++) {
            float randFactor = (i + 1f) / planetCount;
            float dist = Mathf.Pow(randFactor, internalDensityFactor);
            randFactor = dist * 0.3f;
            dist += Random.Range(-randFactor, randFactor);
            dist *= systemSize;
            planetDistances.Add(dist+0.05f);

            planetSeeds[i] = Random.Range(int.MinValue, int.MaxValue);
        }

        for (int i = 0; i < planetCount; i++) {
            Planets.Add(new Planet(this, planetDistances[i], planetSeeds[i]));
        }

        Random.seed = this.Seed;
    }
}