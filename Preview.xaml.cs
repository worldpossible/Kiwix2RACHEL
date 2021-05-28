using System.Windows;

namespace Kiwix2RACHEL
{
    public partial class Preview : Window
    {
        // Preview the contents of the provided template string
        public Preview(string template)
        {
            InitializeComponent();

            // Navigate to string. template is our provided template string generated in AppData.cs
            modPreview.NavigateToString(template);
        }
    }
}
