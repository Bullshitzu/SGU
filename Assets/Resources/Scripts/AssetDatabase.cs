using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AssetDatabaseHelpers;
using Spaceship;

public class AssetDatabase {

    #region Paths
    const string pathShipTiles = "Assets/Game Data/Ship Tiles";
    #endregion

    #region Collections
    public static Dictionary<string, Spaceship.ShipTilePrefab> ShipTiles;
    #endregion

    public static void LoadAll () {

        #region Initialize Collections
        ShipTiles = new Dictionary<string, Spaceship.ShipTilePrefab>();
        #endregion

        #region Group Load Calls
        LoadShipTiles();
        #endregion

    }

    static bool LoadShipTiles () {
        if (!Directory.Exists(pathShipTiles)) return false;

        

        string[] dirs = Directory.GetDirectories(pathShipTiles);
        for (int i = 0; i < dirs.Length; i++) {
            string path = Helpers.CorrectPath(dirs[i]);

            FileParser.Parsers.ShipTileParser parser = new FileParser.Parsers.ShipTileParser(path);
            parser.StartParse();
            ShipTilePrefab tile = (ShipTilePrefab)parser.Generate();

            Debug.Log(tile);
        }

        return true;
    }


}
