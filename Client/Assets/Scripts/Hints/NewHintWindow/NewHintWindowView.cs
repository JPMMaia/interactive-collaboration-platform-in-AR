using System;
using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class NewHintWindowView : Entity
    {
        #region Unity Editor
        public InputField NameInputField;
        public Toggle NameTypeToggle;
        public Toggle ImageTypeToggle;
        public RectTransform FormTransform;
        public GameObject ImagesContainerPrefab;
        public ImageHintButtonView ImageButtonPrefab;
        #endregion

        #region Properties
        public String Name
        {
            get { return NameInputField.text; }
        }
        public HintType HintType
        {
            get
            {
                return NameTypeToggle.isOn ? HintType.Text : HintType.Image;
            }
        }
        private ImageHintButtonView SelectedImageHintButton
        {
            set
            {
                if (_selectedImageHintButton != null)
                    _selectedImageHintButton.Selected = false;

                _selectedImageHintButton = value;

                if(_selectedImageHintButton != null)
                    _selectedImageHintButton.Selected = true;
            }
        }
        public ImageHintType SelectedImageHintType
        {
            get
            {
                return _selectedImageHintButton == null ? ImageHintType.Null : _selectedImageHintButton.ImageHintType;
            }
        }
        #endregion

        #region Members
        private GameObject _imagesContainer;
        private ImageHintButtonView _selectedImageHintButton;
        private float _originalHeight;
        #endregion

        public void Start()
        {
            NameInputField.ActivateInputField();
            _originalHeight = GetComponent<RectTransform>().rect.height;
        }

        public void OnImageTypeChanged()
        {
            if (ImageTypeToggle.isOn)
            {
                // Ignore, if images container is already spawned:
                if (_imagesContainer != null)
                    return;

                // Instantiate images container:
                _imagesContainer = Instantiate(ImagesContainerPrefab, FormTransform);

                // Populate images container:
                for (var imageType = 0; imageType < (int)ImageHintType.Count; ++imageType)
                {
                    var imageButton = Instantiate(ImageButtonPrefab, _imagesContainer.transform);
                    imageButton.ImageHintType = (ImageHintType)imageType;
                    imageButton.OnClicked += ImageButton_OnClicked;
                }

                // Select first by default:
                SelectedImageHintButton = _imagesContainer.transform.GetChild(0).gameObject.GetComponent<ImageHintButtonView>();

                ExpandPanel();
            }
            else
            {
                // Ignore, if images container doesn't exist:
                if (_imagesContainer == null)
                    return;

                // Destroy images container:
                Destroy(_imagesContainer.gameObject);
                _imagesContainer = null;

                // Set selected image hint to null:
                _selectedImageHintButton = null;

                CollapsePanel();
            }
        }

        private void ExpandPanel()
        {
            var rectTransform = GetComponent<RectTransform>();

            var cellHeight = _imagesContainer.GetComponent<GridLayoutGroup>().cellSize.y;

            uint rows = 0;
            if (_imagesContainer.transform.childCount % 5 > 0)
                ++rows;
            rows += (uint)_imagesContainer.transform.childCount / 5;

            // Increase size:
            var size = _originalHeight + rows * cellHeight + 40.0f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }
        private void CollapsePanel()
        {
            var rectTransform = GetComponent<RectTransform>();

            // Decrease size:
            var size = _originalHeight;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }

        private void ImageButton_OnClicked(ImageHintButtonView sender, ImageHintButtonView.ButtonEventArgs eventArgs)
        {
            SelectedImageHintButton = sender;
        }
    }
}
