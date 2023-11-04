#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(DungeonWallRuleTile))]
    [CanEditMultipleObjects]
    public class DungeonWallRuleTileEditor : RuleTileEditor
    {
        public Texture2D FloorIcon;
        public Texture2D WallIcon;
        public Texture2D DoorIcon;
        public Texture2D VoidIcon;

        public override void RuleOnGUI(Rect rect, Vector3Int position, int neighbor)
        {
            switch (neighbor)
            {
                case DungeonWallRuleTile.Neighbor.Floor:
                    GUI.DrawTexture(rect, FloorIcon);
                    return;
                case DungeonWallRuleTile.Neighbor.Wall:
                    GUI.DrawTexture(rect, WallIcon);
                    return;
                case DungeonWallRuleTile.Neighbor.Door:
                    GUI.DrawTexture(rect, DoorIcon);
                    return;
                case DungeonWallRuleTile.Neighbor.Null:
                    GUI.DrawTexture(rect, VoidIcon);
                    return;
            }

            base.RuleOnGUI(rect, position, neighbor);
        }
    }
}
#endif
