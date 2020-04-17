using UnityEngine;
using UnityEditor;

public class SettlementSection
{
    public Vec2i Point { get; private set; }
    public Vec2i[] Connected { get; private set; }
    public SettlementSection(Vec2i point, Vec2i[] connected) {
        Point = point;
        Connected = connected;
    }

}