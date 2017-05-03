using System;
using CollaborationEngine.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Inspector
{
    public class TextInspectorPanel : MonoBehaviour
    {
        #region Unity Editor
        public InputField TextInputField;
        #endregion

        #region Properties
        public TextInstruction TextInstruction { get; set; }
        #endregion

        public void OnNameChanged()
        {
            TextInstruction.Text = TextInputField.text;
        }
    }
}
