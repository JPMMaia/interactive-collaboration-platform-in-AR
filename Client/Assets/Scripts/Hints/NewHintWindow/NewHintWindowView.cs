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
        public Toggle TextTypeToggle;
        public Toggle ImageTypeToggle;
        public Toggle GeometryTypeToggle;
        public RectTransform FormTransform;
        public GameObject ImagesContainerPrefab;
        public ImageHintButtonView ImageButtonPrefab;
        #endregion

        #region Properties
        public String Name
        {
            get { return NameInputField.text; }
            set { NameInputField.text = value; }
        }
        public HintType HintType
        {
            get
            {
                if (TextTypeToggle.isOn)
                    return HintType.Text;

                if (ImageTypeToggle.isOn)
                    return HintType.Image;

                if (GeometryTypeToggle.isOn)
                    return HintType.Geometry;

                throw new NotSupportedException();
            }
            set
            {
                if (value == HintType.Text)
                    TextTypeToggle.isOn = true;

                if (value == HintType.Image)
                    ImageTypeToggle.isOn = true;

                if (value == HintType.Geometry)
                    GeometryTypeToggle.isOn = true;

                throw new NotSupportedException();
            }
        }
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
        public uint SelectedHintTypeID
        {
            get
            {
                return _selectedImageHintButton == null ? uint.MaxValue : _selectedImageHintButton.HintTypeID;
            }
            set
            {
                if (value == uint.MaxValue)
                {
                    _selectedImageHintButton = null;
                    return;
                }

                if (_selectedImageHintButton)
                    _selectedImageHintButton.HintTypeID = value;
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

        public void OnTogglesChanged()
        {
            if (HintType == HintType.Image || HintType == HintType.Geometry)
            {
                // Destroy, if images container is already spawned:
                if (_imagesContainer != null)
                    DestroyImagesContainer();

                // Instantiate images container:
                _imagesContainer = Instantiate(ImagesContainerPrefab, FormTransform);

                // Populate images container:
                InstantiateImageButtons(_imagesContainer);

                // Select first by default:
                SelectedImageHintButton = _imagesContainer.transform.GetChild(0).gameObject.GetComponent<ImageHintButtonView>();

                ExpandPanel();
            }
            else
            {
                DestroyImagesContainer();
            }
        }
        private void DestroyImagesContainer()
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

        private void InstantiateImageButtons(GameObject parent)
        {
            if (HintType == HintType.Image)
            {
                for (var imageType = 0; imageType < (int)ImageHintType.Count; ++imageType)
                {
                    var imageButton = Instantiate(ImageButtonPrefab, parent.transform);
                    imageButton.HintTypeID = (uint)imageType;
                    imageButton.Texture = Application.View.ImageHintTextures.GetTexture((ImageHintType) imageType);
                    imageButton.OnClicked += ImageButton_OnClicked;
                }
            }
            else if (HintType == HintType.Geometry)
            {
                foreach (var geometry in Application.View.GeometryModels.HintGeometries)
                {
                    var imageButton = Instantiate(ImageButtonPrefab, parent.transform);
                    imageButton.HintTypeID = geometry.ID;
                    imageButton.Texture = geometry.Icon;
                    imageButton.OnClicked += ImageButton_OnClicked;
                }
            }
            else
            {
                throw new NotSupportedException();
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
