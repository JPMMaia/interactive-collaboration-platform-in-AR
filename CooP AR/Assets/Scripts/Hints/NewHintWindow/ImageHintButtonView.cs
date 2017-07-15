using System;
using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class ImageHintButtonView : Entity
    {
        #region Events
        public class ButtonEventArgs : EventArgs
        {
            public uint HintTypeID { get; private set; }

            public ButtonEventArgs(uint hintTypeID)
            {
                HintTypeID = hintTypeID;
            }
        }

        public delegate void ButtonEventDelegate(ImageHintButtonView sender, ButtonEventArgs eventArgs);

        public event ButtonEventDelegate OnClicked;
        #endregion

        public Color SelectedColor;
        public Button Button;
        public RawImage Icon;

        public uint HintTypeID
        {
            get { return _hintTypeID; }
            set
            {
                _hintTypeID = value;
                
            }
        }
        public Texture Texture
        {
            get { return Icon.mainTexture; }
            set { Icon.texture = value; }
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

        private uint _hintTypeID;
        private bool _selected;

        public void OnClick()
        {
            if(OnClicked != null)
                OnClicked(this, new ButtonEventArgs(_hintTypeID));
        }
    }
}
