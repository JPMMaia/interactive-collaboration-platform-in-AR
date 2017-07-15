using System;
using System.Linq;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class Hint3DGeometryModels : Entity
    {
        [Serializable]
        public struct GeometryIDKeyValuePair
        {
            public uint ID;
            public GameObject Geometry;
            public Texture Icon;
        }

        public GeometryIDKeyValuePair[] HintGeometries;

        public GameObject GetGeometry(uint id)
        {
            return HintGeometries.First(pair => pair.ID == id).Geometry;
        }
        public Texture GetIcon(uint id)
        {
            return HintGeometries.First(pair => pair.ID == id).Icon;
        }
    }
}
