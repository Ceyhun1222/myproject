using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs.Symbols;
using Telerik.Windows.Documents.Layout;

namespace XHTML_WPF
{

    public partial class SymbolPicker : UserControl
    {
        //private bool _contentLoaded;
        //internal RadComboBox comboCategory;
        //internal RadComboBox comboFont;
        //internal Grid LayoutRoot;
        //internal SymbolsTable symbolTable;
        //internal TextBlock textDescription;
        //internal TextBlock textSymbolPreview;
        private Dictionary<UnicodeCategory, string> unicodeCategoryNames;
        
        List<Tuple<string, UnicodeCategory?>> list = new List<Tuple<string, UnicodeCategory?>>();

        public event EventHandler<SymbolEventArgs> SymbolSelected;
        
        public SymbolPicker()
        {
            this.InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.unicodeCategoryNames = new Dictionary<UnicodeCategory, string>();
                list.Insert(0, new Tuple<string, UnicodeCategory?>(LocalizationManager.GetString("Documents_InsertSymbolDialog_UnicodeCategory_AllSymbols"), null));
                int cnt = 1;
                foreach (UnicodeCategory category in Enum.GetValues(typeof(UnicodeCategory)))
                {
                    this.unicodeCategoryNames.Add(category, LocalizationManager.GetString("Documents_InsertSymbolDialog_UnicodeCategory_" + category.ToString()));
                    list.Insert(cnt++, new Tuple<string, UnicodeCategory?>(LocalizationManager.GetString("Documents_InsertSymbolDialog_UnicodeCategory_" + category.ToString()), category));
                    //this.unicodeCategoryNames2.Add(LocalizationManager.GetString("Documents_InsertSymbolDialog_UnicodeCategory_" + category.ToString()), category);
                }
                this.comboFont.SelectedIndex = 0;
                this.PopulateFilter();
            }
        }

        [DebuggerNonUserCode, GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        internal Delegate _CreateDelegate(Type delegateType, string handler)
        {
            return Delegate.CreateDelegate(delegateType, this, handler);
        }
        

        private void characterTable_CharMouseEnter(object sender, SymbolEventArgs e)
        {
            this.textSymbolPreview.Text = e.Symbol.ToString();
            this.textDescription.Text = this.GetDescription(e.Symbol);
        }

        private void characterTable_CharMouseLeave(object sender, SymbolEventArgs e)
        {
            this.textSymbolPreview.Text = string.Empty;
            this.textDescription.Text = string.Empty;
        }

        private void characterTable_CharSelected(object sender, SymbolEventArgs e)
        {
            this.OnSymbolSelected(new SymbolEventArgs(e.Symbol));
        }

        private void comboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateSymbolTable();
        }

        private void comboFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboFont.SelectedItem != null)
            {
                UnicodeCategory? selectedValue = (UnicodeCategory?)this.comboCategory.SelectedValue;
                this.PopulateFilter();
                this.comboCategory.SelectedValue = selectedValue;
                if (this.comboCategory.SelectedIndex == -1)
                {
                    this.comboCategory.SelectedIndex = 0;
                }
                this.UpdateSymbolTable();
                this.textSymbolPreview.FontFamily = (FontFamily)this.comboFont.SelectedItem;
            }
        }

        private UnicodeCategory[] GetCategoriesForFont(FontFamily fontFamily)
        {
            bool[] flagArray = new bool[0x20];
            foreach (char ch in FontManager.GetSupportedCharacters(fontFamily))
            {
                flagArray[(int)char.GetUnicodeCategory(ch)] = true;
            }
            List<UnicodeCategory> list = new List<UnicodeCategory>();
            for (int i = 0; i < flagArray.Length; i++)
            {
                if (flagArray[i])
                {
                    list.Add((UnicodeCategory)i);
                }
            }
            return list.ToArray();
        }

        private string GetDescription(char c)
        {
            return string.Format("U+{0}: {1}", (int)c, this.unicodeCategoryNames[char.GetUnicodeCategory(c)]);
        }



        protected virtual void OnSymbolSelected(SymbolEventArgs e)
        {
            if (this.SymbolSelected != null)
            {
                this.SymbolSelected(this, e);
            }
        }

        private void PopulateFilter()
        {
            if (this.comboFont.SelectedItem != null)
            {
                //List<Tuple<string, UnicodeCategory?>> list = new List<Tuple<string, UnicodeCategory?>>();
                //list.Insert(0, new Tuple<string, UnicodeCategory?>(LocalizationManager.GetString("Documents_InsertSymbolDialog_UnicodeCategory_AllSymbols"), null));

                this.comboCategory.ItemsSource = list;
                this.comboCategory.SelectedIndex = 0;
            }
        }
        

        private void UpdateSymbolTable()
        {
            if ((this.comboFont.SelectedItem != null) && (this.comboCategory.SelectedItem != null))
            {
                FontFamily selectedItem = (FontFamily)this.comboFont.SelectedItem;
                UnicodeCategory? selectedValue = (UnicodeCategory?)this.comboCategory.SelectedValue;
                if (selectedValue.HasValue)
                {
                    this.symbolTable.PopulateWith(selectedItem, selectedValue.Value);
                }
                else
                {
                    this.symbolTable.PopulateWithAll(selectedItem);
                }
            }
        }

        public FontFamily SelectedFontFamily
        {
            get
            {
                return (FontFamily)this.comboFont.SelectedItem;
            }
            set
            {
                if (this.comboFont.SelectedItem != value)
                {
                    this.comboFont.SelectedItem = value;
                    if (this.comboFont.SelectedItem == null)
                    {
                        this.comboFont.SelectedIndex = 0;
                    }
                    this.PopulateFilter();
                }
            }
        }
    }
}

