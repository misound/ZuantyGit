using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    public static IItem reset(string Secne)
    {
        switch (Secne)
        {
            case "S1":
                return new S1Item();
            case "S2":
                return new S2Item();
            case "S3":
                return new S3Item();
            case "S4":
                return new S4Item();
            case "NS":
                return new NewSItem();
            case "SS":
                return new SSItem();
            default:
                break;
        }

        return null;
    }
}
