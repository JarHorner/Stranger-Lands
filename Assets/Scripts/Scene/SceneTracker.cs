using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTracker
{
    #region Variables

    //auto set for OverWorld when starting game for first time
    private static string lastSceneName = "OverWorld";

    #endregion

    #region Methods

    public static string LastSceneName
    {
        get { return lastSceneName; }
        set { lastSceneName = value; }
    }


    #endregion
}
