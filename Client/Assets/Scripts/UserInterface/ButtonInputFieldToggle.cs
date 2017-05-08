using System;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UserInterface
{
    public class ButtonInputFieldToggle : MonoBehaviour
    {
        #region Unity Editor
        public Button Button;
        public Text ButtonOrderText;
        public Text ButtonText;
        public InputField InputField;
        public Text InputFieldOrderText;
        #endregion

        #region Properties
        public String OrderText
        {
            get { return _orderText; }
            set
            {
                _orderText = value;
                UpdateUI();
            }
        }
        public String Text
        {
            get { return _text; }
            set
            {
                if(value == null)
                    value = String.Empty;

                _text = value.ToUpper();
                UpdateUI();
            }
        }
        #endregion

        #region Members
        private String _orderText = String.Empty;
        private String _text = String.Empty;
        #endregion

        public void ActivateButton()
        {
            // Enable button and disable input field:
            Button.gameObject.SetActive(true);
            InputField.gameObject.SetActive(false);

            UpdateUI();
        }
        public void ActivateInputField()
        {
            // Enable input field and disable button:
            InputField.gameObject.SetActive(true);
            Button.gameObject.SetActive(false);

            UpdateUI();

            InputField.ActivateInputField();
        }

        private void UpdateUI()
        {
            if (Button.enabled)
            {
                ButtonOrderText.text = _orderText;
                ButtonText.text = _text;
            }

            if (InputField.enabled)
            {
                InputField.text = _text;
                InputFieldOrderText.text = _orderText;
            }
        }

        #region Unity UI Event Handlers
        public void OnInputFieldTextChanged()
        {
            Text = InputField.text;
            InputField.text = _text;
        }
        public void OnInputFieldEndEdit()
        {
            ActivateButton();

            // TODO do not activate button if text is empty:
            throw new NotImplementedException();
        }
        #endregion
    }
}
