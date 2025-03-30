using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText;
using Outlook.Core.Interfaces;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Outlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MessageTab.xaml
    /// </summary>
    public partial class MessageTab : ISupportDataContext, ISupportRichText
    {
        //
        private bool _isUpdatingState;
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

        // Reference to the RichTextEditor control in the MessageDialogView
        // We use MessageDialogView for writting emails and we need RichTextEditor for formatting the text
        // from the MessageTab (ISupportRichText)
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
            _isUpdatingState = true;

            /*
             * DocumentSpan:
               When you want to change the formatting of a section of text(e.g.bold, italic, color).
               When you need to get information about a specific section of text(e.g.font, font size).
               To select a specific section of text for further work.
            */
            // represents a range (interval) of text in a document that you can edit or analyze in XamRichTextEditor
            DocumentSpan docSpan = RichTextEditor.Selection == null ? new DocumentSpan(0,0) : RichTextEditor.Selection.DocumentSpan;

            /*
             * CharacterSettings:
             * Change the font, size, color of text.
               Set bold, italic, or underlined text.
               Used with DocumentSpan to format a specific section of text.
             */
            // is used to change the appearance of text within a given range
            var settings = RichTextEditor.Document.GetCommonCharacterSettings(docSpan);

            if (settings == null)
            {
                _isUpdatingState = false;
                return;
            }

            // Get values or set default from the document 
            UpdateFontSize(settings);
            UpdateFontFamily(settings);

            UpdateToggleButton(boldButton, settings.Bold);
            UpdateToggleButton(italicButton, settings.Italics);
            UpdateUnderlineState(settings);

            UpdateAligment(docSpan);


            _isUpdatingState = false;
        }

        private void UpdateAligment(DocumentSpan docSpan)
        {
            // When you want to find out what paragraph styles (alignment, line spacing, etc.)
            // are used in a certain range of text, you can use the GetCommonParagraphSettings method.

            // Get common paragraph settings for a given range
            var paragraphSettings = RichTextEditor.Document.GetCommonParagraphSettings(docSpan);

            // 
            if (paragraphSettings.ParagraphAlignment.HasValue)
            {
                switch (paragraphSettings.ParagraphAlignment.Value)
                {
                    case ParagraphAlignment.Start:

                        UpdateToggleButton(alignLeft, true);
                        break;
                    case ParagraphAlignment.Center:

                        UpdateToggleButton(alignCenter, true);
                        break;
                    case ParagraphAlignment.End:

                        UpdateToggleButton(alignRight, true);
                        break;

                    default:
                        UpdateToggleButton(alignLeft, false);
                        UpdateToggleButton(alignCenter, false);
                        UpdateToggleButton(alignRight, false);
                        break;
                }
            }
        }

        private void UpdateUnderlineState(CharacterSettings settings)
        {
            if (settings.UnderlineType.HasValue)
            {
                UpdateToggleButton(underlineButton, settings.UnderlineType.Value != UnderlineType.None);
            }
        }

        void UpdateToggleButton(ToggleButton toggleButton, bool? value) 
        {
            toggleButton.IsChecked = value.HasValue ? value.Value : false;
        }

        void UpdateFontSize(CharacterSettings settings)
        {
            fontSizes.Value = settings.FontSize.HasValue ? settings.FontSize.Value.Points : 12.0;
        }

        void UpdateFontFamily(CharacterSettings settings)
        {
            if (settings.FontSettings != null)
            {
                // todo: add font for multi-font scenarios
                fontNames.Value = (settings.FontSettings.Ascii.HasValue &&
                                   !string.IsNullOrWhiteSpace(settings.FontSettings.Ascii.Value.Name)) ?
                                   settings.FontSettings.Ascii.Value.Name :
                                   "Arial";
            }
        }


        /// <summary>
        /// Event handler for when the font size is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontSizes_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_isUpdatingState)
            {
                return;
            }

            RichTextEditor.Selection.ApplyFontSize((double)e.NewValue);
        }

        /// <summary>
        /// Event handler for when the font name is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fontNames_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_isUpdatingState)
            {
                return;
            }

            var selectedFont = e.NewValue as string;
            if (selectedFont != null)
            {
                RichTextEditor.Selection.ApplyFont(selectedFont);
            }
        }
    }
}
