using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs;
using Telerik.Windows.Controls.RichTextBoxUI.Menus;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Model.Styles;
using Telerik.Windows.Documents.RichTextBoxCommands;
using Telerik.Windows.Documents.UI.Extensibility;
using XHTML_WPF.Classes;
using XHTML_WPF.ViewModel;
using ContextMenu = Telerik.Windows.Controls.RichTextBoxUI.ContextMenu;
using ContextMenuEventArgs = Telerik.Windows.Controls.RichTextBoxUI.Menus.ContextMenuEventArgs;

namespace XHTML_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class XhtmlEditor2 : Window
    {
        public XhtmlEditor2()
        {
            //RadCompositionInitializer.Catalog = new TypeCatalog(typeof(HtmlFormatProvider));
            RadCompositionInitializer.Catalog = new TypeCatalog(
                // format providers
                //typeof(XamlFormatProvider),
                //typeof(RtfFormatProvider),
                //typeof(DocxFormatProvider),
                //typeof(PdfFormatProvider),
                typeof(HtmlFormatProvider),
                //typeof(TxtFormatProvider),

                // mini toolbars
                //typeof(SelectionMiniToolBar),
                //typeof(ImageMiniToolBar),

                // context menu
                typeof(ContextMenu),

                // the default English spellchecking dictionary
                //typeof(RadEn_USDictionary),

                // dialogs
                //typeof(AddNewBibliographicSourceDialog),
                //typeof(ChangeEditingPermissionsDialog),
                //typeof(EditCustomDictionaryDialog),
                typeof(FindReplaceDialog),
                //typeof(FloatingBlockPropertiesDialog),
                //typeof(FontPropertiesDialog),
                typeof(ImageEditorDialog),
                //typeof(InsertCaptionDialog),
                //typeof(InsertCrossReferenceWindow),
                //typeof(InsertDateTimeDialog),
                //typeof(InsertTableOfContentsDialog),
                //typeof(ManageBibliographicSourcesDialog),
                typeof(ManageBookmarksDialog),
                typeof(ManageStylesDialog),
                //typeof(NotesDialog),
                //typeof(ProtectDocumentDialog),
                typeof(InsertHyperlinkDialog),
                //typeof(RadInsertHyperlinkDialog),
                typeof(InsertSymbolDialog),
                //typeof(RadInsertSymbolDialog),
                //typeof(RadParagraphPropertiesDialog),
                typeof(SetNumberingValueDialog),
                //typeof(SpellCheckingDialog),
                typeof(StyleFormattingPropertiesDialog),
                //typeof(TableBordersDialog),
                typeof(TablePropertiesDialog),
                //typeof(TabStopsPropertiesDialog),
                //typeof(UnprotectDocumentDialog),
                //typeof(WatermarkSettingsDialog)
                typeof(InsertTableDialog)
                );
            InitializeComponent();
            InitForm();
        }

        // Always paste as Plain text to prevent any problems with visualization
        // All should be made manually using controls on the form
        private void Editor_CommandExecuting(object sender, CommandExecutingEventArgs e)
        {
            if (e.Command is PasteCommand)
            {
                e.Cancel = true;
                Editor.ClearAllFormatting();
                Editor.Insert(Clipboard.GetText());
            }
        }

        private readonly List<ContextMenuGroupType> NeedInContext = new List<ContextMenuGroupType> { ContextMenuGroupType.ClipboardCommands, ContextMenuGroupType.HyperlinkCommands, ContextMenuGroupType.TableCommands };

        /// <summary>
        /// Commands to ignore
        /// If List is null - all subitems ignoring
        /// </summary>
        private readonly Dictionary<ContextMenuGroupType, List<string>> IgnoreCommands =
            new Dictionary<ContextMenuGroupType, List<string>>
            {
                {ContextMenuGroupType.TextEditCommands, null },
                {ContextMenuGroupType.ListCommands, null },
                {ContextMenuGroupType.TableCommands, new List<string>(){ "Table Borders...", "Cell Alignment", "AutoFit" } }
            };

        private readonly List<Key> disabledKeys = new List<Key>
        {
            Key.U,Key.Add,Key.Space, Key.J,Key.R,Key.L,Key.E,Key.D
        };


        private void ContextMenu_Showing(object sender, ContextMenuEventArgs e)
        {
            try
            {
                for (var i = 0; i < e.ContextMenuGroupCollection.ToList().Count; i++)
                {
                    ContextMenuGroup contextMenuGroup = e.ContextMenuGroupCollection.ToList()[i];
                    if (IgnoreCommands.ContainsKey(contextMenuGroup.Type))
                    {
                        if (IgnoreCommands[contextMenuGroup.Type] == null)
                        {
                            e.ContextMenuGroupCollection.RemoveAt(i);
                            //foreach (RadMenuItem rmi in contextMenuGroup) rmi.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            foreach (RadMenuItem rmi in contextMenuGroup)
                            {
                                if (IgnoreCommands[contextMenuGroup.Type].Contains(rmi.Header))
                                {
                                    rmi.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Setting for Export, full specification: https://docs.telerik.com/devtools/wpf/controls/radrichtextbox/import-export/html/settings
        /// </summary>
        /// <returns>HtmlExportSettings</returns>
        private static HtmlExportSettings UseHtmlExportOptions()
        {
            try
            {
                HtmlExportSettings set = new HtmlExportSettings
                {
                    DocumentExportLevel = DocumentExportLevel.Fragment,
                    ExportBoldAsStrong = true,
                    ExportEmptyDocumentAsEmptyString = true,
                    ExportFontStylesAsTags = true,
                    ExportHeadingsAsTags = true,
                    ExportItalicAsEm = true,
                    ExportStyleMetadata = false,
                    SpanExportMode = SpanExportMode.DefaultBehavior,
                    StyleRepositoryExportMode = StyleRepositoryExportMode.DontExportStyles,
                    StylesExportMode = StylesExportMode.Inline
                };

                //set.PropertiesToIgnore["span"].AddRange(new[] { "font-family",  "dir" });
                //set.PropertiesToIgnore["span"].AddRange(new[] { "font-family"});
                //set.PropertiesToIgnore["p"].AddRange(new string[] { "margin-top", "margin-bottom", "margin-left", "margin-right", "line-height", "text-indent", "text-align", "direction" });
                //set.PropertiesToIgnore["p"].AddRange(new[] { "margin-top", "margin-bottom", "margin-left", "margin-right", "line-height", "direction" });
                //set.PropertiesToIgnore["table"].AddRange(new string[] { "border-top", "border-bottom", "border-left", "border-right", "margin-left", "border-spacing" });
                // set.PropertiesToIgnore["td"].AddRange(new string[] {"align" , "valign" , "border-top", "border-bottom", "border-left", "border-right", "padding", "vertical-align", "width" });
                //set.PropertiesToIgnore["tr"].AddRange(new string[] {"align" , "valign" });

                return set;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void InitForm()
        {
            try
            {
                //string version = Assembly.GetExecutingAssembly().GetName().Version.ToTitleString();
                //Title = $@"Xhtml Editor - {version}";
                htmlDataProvider.FormatProvider = new HtmlFormatProvider { ExportSettings = UseHtmlExportOptions() };
                var vm = new XhtmlEditorViewModel2();
                DataContext = vm;
                if (vm.CloseAction == null)
                    vm.CloseAction = Close;

                // todo - map to prop
                //vm.AipExists = Lib.isAipDbExists();
                //aipTab.Visibility = Lib.isAipDbExists() ? Visibility.Visible : Visibility.Collapsed;
                //aipTab.Visibility = Visibility.Collapsed;


                // Window Loaded
                Loaded += (a, b) =>
                {
                    Editor.IsContextMenuEnabled = true;
                    Editor.IsSelectionMiniToolBarEnabled = false;
                    ContextMenu contextMenu =
                        (ContextMenu)Editor.ContextMenu;
                    contextMenu.Showing += ContextMenu_Showing;
                    WindowState = WindowState.Maximized;
                    SetDefaultFont();
                    LoadStyles();
                // Removing new document command
                Editor.RegisteredApplicationCommands.Remove(ApplicationCommands.New);
                // Removing Open document command
                Editor.RegisteredApplicationCommands.Remove(ApplicationCommands.Open);

                    testBtn.Click += TestBtn_Click;
                };
                // HotKeys - disabling
                Editor.PreviewEditorKeyDown += (sender, args) =>
                {
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && disabledKeys.Contains(args.Key))
                    {
                        args.SuppressDefaultAction = true;
                    }
                //if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) || Keyboard.Modifiers.HasFlag(ModifierKeys.) && args.Key == Key.E)
                //{
                //    args.SuppressDefaultAction = true;
                //    args.OriginalArgs.Handled = true;
                //    Editor.Insert("€");
                //}
            };

                ContextMenu contextMenu2 = (ContextMenu)Editor.ContextMenu;
                contextMenu2.Showing += RichTextBox_ContextMenuShowing;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void RichTextBox_ContextMenuShowing(object sender, ContextMenuEventArgs e)
        {
            if (!Editor.Document.Selection.IsEmpty && Editor.Document.CaretPosition.IsPositionInsideTable)
            {
                RadMenuItem addHeaderForTable = new RadMenuItem()
                {
                    Header = "Set Header Background",
                    //Icon = new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Telerik.Windows.Controls.RichTextBoxUI;component/Images/MSOffice/16/cut.png", UriKind.Absolute)) }
                };
                addHeaderForTable.Click += this.OnChangeSelectionForeground;

                ContextMenuGroup contextMenuGroup = new ContextMenuGroup();
                contextMenuGroup.Add(addHeaderForTable);

                RadMenuItem addNumberHeaderForTable = new RadMenuItem()
                {
                    Header = "Set SubHeader Background"
                };
                addNumberHeaderForTable.Click += this.OnChangeSelectionForegroundForNumbering;


                contextMenuGroup.Add(addNumberHeaderForTable);
                e.ContextMenuGroupCollection.Add(contextMenuGroup);
            }
        }

        private void OnChangeSelectionForeground(object sender, RadRoutedEventArgs e)
        {
            Editor.ChangeTableCellBackground((Color)ColorConverter.ConvertFromString("#aaaacc"));
        }

        private void OnChangeSelectionForegroundForNumbering(object sender, RadRoutedEventArgs e)
        {
            Editor.ChangeTableCellBackground((Color)ColorConverter.ConvertFromString("#dddddd"));
        }
        private void LoadStyles()
        {
            try
            {
                for (int headingIndex = 1; headingIndex <= 9; headingIndex++)
                {
                    Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.GetHeadingStyleNameByIndex(headingIndex)).IsPrimary = false;
                }

                Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.CaptionStyleName).IsPrimary = false;
                Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.HyperlinkStyleName).IsPrimary = false;
                //Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.NormalStyleName).IsPrimary = false;


                StyleDefinition ss = new StyleDefinition();
                ss.Type = StyleType.Paragraph;
                //ss.BasedOn = Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.NormalStyleName);
                ss.NextStyleName = "Normal";

                ss.SpanProperties.FontSize = Unit.PointToDip(20);
                ss.SpanProperties.ForeColor = (Color)ColorConverter.ConvertFromString("#800000");
                ss.DisplayName = "Heading 4";
                ss.ParagraphProperties.TextAlignment = RadTextAlignment.Center;
                ss.Name = "Heading 4";
                Editor.Document.StyleRepository.Add(ss);

                ss = new StyleDefinition();
                ss.Type = StyleType.Paragraph;
                //ss.BasedOn = Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.NormalStyleName);
                ss.NextStyleName = "Normal";
                ss.SpanProperties.FontSize = Unit.PointToDip(16);
                ss.SpanProperties.ForeColor = (Color)ColorConverter.ConvertFromString("#800000");
                ss.DisplayName = "Heading 5";
                ss.Name = "AIP_H5";
                Editor.Document.StyleRepository.Add(ss);

                //ss = new StyleDefinition();
                //ss.Type = StyleType.Paragraph;
                ////ss.BasedOn = Editor.Document.StyleRepository.GetValueOrNull(RadDocumentDefaultStyles.NormalStyleName);
                //ss.NextStyleName = "Normal";
                //ss.SpanProperties.FontSize = Unit.PointToDip(14);
                //ss.SpanProperties.ForeColor = Colors.Orange;
                //ss.DisplayName = "Heading 6";
                //ss.Name = "AIP_H6";
                //Editor.Document.StyleRepository.Add(ss);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            var frm = new InsertTestDialog();
            frm.Show();
        }

        private void SetDefaultFont()
        {
            try
            {

                Editor.Document.Selection.SelectAll();
                var fonts = FontManager.GetRegisteredFonts();
                var fnt = fonts.FirstOrDefault(x => x.DisplayName == "Arial Unicode MS") ?? fonts.FirstOrDefault(x => x.DisplayName == "Arial") ?? fonts.FirstOrDefault(x => x.DisplayName == "Helvetica") ?? fonts.FirstOrDefault(x => x.DisplayName == "Microsoft Sans Serif");
                Editor.Document.Style.SpanProperties.FontFamily = fnt;
                Editor.ChangeFontFamily(fnt);
                Editor.Document.Selection.Clear();
                Editor.Document.History.Clear(); //  to prevent undo set default font
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //InsertAipSectionDialog window = new InsertAipSectionDialog();
                //window.ShowDialog();
                //if (window.outputValue != null)
                //{
                //    Editor.InsertHyperlink(window.outputValue);
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddHeading_Click(object sender, RoutedEventArgs e)
        {

            StyleDefinition charStyle = new StyleDefinition();
            charStyle.Type = StyleType.Character;
            charStyle.SpanProperties.FontSize = Unit.PointToDip(20);
            charStyle.SpanProperties.ForeColor = Colors.Orange;
            charStyle.DisplayName = "eaip_H4";
            charStyle.Name = "eaip_H4";
            Editor.Document.StyleRepository.Add(charStyle);

            var textRange = Editor.Document.Selection;

            //Section section = new Section();
            //Paragraph paragraph = new Paragraph() { TextAlignment = RadTextAlignment.Center };
            //Paragraph paragraph = new Paragraph();
            Span span = new Span(textRange.GetSelectedText());
            span.StyleName = "eaip_H4";
            //Color myArgbColor = (Color)ColorConverter.ConvertFromString("#800000");
            //span.ForeColor = myArgbColor;
            //paragraph.Inlines.Add(span);
            //section.Blocks.Add(paragraph);
            //Editor.Document.Sections.Add(section);

            Editor.InsertInline(span);


            //RadDocument originalDocument = new RadDocument();
            //Section originalSection = new Section();

            ////NumberedList originalNumberedList = new NumberedList(originalDocument);
            //Paragraph originalParagraph = new Paragraph();

            //originalParagraph.Inlines.Add(new Span("Numbered Item in Original Document"));
            ////originalNumberedList.AddParagraph(originalParagraph);

            //originalSection.Blocks.Add(originalParagraph);
            //originalSection.Blocks.Add(new Paragraph());
            //originalDocument.Sections.Add(originalSection);
            //originalDocument.CaretPosition.MoveToLastPositionInDocument();

            //RadDocument documentFragment = new RadDocument();
            //Section sectionFragment = new Section();

            //Paragraph paragraphFragment = new Paragraph();

            //paragraphFragment.Inlines.Add(new Span("Numbered Item in Document Fragment"));
            ////originalNumberedList.AddParagraph(paragraphFragment);

            //sectionFragment.Blocks.Add(paragraphFragment);
            //documentFragment.Sections.Add(sectionFragment);

            //originalDocument.InsertFragment(new DocumentFragment(documentFragment));

            //Editor.ChangeTextForeColor((Color)ColorConverter.ConvertFromString("#800000"));



            ////Editor.Focus();
            ////Editor.Insert("test");
            //Editor.UpdateEditorLayout();






            //StyleDefinition linkedCharStyle = new StyleDefinition();
            //linkedCharStyle.Type = StyleType.Character;
            //linkedCharStyle.SpanProperties.FontWeight = FontWeights.Bold;
            //linkedCharStyle.SpanProperties.FontSize = Unit.PointToDip(30);
            //linkedCharStyle.SpanProperties.ForeColor = Colors.Purple;
            //linkedCharStyle.DisplayName = "linkedCharStyle";
            //linkedCharStyle.Name = "linkedCharStyle";


            //var textRange = Editor.Document.Selection;

            ////TextRange newRange = new TextRange(); 
            ////Editor.Document.Selection.GetSelectedText().Replace(textRange, "<h1>teeee"+ textRange+"</h1>");

            ////Editor.Insert("<span>" + textRange + "</span>");
            //Section section = new Section();
            ////Paragraph paragraph = new Paragraph();
            ////Span span = new Span(textRange.ToString());

            ////paragraph.Inlines.Add(span);
            ////section.Blocks.Add(paragraph);
            ////RadDocument document = new RadDocument();

            //Span span = new Span(textRange.ToString()+"test");
            ////span.ForeColor = System.Windows.Media.Color.FromArgb(1, 128, 0, 0);
            ////span.FontWeight = FontWeights.Bold;
            ////span.FontSize = Unit.PointToDip(10);
            //Paragraph paragraph = new Paragraph();
            //paragraph.Inlines.Add(span);
            //section.Blocks.Add(paragraph);
            //Editor.Document.Sections.Add(section);
        }

        private void testBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Span span = new Span("Test text");
            Editor.InsertInline(span);
        }
    }

}
