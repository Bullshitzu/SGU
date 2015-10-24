using UnityEngine;
using System.Collections;

public class StarSystem {

    // Vars

    private int _seed;
    public int Seed {
        get {
            return _seed;
        }
    }
    public Star Star { get; set; }

    // todo: list of planets

    // Constructors

    public StarSystem () {

        //todo: check with gameLoadManager if this star system already exists (ie has been saved), if yes load seed

        this._seed = Random.Range(int.MinValue, int.MaxValue);
        Random.seed = _seed;

        this.Star = new Star();
    }

    // Methods
    

}