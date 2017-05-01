using System;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.UI.Hint
{
    public class HintText : MonoBehaviour
    {
        #region Unity Editor

        public GameObject Panel;
        public Text Text;

        #endregion

        public void SetText(String text)
        {
            Text.text = text;
            Panel.transform.SetAsLastSibling();
        }

        public void Enable(bool enable)
        {
            Panel.SetActive(enable);
        }
    }
}