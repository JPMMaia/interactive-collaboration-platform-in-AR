using System;
using CollaborationEngine.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Instructions
{
    public class InstructionTypeItem : MonoBehaviour
    {
        #region Delegates
        public delegate void InstructionTypeItemDelegate(InstructionTypeItem sender, EventArgs eventArgs);
        #endregion

        #region Events
        public event InstructionTypeItemDelegate OnPressed;
        #endregion

        #region Unity UI
        public Text Name;
        public Image BackgroundImage;
        #endregion

        #region Properties
        public InstructionType Type { get; set; }
        #endregion

        public void Start()
        {
            Name.text = Type.ToString();
        }

        public void SetSelectedAppearance(bool enable)
        {
            BackgroundImage.color = enable ? Color.green : Color.white;
        }

        #region Unity UI Event Handlers
        public void OnClick()
        {
            if(OnPressed != null)
                OnPressed(this, EventArgs.Empty);
        }
        #endregion
    }
}
