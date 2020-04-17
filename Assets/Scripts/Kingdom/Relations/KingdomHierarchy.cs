using UnityEngine;
using UnityEditor;

public enum KingdomHierarchy
{
    Monarch, //King lives in capital, in castle, very rich
    Noble, //Lives in City, in castle, very rich
    LordLady, //Lives in hold, in towns, quite rich
    Mayor, //Lives in village hall, medium rich
    Citizen, //Lives in normal house, normal
    Peasant //Very poor, simple jobs i.e farmer
}
