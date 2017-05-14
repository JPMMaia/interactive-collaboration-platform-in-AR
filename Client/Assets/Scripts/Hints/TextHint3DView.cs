using System;
using CollaborationEngine.Base;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class TextHint3DView : Entity
    {
        public TextMesh TextComponent;

        public String Text
        {
            get { return TextComponent.text; }
            set { TextComponent.text = value; }
        }
    }
}
