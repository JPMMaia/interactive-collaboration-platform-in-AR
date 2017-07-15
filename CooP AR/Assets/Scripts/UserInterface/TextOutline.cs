using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.Rendering;

namespace CollaborationEngine.UserInterface
{
    public class TextOutline : Entity
    {
        public float PixelSize = 1;
        public Color OutlineColor = Color.black;
        public bool ResolutionDependant = false;
        public int DoubleResolution = 1024;

        private TextMesh _textMesh;
        private MeshRenderer _meshRenderer;

        public void Start()
        {
            _textMesh = GetComponent<TextMesh>();
            _meshRenderer = GetComponent<MeshRenderer>();

            for (int i = 0; i < 8; i++)
            {
                GameObject outline = new GameObject("outline", typeof(TextMesh));
                outline.transform.SetParent(transform, false);
                outline.transform.localScale = new Vector3(1, 1, 1);

                MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
                otherMeshRenderer.material = new Material(_meshRenderer.material);
                otherMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                otherMeshRenderer.receiveShadows = false;
                otherMeshRenderer.sortingLayerID = _meshRenderer.sortingLayerID;
                otherMeshRenderer.sortingLayerName = _meshRenderer.sortingLayerName;
            }
        }

        public void LateUpdate()
        {
            var currentCamera = Camera.current;

            if (currentCamera == null)
                return;

            Vector3 screenPoint = currentCamera.WorldToScreenPoint(transform.position);

            OutlineColor.a = _textMesh.color.a * _textMesh.color.a;

            // copy attributes
            for (int i = 0; i < transform.childCount; i++)
            {

                TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
                other.color = OutlineColor;
                other.text = _textMesh.text;
                other.alignment = _textMesh.alignment;
                other.anchor = _textMesh.anchor;
                other.characterSize = _textMesh.characterSize;
                other.font = _textMesh.font;
                other.fontSize = _textMesh.fontSize;
                other.fontStyle = _textMesh.fontStyle;
                other.richText = _textMesh.richText;
                other.tabSize = _textMesh.tabSize;
                other.lineSpacing = _textMesh.lineSpacing;
                other.offsetZ = _textMesh.offsetZ;

                bool doublePixel = ResolutionDependant && (Screen.width > DoubleResolution || Screen.height > DoubleResolution);
                Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * PixelSize : PixelSize);
                Vector3 worldPoint = currentCamera.ScreenToWorldPoint(screenPoint + pixelOffset);
                other.transform.position = worldPoint;

                MeshRenderer otherMeshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
                otherMeshRenderer.sortingLayerID = _meshRenderer.sortingLayerID;
                otherMeshRenderer.sortingLayerName = _meshRenderer.sortingLayerName;
            }
        }

        private static Vector3 GetOffset(int i)
        {
            switch (i % 8)
            {
                case 0: return new Vector3(0, 1, 0);
                case 1: return new Vector3(1, 1, 0);
                case 2: return new Vector3(1, 0, 0);
                case 3: return new Vector3(1, -1, 0);
                case 4: return new Vector3(0, -1, 0);
                case 5: return new Vector3(-1, -1, 0);
                case 6: return new Vector3(-1, 0, 0);
                case 7: return new Vector3(-1, 1, 0);
                default: return Vector3.zero;
            }
        }
    }
}
