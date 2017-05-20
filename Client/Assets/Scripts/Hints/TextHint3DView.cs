using System;
using UnityEngine;

namespace CollaborationEngine.Hints
{
    public class TextHint3DView : Hint3DView
    {
        public TextMesh TextComponent;

        public String Text
        {
            get { return TextComponent.text; }
            set { TextComponent.text = value; }
        }
    }
}
