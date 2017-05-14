using System;
using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Cameras
{
    public class CameraItemView : Entity
    {
        #region Events
        public class ViewEventArgs : EventArgs
        {
            public CameraViewType Type { get; private set; }

            public ViewEventArgs(CameraViewType type)
            {
                Type = type;
            }
        }
        public delegate void ViewEventDelegate(CameraItemView sender, ViewEventArgs eventArgs);

        public event ViewEventDelegate OnClicked;
        #endregion

        #region Unity Editor
        public CameraViewType Type;
        public RawImage Icon;
        public Text Text;
        public Color SelectedColor;
        #endregion

        #region Properties
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;

                if (_selected)
                {
                    Icon.color = SelectedColor;
                    Text.color = SelectedColor;
                }
                else
                {
                    Icon.color = Color.white;
                    Text.color = new Color(0.20f, 0.20f, 0.20f, 1.0f);
                }
            }
        }
        #endregion

        #region Members
        private bool _selected;
        #endregion

        public void OnClick()
        {
            if (OnClicked != null)
                OnClicked(this, new ViewEventArgs(Type));
        }
    }
}
