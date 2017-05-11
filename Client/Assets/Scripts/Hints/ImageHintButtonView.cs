using System;
using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Hints
{
    public class ImageHintButtonView : Entity
    {
        #region Events
        public class ButtonEventArgs : EventArgs
        {
            public ImageHintType ImageHintType { get; private set; }

            public ButtonEventArgs(ImageHintType imageHintType)
            {
                ImageHintType = imageHintType;
            }
        }

        public delegate void ButtonEventDelegate(ImageHintButtonView sender, ButtonEventArgs eventArgs);

        public event ButtonEventDelegate OnClicked;
        #endregion

        public Color SelectedColor;
        public Button Button;
        public RawImage Icon;

        public ImageHintType ImageHintType
        {
            get { return _imageHintType; }
            set
            {
                _imageHintType = value;
                Icon.texture = Application.View.ImageHintTextures.GetTexture(value);
            }
        }
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;

                Button.targetGraphic.color = _selected ? SelectedColor : Color.white;
            }
        }

        private ImageHintType _imageHintType;
        private bool _selected;

        public void OnClick()
        {
            if(OnClicked != null)
                OnClicked(this, new ButtonEventArgs(_imageHintType));
        }
    }
}
