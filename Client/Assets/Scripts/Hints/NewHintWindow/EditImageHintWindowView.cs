using CollaborationEngine.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CollaborationEngine.Hints.NewHintWindow
{
    public class EditImageHintWindowView : Entity
    {
        #region Unity Editor
        public RectTransform FormTransform;
        public GameObject ImagesContainerPrefab;
        public ImageHintButtonView ImageButtonPrefab;
        #endregion

        #region Properties
        private ImageHintButtonView SelectedImageHintButton
        {
            set
            {
                if (_selectedImageHintButton != null)
                    _selectedImageHintButton.Selected = false;

                _selectedImageHintButton = value;

                if (_selectedImageHintButton != null)
                    _selectedImageHintButton.Selected = true;
            }
        }
        public ImageHintType SelectedImageHintType
        {
            get
            {
                return _selectedImageHintButton == null ? ImageHintType.Null : _selectedImageHintButton.ImageHintType;
            }
            set
            {
                _selectedImageHintType = value;

                if (value == ImageHintType.Null)
                {
                    _selectedImageHintButton = null;
                    return;
                }

                if(_selectedImageHintButton)
                    _selectedImageHintButton.ImageHintType = value;
            }
        }
        #endregion

        #region Members
        private GameObject _imagesContainer;
        private ImageHintButtonView _selectedImageHintButton;
        private ImageHintType _selectedImageHintType;
        private float _originalHeight;
        #endregion

        public void Start()
        {
            _originalHeight = GetComponent<RectTransform>().rect.height;

            AddImages();
        }

        private void AddImages()
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

                if (imageType == (int) _selectedImageHintType)
                    SelectedImageHintButton = imageButton;
            }

            ExpandPanel();
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

        private void ImageButton_OnClicked(ImageHintButtonView sender, ImageHintButtonView.ButtonEventArgs eventArgs)
        {
            SelectedImageHintButton = sender;
        }
    }
}
