using UnityEngine;
using System.Collections;
using System.IO;

public class CountingReader : StreamReader {

    public CountingReader (string path)
        : base(path) {
        _currentLine = 0;
    }

    private int _currentLine;
    public int GetCurrentLine {
        get {
            return _currentLine;
        }
    }

    public override string ReadLine () {
        _currentLine++;
        return base.ReadLine();
    }

}
