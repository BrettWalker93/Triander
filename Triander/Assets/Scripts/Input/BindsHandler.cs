using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//

[CreateAssetMenu()]

public class BindsHandler : ScriptableObject
{

    //temporary bind retrieval functionality until settings file is implemented
    public string[] ActionsK;

    public string[] KeyCodesV;

    string[] ActionsTest = {"Forward", "Backward", "Left", "Right"};
    string[] KeyCodeTest = {"w", "s", "a", "d"};

    public Dictionary<string, string> InputBinds;

    void Start()
    {

        InputBinds = new Dictionary<string, string>();
        //dictionary population, to be replaced by file reading
        for (int i = 0; i < ActionsTest.Length; i++)
            InputBinds.Add(ActionsTest[i], KeyCodeTest[i]);
    }

    //replace array search loop with dictionary TryGetValue once population issue is fixed
    public string GetKeyCode(string action)
    {
        string reVal = null;
        int c = 0;
        for (int i = 0; i < ActionsK.Length; i++)
        {
            if (action == ActionsK[i])
                c = i;
        }
        reVal = KeyCodesV[c];
        return reVal;
    }

}
