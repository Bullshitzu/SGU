using UnityEngine;
using System.Collections;

public class SceneMessanger {

    public enum Action {
        New,
        Load
    }

    public static Action action;

    static SceneMessanger () {
        action = Action.New;
    }

}
