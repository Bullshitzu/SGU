using UnityEngine;
using System.Collections;

public class TEST : MonoBehaviour {

    void Start () {

        FileParser.Parsers.FloorParser temp = new FileParser.Parsers.FloorParser("Assets/Game Data/TestFloor");

        try {
            temp.StartParse();
            Floor tempFloor = (Floor)temp.Generate();
            Debug.Log(tempFloor);
        }
        catch (System.Exception e) {
            string[] data = FileParser.ParserLogger.CollapseExceptions(e);
            Debug.Log(data[0]);

            ErrorLogger.CreateErrorLog(data[0], e, FileParser.ParserLogger.TempData);

        }
        

    }

}
