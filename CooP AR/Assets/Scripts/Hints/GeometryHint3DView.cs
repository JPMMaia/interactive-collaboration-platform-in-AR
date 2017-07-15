using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class GeometryHint3DView : Hint3DView
    {
        public GameObject Geometry
        {
            get
            {
                return _geometry;
            }
            set
            {
                if(_geometry)
                    Destroy(_geometry.gameObject);

                _geometry = value != null ? Instantiate(value, transform) : null;
            }
        }
        private GameObject _geometry;
    }
}
