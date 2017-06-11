using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour {

    public string[] Prefix;
    public string[] Suffix;

    public string GenerateName()
    {
        var p = Prefix[Mathf.FloorToInt(Random.Range(0, Prefix.Length))];
        var s = Suffix[Mathf.FloorToInt(Random.Range(0, Suffix.Length))];

        return p + " Status " + s;
    }

}
