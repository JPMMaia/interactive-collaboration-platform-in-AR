using CollaborationEngine.UI;
using UnityEngine;

namespace CollaborationEngine.Objects
{
    public class IndicationToolsObject : SceneObject
    {
        public IndicationToolsObject(IndicationObject indicationObject) : 
            base(ObjectLocator.Instance.IndicationToolsPrefab, SceneObjectType.IndicationTools)
        {
            _indicationObject = indicationObject;
            NetworkData.Position.z = 1.0f;
        }

        public override GameObject Instantiate(Transform parent)
        {
            var gameObject = base.Instantiate(parent);

            _toolsComponent = GetComponent<ToolsWindow>();
            _toolsComponent.CloseButton.OnPressedEvent += CloseButton_OnPressedEvent;
            _toolsComponent.DeleteButton.OnPressedEvent += DeleteButton_OnPressedEvent;
            _toolsComponent.MoveButton.OnMouseDownEvent += MoveButton_OnMouseDownEvent;
            _toolsComponent.MoveButton.OnDraggedEvent += MoveButton_OnDraggedEvent;

            return gameObject;
        }
        public override void Destroy()
        {
            if (_toolsComponent)
            {
                _toolsComponent.MoveButton.OnDraggedEvent -= MoveButton_OnDraggedEvent;
                _toolsComponent.MoveButton.OnMouseDownEvent -= MoveButton_OnMouseDownEvent;
                _toolsComponent.DeleteButton.OnPressedEvent -= DeleteButton_OnPressedEvent;
                _toolsComponent.CloseButton.OnPressedEvent -= CloseButton_OnPressedEvent;
                _toolsComponent = null;
            }

            base.Destroy();
        }

        public IndicationObject IndicationObject
        {
            get
            {
                return _indicationObject;
            }
        }

        private void CloseButton_OnPressedEvent(object sender, System.EventArgs args)
        {
            Destroy();
        }
        private void DeleteButton_OnPressedEvent(object sender, System.EventArgs args)
        {
            // Destroy object:
            _indicationObject.Destroy();
        }
        private void MoveButton_OnMouseDownEvent(object sender, System.EventArgs args)
        {
        }
        private void MoveButton_OnDraggedEvent(object sender, System.EventArgs args)
        {
            var camera = ObjectLocator.Instance.MainCamera;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var gameObject = _indicationObject.GameObject;

            var pointOnPlane = gameObject.transform.position + gameObject.transform.right;
            var normal = gameObject.transform.forward;
            var d = -Vector3.Dot(pointOnPlane, normal);

            var t = -(Vector3.Dot(ray.origin, normal) + d) / Vector3.Dot(ray.direction, normal);

            var hitPoint = ray.origin + t * ray.direction;

            gameObject.transform.position = hitPoint;
        }

        private IndicationObject _indicationObject;
        private ToolsWindow _toolsComponent;
    }
}
