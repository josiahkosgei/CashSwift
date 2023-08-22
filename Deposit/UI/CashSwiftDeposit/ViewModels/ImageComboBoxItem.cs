using System;
using CashSwiftUtil.Imaging;
using System.Windows.Media.Imaging;

namespace CashSwiftDeposit.ViewModels
{
    public class ImageComboBoxItem<T>
    {
        public BitmapImage Image { get; set; }

        public string ImageContent { get; set; }

        public string Label { get; set; }

        public T Value { get; set; }

        public ImageComboBoxItem(string imageContent, string selectionText, T value)
        {
            ImageContent = imageContent.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory).Replace("{ResourceDir}", "pack://application:,,,");
            Image = ImageManipuation.GetBitmapFromFile(ImageContent);
            Label = selectionText;
            Value = value;
        }

        public ImageComboBoxItem(BitmapImage image, string selectionText, T value)
        {
            Image = image;
            Label = selectionText;
            Value = value;
        }
    }
}
