using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText;
using Outlook.Core.Interfaces;
using System.Windows;
using System.Windows.Media;

namespace Outlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MessageTab.xaml
    /// </summary>
    public partial class MessageTab : ISupportDataContext, ISupportRichText
    {
        public MessageTab()
        {
            InitializeComponent();

            // Set the resources font size and font name
            fontSizes.ItemsSource = FontSizes;
            fontNames.ItemsSource = Fonts.SystemFontFamilies.ToList().Select(f => f.Source);
        }

        public static double[] FontSizes
        {
            get 
            {
                return new double[] 
                { 
                    3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0,11.5,12.0,14.0,16.0,18.0,20.0,22.0,24.0,26.0,28.0,36.0,48.0,72.0
                };
            }
        }

        private XamRichTextEditor _richTextEditor;
        public XamRichTextEditor RichTextEditor 
        { 
            get => _richTextEditor;
            set
            {
                _richTextEditor = value;

                if (_richTextEditor != null)
                {
                    _richTextEditor.Loaded -= RichTextEditor_Loaded;
                    _richTextEditor.SelectionChanged -= RichTextEditor_SelectionChanged;
                }

                if (_richTextEditor != null)
                {
                    _richTextEditor.Loaded += RichTextEditor_Loaded;
                    _richTextEditor.SelectionChanged += RichTextEditor_SelectionChanged;
                }
            }
        }

        private void RichTextEditor_SelectionChanged(object? sender, RichTextSelectionChangedEventArgs e)
        {
            UpdateVisualState();
        }

        private void RichTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            // 
            DocumentSpan span = RichTextEditor.Selection == null ? new DocumentSpan(0,0) : RichTextEditor.Selection.DocumentSpan;
            var settings = RichTextEditor.Document.GetCommonCharacterSettings(span);

            // get values or set default
            fontSizes.Value = settings.FontSize.HasValue ? settings.FontSize.Value.Points : 12.0;

        }

        /// <summary>
        /// Event handler for when the font size is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontSizes_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RichTextEditor.Selection.ApplyFontSize((double)e.NewValue);
        }

        /// <summary>
        /// Event handler for when the font name is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontNames_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedFont = e.NewValue as string;
            if (selectedFont != null)
            {
                RichTextEditor.Selection.ApplyFont(selectedFont);
            }
        }
    }
}
