using System.Windows.Forms;
using Telerik.WinForms.RichTextEditor;

namespace XHTML
{
    public class StrictInputBehavior : Telerik.WinForms.RichTextEditor.RichTextEditorInputBehavior
    {
        // Allowed operations are commented
        public StrictInputBehavior(RadRichTextBox editor) : base(editor) { }

        //protected override void PerformCopyOperation(System.Windows.Forms.KeyEventArgs e) { }
        protected override void PerformAlignmentOperation(KeyEventArgs e) { }
        //protected override void PerformBoldOperation(KeyEventArgs e) { }
        protected override void PerformClearFormatting(KeyEventArgs e) { }
        //protected override void PerformCutOperation(KeyEventArgs e) { }
        protected override void PerformDecrementFontSize(KeyEventArgs e) { }
        protected override void PerformFormattingSymbols(KeyEventArgs e) { }
        protected override void PerformIncrementFontSize(KeyEventArgs e) { }
        //protected override void PerformItalicOperation(KeyEventArgs e) { base.PerformItalicOperation(e); }
        //protected override void PerformPasteOperation(KeyEventArgs e) { }
        //protected override void PerformRedoOperation(KeyEventArgs e) { }
        //protected override void PerformSelectAllOperation(KeyEventArgs e) { }
        //protected override void PerformShowFindReplaceDialog(KeyEventArgs e) { }
        protected override void PerformShowFontPropertiesDialog(KeyEventArgs e) { }
        //protected override void PerformShowInsertHyperlinkDialog(KeyEventArgs e) { }
        //protected override void PerformShowSpellCheckingDialog(KeyEventArgs e) { }
        protected override void PerformSubscript(KeyEventArgs e) { }
        protected override void PerformSuperscript(KeyEventArgs e) { }
        protected override void PerformUnderlineOperation(KeyEventArgs e) { }
        //protected override void PerformUndoOperation(KeyEventArgs e) { }
        //protected override void ProcessBackKey(KeyEventArgs e) { }
        //protected override void ProcessDeleteKey(KeyEventArgs e) { }
        //protected override void ProcessDownKey(KeyEventArgs e) { }
        //protected override void ProcessEndKey(KeyEventArgs e) { }
        //protected override void ProcessEnterKey(KeyEventArgs e) { }
        //protected override void ProcessEscapeKey(KeyEventArgs e) { }
        //protected override void ProcessHomeKey(KeyEventArgs e) { }
        //protected override void ProcessKeyDownCore(KeyEventArgs e) { }
        //protected override void ProcessLeftKey(KeyEventArgs e) { }
        //protected override void ProcessPageDownKey(KeyEventArgs e) { }
        //protected override void ProcessPageUpKey(KeyEventArgs e) { }
        //protected override void ProcessRightKey(KeyEventArgs e) { }
        //protected override void ProcessTabKey(KeyEventArgs e) { }
        //protected override void ProcessUpKey(KeyEventArgs e) { }

    }
}
