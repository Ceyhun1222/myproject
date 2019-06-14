using Aran.Aim.BusinessRules.SbvrParser;
using BRuleManager.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BRuleManager.View
{
    /// <summary>
    /// Interaction logic for SbvrEditor.xaml
    /// </summary>
    public partial class SbvrEditorControl : UserControl
    {
        public SbvrEditorControl()
        {
            InitializeComponent();

            Loaded += SbvrEditorControl_Loaded;
        }

        public string GetTaggedDescription()
        {
            var sb = new StringBuilder();

            foreach (var block in ui_rtb.Document.Blocks)
            {
                if (block is Paragraph prg)
                {
                    foreach(var inLine in prg.Inlines)
                    {
                        if ((inLine is InlineUIContainer inlineCont) && (inlineCont.Child is TextBlock tb))
                        {
                            if (tb.Tag != null)
                                sb.Append(SurroundText((TaggedKey)tb.Tag, tb.Text));
                        }
                        else if (inLine is Run runLine)
                        {
                            sb.Append(SurroundText(TaggedKey.Name, runLine.Text));
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public void SetTaggedDescription(string taggedDescription)
        {
            var conv = new Converters.TaggedDescriptionItemsConverter();
            var obj = conv.Convert(taggedDescription, null, null, null);

            if (obj is List<TaggedItem> items)
            {
                foreach(var item in items)
                {
                    AddText(item.Text, item.Key);
                }
            }
        }

        private string SurroundText(TaggedKey key, string text)
        {
            var dscArr = typeof(TaggedKey).GetField(key.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (dscArr?.Length > 0)
                return $"<{dscArr[0].Description}>{text.Trim()}</{dscArr[0].Description}>";
            return string.Empty;
        }

        private void SbvrEditorControl_Loaded(object sender, RoutedEventArgs e)
        {
            dynamic eo = DataContext;
            eo.Vocabulary.PropertySelectedAction = new Action<string>((pathText) =>
            {
                AddText(pathText, TaggedKey.Noun);
            });
        }

        private void AddVocabulary_Click(object sender, RoutedEventArgs e)
        {
            string text = null;
            string tag = null;

            if (sender is FrameworkElement fe)
            {
                text = fe.DataContext as string;
                tag = fe.Tag as string;
            }
            else if (sender is Hyperlink hyperlinkSender)
            {
                tag = hyperlinkSender.Tag as string;

                if (tag == "other-keyword")
                {
                    text = (DataContext as dynamic).Vocabulary.OtherKeyword;
                    tag = "Keyword";
                    (DataContext as dynamic).Vocabulary.OtherKeyword = string.Empty;
                }
                else if (tag == "other-verb")
                {
                    text = (DataContext as dynamic).Vocabulary.OtherVerb;
                    tag = "Verb";
                    (DataContext as dynamic).Vocabulary.OtherVerb = string.Empty;
                }
                else if (tag == "other-noun")
                {
                    text = (DataContext as dynamic).Vocabulary.OtherNoun;
                    tag = "Noun";
                    (DataContext as dynamic).Vocabulary.OtherNoun = string.Empty;
                }
                else if (tag == "Noun")
                {
                    text = hyperlinkSender.DataContext as string;
                    (DataContext as dynamic).Vocabulary.LastAddedAimObjectName = text;
                }
            }

            if (text == null || tag == null)
            {
                Console.Error.WriteLine("No sufficient parameter");
                return;
            }

            if (!Enum.TryParse<TaggedKey>(tag as string, out var vocabularyType))
            {
                Console.Error.WriteLine("Button Tag has invalid TaggedKey value");
                return;
            }

            AddText(text, vocabularyType);
        }

        private void AddText(string text, TaggedKey vocabularyType)
        {
            var styleConverter = new TaggedStyleConverter();

            var tb = new TextBlock { Text = text, Tag = vocabularyType };
            tb.Margin = new Thickness(4, 1, 4, 1);

            if (styleConverter.Convert(vocabularyType, null, "Color", CultureInfo.InvariantCulture) is Brush foreground)
                tb.Foreground = foreground;

            var fontWeightValue = styleConverter.Convert(vocabularyType, null, "FontWeight", CultureInfo.InvariantCulture);
            if (fontWeightValue != null && fontWeightValue != Binding.DoNothing)
                tb.FontWeight = (FontWeight)fontWeightValue;

            if (styleConverter.Convert(vocabularyType, null, "TextDecorations", CultureInfo.InvariantCulture) is TextDecorationCollection textDecorationsValue)
                tb.TextDecorations = textDecorationsValue;

            tb.Background = new SolidColorBrush(Colors.WhiteSmoke);

            new InlineUIContainer(tb, ui_rtb.CaretPosition);

            var cp = ui_rtb.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

            if (cp != null)
                ui_rtb.CaretPosition = cp;

            ui_rtb.Focus();

        }

        private void TextBlock_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var tb = sender as TextBox;
                if (tb.Text.Length > 0 && Enum.TryParse<TaggedKey>(tb.Tag as string, out var vocabularyType))
                {
                    AddText(tb.Text, vocabularyType);
                    tb.Text = string.Empty;
                }
            }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var cb = sender as ComboBox;
                if (cb.Text.Length > 0 && Enum.TryParse<TaggedKey>(cb.Tag as string, out var vocabularyType))
                {
                    AddText(cb.Text, vocabularyType);
                    cb.Text = string.Empty;
                }

            }
        }
    }
}
