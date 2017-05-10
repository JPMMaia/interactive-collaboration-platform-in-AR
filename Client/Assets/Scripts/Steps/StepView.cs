using System;
using CollaborationEngine.Base;
using UnityEngine.UI;

namespace CollaborationEngine.Steps
{
    public class StepView : Entity
    {
        #region Unity Editor
        public Text NameText;
        public InputField DescriptionInputField;
        #endregion

        #region Properties
        public uint StepID { get; set; }
        public uint StepOrder
        {
            get { return _stepOrder; }
            set
            {
                _stepOrder = value;
                UpdateName();
            }
        }
        public String StepDescription
        {
            get { return DescriptionInputField.text; }
            set { DescriptionInputField.text = value; }
        }
        #endregion

        #region Members
        private uint _stepOrder;
        #endregion

        private void UpdateName()
        {
            NameText.text = String.Format("Step {0}", StepOrder);
        }
    }
}
