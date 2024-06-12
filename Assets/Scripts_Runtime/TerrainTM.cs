using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dig {

    [CreateAssetMenu(fileName = "TM_Terrain", menuName = "Dig/TerrainTM")]
    public class TerrainTM : ScriptableObject {

        public int typeID;
        public Tile tile_wall;
        public Tile tile_ground;

    }

}