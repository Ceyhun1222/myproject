using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOSSM.Util.Notams.Xml
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp", TypeName = "Notam")]
    public partial class NotamQueryResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamQueryResponseType notamQueryField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamQueryResponseType NotamQuery
        {
            get
            {
                return this.notamQueryField;
            }
            set
            {
                this.notamQueryField = value;
                this.RaisePropertyChanged("NotamQuery");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamListElementType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamListElementType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamListElementType : NotamQueryListElementType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NotamListElementType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryListElementType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 2)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 3)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamQueryType notamQueryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamQueryType NotamQuery
        {
            get
            {
                return this.notamQueryField;
            }
            set
            {
                this.notamQueryField = value;
                this.RaisePropertyChanged("NotamQuery");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryListResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamListElementType[] notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Notam", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamListElementType[] Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryListResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamQueryListResponseType notamQueryListField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamQueryListResponseType NotamQueryList
        {
            get
            {
                return this.notamQueryListField;
            }
            set
            {
                this.notamQueryListField = value;
                this.RaisePropertyChanged("NotamQueryList");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryListType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamQueryListElementType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamQueryListElementType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryListMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamQueryListType notamQueryListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamQueryListType NotamQueryList
        {
            get
            {
                return this.notamQueryListField;
            }
            set
            {
                this.notamQueryListField = value;
                this.RaisePropertyChanged("NotamQueryList");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamValidateMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamStoreType notamStoreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamStoreType NotamStore
        {
            get
            {
                return this.notamStoreField;
            }
            set
            {
                this.notamStoreField = value;
                this.RaisePropertyChanged("NotamStore");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamStoreType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamStoreType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamStoreType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamStoreResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamStoreType notamStoreField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamStoreType NotamStore
        {
            get
            {
                return this.notamStoreField;
            }
            set
            {
                this.notamStoreField = value;
                this.RaisePropertyChanged("NotamStore");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamStoreRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamStoreMessageType NotamStore;

        public notamStoreRequest()
        {
        }

        public notamStoreRequest(NotamStoreMessageType NotamStore)
        {
            this.NotamStore = NotamStore;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamStoreResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "NotamStoreResponse", Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamStoreResponseMessageType NotamStoreResponse1;

        public notamStoreResponse()
        {
        }

        public notamStoreResponse(NotamStoreResponseMessageType NotamStoreResponse1)
        {
            this.NotamStoreResponse1 = NotamStoreResponse1;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamValidateRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamValidateMessageType NotamValidate;

        public notamValidateRequest()
        {
        }

        public notamValidateRequest(NotamValidateMessageType NotamValidate)
        {
            this.NotamValidate = NotamValidate;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamValidateResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "NotamValidateResponse", Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamStoreResponseMessageType NotamValidateResponse1;

        public notamValidateResponse()
        {
        }

        public notamValidateResponse(NotamStoreResponseMessageType NotamValidateResponse1)
        {
            this.NotamValidateResponse1 = NotamValidateResponse1;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamQueryListRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamQueryListMessageType NotamQueryList;

        public notamQueryListRequest()
        {
        }

        public notamQueryListRequest(NotamQueryListMessageType NotamQueryList)
        {
            this.NotamQueryList = NotamQueryList;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class notamQueryListResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "NotamQueryListResponse", Namespace = "http://www.teamead.com/ws/inodp", Order = 0)]
        public NotamQueryListResponseMessageType NotamQueryListResponse1;

        public notamQueryListResponse()
        {
        }

        public notamQueryListResponse(NotamQueryListResponseMessageType NotamQueryListResponse1)
        {
            this.NotamQueryListResponse1 = NotamQueryListResponse1;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamStoreMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private transactionType transactionField;

        private RequestHeaderNotamType requestHeaderField;

        private NotamStoreType notamStoreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp", Order = 0)]
        public transactionType transaction
        {
            get
            {
                return this.transactionField;
            }
            set
            {
                this.transactionField = value;
                this.RaisePropertyChanged("transaction");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NotamStoreType NotamStore
        {
            get
            {
                return this.notamStoreField;
            }
            set
            {
                this.notamStoreField = value;
                this.RaisePropertyChanged("NotamStore");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp")]
    public partial class transactionType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string uuidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string uuid
        {
            get
            {
                return this.uuidField;
            }
            set
            {
                this.uuidField = value;
                this.RaisePropertyChanged("uuid");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SingleNotamProposalStatusResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string statusField;

        private string notamProposalIdentifierField;

        private string reasonField;

        private NotamBaseType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string NotamProposalIdentifier
        {
            get
            {
                return this.notamProposalIdentifierField;
            }
            set
            {
                this.notamProposalIdentifierField = value;
                this.RaisePropertyChanged("NotamProposalIdentifier");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
                this.RaisePropertyChanged("Reason");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public NotamBaseType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SingleNotamStoreType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SingleNotamQueryType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamBaseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        private NotamTypeType typeField;

        private string referredSeriesField;

        private string referredNumberField;

        private string referredYearField;

        private QLineType qLineField;

        private string coordinatesField;

        private string radiusField;

        private string[] itemAField;

        private string startValidityField;

        private string endValidityField;

        private EstimationType estimationField;

        private bool estimationFieldSpecified;

        private string itemDField;

        private string itemEField;

        private MultiLanguageType multiLanguageField;

        private string itemFField;

        private string itemGField;

        private string itemXField;

        private string operatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 2)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 3)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public NotamTypeType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("Type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string ReferredSeries
        {
            get
            {
                return this.referredSeriesField;
            }
            set
            {
                this.referredSeriesField = value;
                this.RaisePropertyChanged("ReferredSeries");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 6)]
        public string ReferredNumber
        {
            get
            {
                return this.referredNumberField;
            }
            set
            {
                this.referredNumberField = value;
                this.RaisePropertyChanged("ReferredNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 7)]
        public string ReferredYear
        {
            get
            {
                return this.referredYearField;
            }
            set
            {
                this.referredYearField = value;
                this.RaisePropertyChanged("ReferredYear");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public QLineType QLine
        {
            get
            {
                return this.qLineField;
            }
            set
            {
                this.qLineField = value;
                this.RaisePropertyChanged("QLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
                this.RaisePropertyChanged("Coordinates");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 10)]
        public string Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
                this.RaisePropertyChanged("Radius");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemA", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
        public string[] ItemA
        {
            get
            {
                return this.itemAField;
            }
            set
            {
                this.itemAField = value;
                this.RaisePropertyChanged("ItemA");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        public string StartValidity
        {
            get
            {
                return this.startValidityField;
            }
            set
            {
                this.startValidityField = value;
                this.RaisePropertyChanged("StartValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        public string EndValidity
        {
            get
            {
                return this.endValidityField;
            }
            set
            {
                this.endValidityField = value;
                this.RaisePropertyChanged("EndValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        public EstimationType Estimation
        {
            get
            {
                return this.estimationField;
            }
            set
            {
                this.estimationField = value;
                this.RaisePropertyChanged("Estimation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimationSpecified
        {
            get
            {
                return this.estimationFieldSpecified;
            }
            set
            {
                this.estimationFieldSpecified = value;
                this.RaisePropertyChanged("EstimationSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        public string ItemD
        {
            get
            {
                return this.itemDField;
            }
            set
            {
                this.itemDField = value;
                this.RaisePropertyChanged("ItemD");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        public string ItemE
        {
            get
            {
                return this.itemEField;
            }
            set
            {
                this.itemEField = value;
                this.RaisePropertyChanged("ItemE");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
        public MultiLanguageType MultiLanguage
        {
            get
            {
                return this.multiLanguageField;
            }
            set
            {
                this.multiLanguageField = value;
                this.RaisePropertyChanged("MultiLanguage");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 18)]
        public string ItemF
        {
            get
            {
                return this.itemFField;
            }
            set
            {
                this.itemFField = value;
                this.RaisePropertyChanged("ItemF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 19)]
        public string ItemG
        {
            get
            {
                return this.itemGField;
            }
            set
            {
                this.itemGField = value;
                this.RaisePropertyChanged("ItemG");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 20)]
        public string ItemX
        {
            get
            {
                return this.itemXField;
            }
            set
            {
                this.itemXField = value;
                this.RaisePropertyChanged("ItemX");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 21)]
        public string Operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
                this.RaisePropertyChanged("Operator");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum NotamTypeType
    {

        /// <remarks/>
        N,

        /// <remarks/>
        R,

        /// <remarks/>
        C,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class QLineType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fIRField;

        private string code23Field;

        private string code45Field;

        private TrafficType trafficField;

        private PurposeType purposeField;

        private ScopeType scopeField;

        private string lowerField;

        private string upperField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string FIR
        {
            get
            {
                return this.fIRField;
            }
            set
            {
                this.fIRField = value;
                this.RaisePropertyChanged("FIR");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Code23
        {
            get
            {
                return this.code23Field;
            }
            set
            {
                this.code23Field = value;
                this.RaisePropertyChanged("Code23");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string Code45
        {
            get
            {
                return this.code45Field;
            }
            set
            {
                this.code45Field = value;
                this.RaisePropertyChanged("Code45");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public TrafficType Traffic
        {
            get
            {
                return this.trafficField;
            }
            set
            {
                this.trafficField = value;
                this.RaisePropertyChanged("Traffic");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public PurposeType Purpose
        {
            get
            {
                return this.purposeField;
            }
            set
            {
                this.purposeField = value;
                this.RaisePropertyChanged("Purpose");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public ScopeType Scope
        {
            get
            {
                return this.scopeField;
            }
            set
            {
                this.scopeField = value;
                this.RaisePropertyChanged("Scope");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 6)]
        public string Lower
        {
            get
            {
                return this.lowerField;
            }
            set
            {
                this.lowerField = value;
                this.RaisePropertyChanged("Lower");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 7)]
        public string Upper
        {
            get
            {
                return this.upperField;
            }
            set
            {
                this.upperField = value;
                this.RaisePropertyChanged("Upper");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum TrafficType
    {

        /// <remarks/>
        I,

        /// <remarks/>
        V,

        /// <remarks/>
        IV,

        /// <remarks/>
        K,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum PurposeType
    {

        /// <remarks/>
        NB,

        /// <remarks/>
        BO,

        /// <remarks/>
        M,

        /// <remarks/>
        K,

        /// <remarks/>
        NBO,

        /// <remarks/>
        B,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum ScopeType
    {

        /// <remarks/>
        A,

        /// <remarks/>
        E,

        /// <remarks/>
        W,

        /// <remarks/>
        AE,

        /// <remarks/>
        AW,

        /// <remarks/>
        K,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum EstimationType
    {

        /// <remarks/>
        EST,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class MultiLanguageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private MultiLanguageTypeItemE[] itemEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemE", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public MultiLanguageTypeItemE[] ItemE
        {
            get
            {
                return this.itemEField;
            }
            set
            {
                this.itemEField = value;
                this.RaisePropertyChanged("ItemE");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class MultiLanguageTypeItemE : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string languageField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
                this.RaisePropertyChanged("language");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SingleNotamStoreType : NotamBaseType
    {

        private LinkageStoreType linkageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public LinkageStoreType Linkage
        {
            get
            {
                return this.linkageField;
            }
            set
            {
                this.linkageField = value;
                this.RaisePropertyChanged("Linkage");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class LinkageStoreType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private object itemField;

        private ItemChoiceType1 itemElementNameField;

        private string insertEADGridField;

        private string copyToItemEField;

        private string flightLevelUpdateField;

        private string linkColocatedNavigationAidField;

        private string bufferedQueryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirwayLinkage", typeof(AirwayLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AirwaySegmentLinkage", typeof(AirwaySegmentLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("NavigationAidLinkage", typeof(NavigationAidStoreType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PolygonLinkage", typeof(PolygonLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RestrictedAirspaceLinkage", typeof(RestrictedAirspaceLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RunwayLinkage", typeof(RunwayLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SidLinkage", typeof(SidLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StarLinkage", typeof(StarLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
                this.RaisePropertyChanged("Item");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
                this.RaisePropertyChanged("ItemElementName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string InsertEADGrid
        {
            get
            {
                return this.insertEADGridField;
            }
            set
            {
                this.insertEADGridField = value;
                this.RaisePropertyChanged("InsertEADGrid");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string CopyToItemE
        {
            get
            {
                return this.copyToItemEField;
            }
            set
            {
                this.copyToItemEField = value;
                this.RaisePropertyChanged("CopyToItemE");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string FlightLevelUpdate
        {
            get
            {
                return this.flightLevelUpdateField;
            }
            set
            {
                this.flightLevelUpdateField = value;
                this.RaisePropertyChanged("FlightLevelUpdate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string LinkColocatedNavigationAid
        {
            get
            {
                return this.linkColocatedNavigationAidField;
            }
            set
            {
                this.linkColocatedNavigationAidField = value;
                this.RaisePropertyChanged("LinkColocatedNavigationAid");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
        public string BufferedQuery
        {
            get
            {
                return this.bufferedQueryField;
            }
            set
            {
                this.bufferedQueryField = value;
                this.RaisePropertyChanged("BufferedQuery");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class AirwayLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private AirwayType[] airwayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Airway", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public AirwayType[] Airway
        {
            get
            {
                return this.airwayField;
            }
            set
            {
                this.airwayField = value;
                this.RaisePropertyChanged("Airway");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class AirwayType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string airwayDesignatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string AirwayDesignator
        {
            get
            {
                return this.airwayDesignatorField;
            }
            set
            {
                this.airwayDesignatorField = value;
                this.RaisePropertyChanged("AirwayDesignator");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class AirwaySegmentLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private AirwaySegmentType[] airwaySegmentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirwaySegment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public AirwaySegmentType[] AirwaySegment
        {
            get
            {
                return this.airwaySegmentField;
            }
            set
            {
                this.airwaySegmentField = value;
                this.RaisePropertyChanged("AirwaySegment");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class AirwaySegmentType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string airwayDesignatorField;

        private string segmentStartField;

        private string segmentEndField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string AirwayDesignator
        {
            get
            {
                return this.airwayDesignatorField;
            }
            set
            {
                this.airwayDesignatorField = value;
                this.RaisePropertyChanged("AirwayDesignator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string SegmentStart
        {
            get
            {
                return this.segmentStartField;
            }
            set
            {
                this.segmentStartField = value;
                this.RaisePropertyChanged("SegmentStart");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string SegmentEnd
        {
            get
            {
                return this.segmentEndField;
            }
            set
            {
                this.segmentEndField = value;
                this.RaisePropertyChanged("SegmentEnd");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NavigationAidStoreType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private NavigationAidType[] navigationAidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NavigationAid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public NavigationAidType[] NavigationAid
        {
            get
            {
                return this.navigationAidField;
            }
            set
            {
                this.navigationAidField = value;
                this.RaisePropertyChanged("NavigationAid");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NavigationAidType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string navigationAidIDField;

        private string navigationAidNameField;

        private NavigationAidTYPEType navigationAidTYPEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NavigationAidID
        {
            get
            {
                return this.navigationAidIDField;
            }
            set
            {
                this.navigationAidIDField = value;
                this.RaisePropertyChanged("NavigationAidID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string NavigationAidName
        {
            get
            {
                return this.navigationAidNameField;
            }
            set
            {
                this.navigationAidNameField = value;
                this.RaisePropertyChanged("NavigationAidName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NavigationAidTYPEType NavigationAidTYPE
        {
            get
            {
                return this.navigationAidTYPEField;
            }
            set
            {
                this.navigationAidTYPEField = value;
                this.RaisePropertyChanged("NavigationAidTYPE");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum NavigationAidTYPEType
    {

        /// <remarks/>
        DME,

        /// <remarks/>
        NDB,

        /// <remarks/>
        OTH,

        /// <remarks/>
        TAC,

        /// <remarks/>
        VOR,

        /// <remarks/>
        WPT,

        /// <remarks/>
        MKR,

        /// <remarks/>
        ILS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class PolygonLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private PolygonType[] polygonField;

        private string straightLineField;

        private float polygonLineBufferField;

        private bool polygonLineBufferFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Polygon", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public PolygonType[] Polygon
        {
            get
            {
                return this.polygonField;
            }
            set
            {
                this.polygonField = value;
                this.RaisePropertyChanged("Polygon");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string StraightLine
        {
            get
            {
                return this.straightLineField;
            }
            set
            {
                this.straightLineField = value;
                this.RaisePropertyChanged("StraightLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public float PolygonLineBuffer
        {
            get
            {
                return this.polygonLineBufferField;
            }
            set
            {
                this.polygonLineBufferField = value;
                this.RaisePropertyChanged("PolygonLineBuffer");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PolygonLineBufferSpecified
        {
            get
            {
                return this.polygonLineBufferFieldSpecified;
            }
            set
            {
                this.polygonLineBufferFieldSpecified = value;
                this.RaisePropertyChanged("PolygonLineBufferSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class PolygonType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string polygonCoordinateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string PolygonCoordinate
        {
            get
            {
                return this.polygonCoordinateField;
            }
            set
            {
                this.polygonCoordinateField = value;
                this.RaisePropertyChanged("PolygonCoordinate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RestrictedAirspaceLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RestrictedAirspaceType[] restrictedAirspaceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RestrictedAirspace", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RestrictedAirspaceType[] RestrictedAirspace
        {
            get
            {
                return this.restrictedAirspaceField;
            }
            set
            {
                this.restrictedAirspaceField = value;
                this.RaisePropertyChanged("RestrictedAirspace");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RestrictedAirspaceType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string restrictedAirspaceDesignatorField;

        private string restrictedAirspaceNameField;

        private string restrictedAirspaceTYPEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string RestrictedAirspaceDesignator
        {
            get
            {
                return this.restrictedAirspaceDesignatorField;
            }
            set
            {
                this.restrictedAirspaceDesignatorField = value;
                this.RaisePropertyChanged("RestrictedAirspaceDesignator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string RestrictedAirspaceName
        {
            get
            {
                return this.restrictedAirspaceNameField;
            }
            set
            {
                this.restrictedAirspaceNameField = value;
                this.RaisePropertyChanged("RestrictedAirspaceName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string RestrictedAirspaceTYPE
        {
            get
            {
                return this.restrictedAirspaceTYPEField;
            }
            set
            {
                this.restrictedAirspaceTYPEField = value;
                this.RaisePropertyChanged("RestrictedAirspaceTYPE");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RunwayLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RunwayType[] runwayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Runway", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RunwayType[] Runway
        {
            get
            {
                return this.runwayField;
            }
            set
            {
                this.runwayField = value;
                this.RaisePropertyChanged("Runway");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RunwayType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string runwayDesignatorField;

        private string runwayAerodromeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string RunwayDesignator
        {
            get
            {
                return this.runwayDesignatorField;
            }
            set
            {
                this.runwayDesignatorField = value;
                this.RaisePropertyChanged("RunwayDesignator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string RunwayAerodrome
        {
            get
            {
                return this.runwayAerodromeField;
            }
            set
            {
                this.runwayAerodromeField = value;
                this.RaisePropertyChanged("RunwayAerodrome");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SidLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private SidStarType[] sidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Sid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public SidStarType[] Sid
        {
            get
            {
                return this.sidField;
            }
            set
            {
                this.sidField = value;
                this.RaisePropertyChanged("Sid");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SidStarType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string sidStarDesignatorField;

        private string sidStarDescriptionField;

        private string sidStarTransIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string SidStarDesignator
        {
            get
            {
                return this.sidStarDesignatorField;
            }
            set
            {
                this.sidStarDesignatorField = value;
                this.RaisePropertyChanged("SidStarDesignator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string SidStarDescription
        {
            get
            {
                return this.sidStarDescriptionField;
            }
            set
            {
                this.sidStarDescriptionField = value;
                this.RaisePropertyChanged("SidStarDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string SidStarTransID
        {
            get
            {
                return this.sidStarTransIDField;
            }
            set
            {
                this.sidStarTransIDField = value;
                this.RaisePropertyChanged("SidStarTransID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class StarLinkageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private SidStarType[] starField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Star", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public SidStarType[] Star
        {
            get
            {
                return this.starField;
            }
            set
            {
                this.starField = value;
                this.RaisePropertyChanged("Star");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp", IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":AirwayLinkage")]
        AirwayLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":AirwaySegmentLinkage")]
        AirwaySegmentLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":NavigationAidLinkage")]
        NavigationAidLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":PolygonLinkage")]
        PolygonLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":RestrictedAirspaceLinkage")]
        RestrictedAirspaceLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":RunwayLinkage")]
        RunwayLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":SidLinkage")]
        SidLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":StarLinkage")]
        StarLinkage,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SingleNotamQueryType : NotamBaseType
    {

        private LinkageQueryType linkageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public LinkageQueryType Linkage
        {
            get
            {
                return this.linkageField;
            }
            set
            {
                this.linkageField = value;
                this.RaisePropertyChanged("Linkage");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class LinkageQueryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private object itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AirwayLinkage", typeof(AirwayLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("AirwaySegmentLinkage", typeof(AirwaySegmentLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("NavigationAidLinkage", typeof(NavigationAidQueryType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PolygonLinkage", typeof(PolygonLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RestrictedAirspaceLinkage", typeof(RestrictedAirspaceLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RunwayLinkage", typeof(RunwayLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SidLinkage", typeof(SidLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StarLinkage", typeof(StarLinkageType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
                this.RaisePropertyChanged("Item");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
                this.RaisePropertyChanged("ItemElementName");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NavigationAidQueryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private NavigationAidType[] navigationAidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NavigationAid", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public NavigationAidType[] NavigationAid
        {
            get
            {
                return this.navigationAidField;
            }
            set
            {
                this.navigationAidField = value;
                this.RaisePropertyChanged("NavigationAid");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":AirwayLinkage")]
        AirwayLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":AirwaySegmentLinkage")]
        AirwaySegmentLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":NavigationAidLinkage")]
        NavigationAidLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":PolygonLinkage")]
        PolygonLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":RestrictedAirspaceLinkage")]
        RestrictedAirspaceLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":RunwayLinkage")]
        RunwayLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":SidLinkage")]
        SidLinkage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":StarLinkage")]
        StarLinkage,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStatusResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamProposalStatusResponseType[] notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NotamProposal", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamProposalStatusResponseType[] NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum ActionType
    {

        /// <remarks/>
        Store,

        /// <remarks/>
        Modify,

        /// <remarks/>
        Delete,

        /// <remarks/>
        Validate,

        /// <remarks/>
        Query,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Query List")]
        QueryList,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStatusResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamProposalStatusResponseType notamProposalStatusField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalStatusResponseType NotamProposalStatus
        {
            get
            {
                return this.notamProposalStatusField;
            }
            set
            {
                this.notamProposalStatusField = value;
                this.RaisePropertyChanged("NotamProposalStatus");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class ResponseHeaderNotamType : ResponseHeaderType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseHeaderNotamType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class ResponseHeaderType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string versionField;

        private ResultType resultField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 0)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
                this.RaisePropertyChanged("Version");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public ResultType Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
                this.RaisePropertyChanged("Result");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public enum ResultType
    {

        /// <remarks/>
        OK,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("OK with Warnings")]
        OKwithWarnings,

        /// <remarks/>
        Error,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class ErrorType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ErrorTypeMsgType msgTypeField;

        private string msgFieldField;

        private string msgNumberField;

        private string msgTextField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ErrorTypeMsgType msgType
        {
            get
            {
                return this.msgTypeField;
            }
            set
            {
                this.msgTypeField = value;
                this.RaisePropertyChanged("msgType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string msgField
        {
            get
            {
                return this.msgFieldField;
            }
            set
            {
                this.msgFieldField = value;
                this.RaisePropertyChanged("msgField");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 2)]
        public string msgNumber
        {
            get
            {
                return this.msgNumberField;
            }
            set
            {
                this.msgNumberField = value;
                this.RaisePropertyChanged("msgNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string msgText
        {
            get
            {
                return this.msgTextField;
            }
            set
            {
                this.msgTextField = value;
                this.RaisePropertyChanged("msgText");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.teamead.com/ws/inodp")]
    public enum ErrorTypeMsgType
    {

        /// <remarks/>
        Error,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Information,

        /// <remarks/>
        Unknown,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SingleNotamProposalStatusType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string notamProposalIdentifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NotamProposalIdentifier
        {
            get
            {
                return this.notamProposalIdentifierField;
            }
            set
            {
                this.notamProposalIdentifierField = value;
                this.RaisePropertyChanged("NotamProposalIdentifier");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStatusType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamProposalStatusType[] notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NotamProposal", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamProposalStatusType[] NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStatusMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private transactionType transactionField;

        private RequestHeaderNotamType requestHeaderField;

        private NotamProposalStatusType notamProposalStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp", Order = 0)]
        public transactionType transaction
        {
            get
            {
                return this.transactionField;
            }
            set
            {
                this.transactionField = value;
                this.RaisePropertyChanged("transaction");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NotamProposalStatusType NotamProposalStatus
        {
            get
            {
                return this.notamProposalStatusField;
            }
            set
            {
                this.notamProposalStatusField = value;
                this.RaisePropertyChanged("NotamProposalStatus");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RequestHeaderNotamType : RequestHeaderType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestHeaderNotamType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class RequestHeaderType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 0)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
                this.RaisePropertyChanged("Version");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalRejectResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamProposalRejectBaseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalRejectBaseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalRejectBaseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string notamProposalIdentifierField;

        private string reasonField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NotamProposalIdentifier
        {
            get
            {
                return this.notamProposalIdentifierField;
            }
            set
            {
                this.notamProposalIdentifierField = value;
                this.RaisePropertyChanged("NotamProposalIdentifier");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
                this.RaisePropertyChanged("Reason");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalRejectResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamProposalRejectResponseType notamProposalRejectField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalRejectResponseType NotamProposalReject
        {
            get
            {
                return this.notamProposalRejectField;
            }
            set
            {
                this.notamProposalRejectField = value;
                this.RaisePropertyChanged("NotamProposalReject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalRejectType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamProposalRejectBaseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalRejectBaseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalRejectMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private transactionType transactionField;

        private RequestHeaderNotamType requestHeaderField;

        private NotamProposalRejectType notamProposalRejectField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp", Order = 0)]
        public transactionType transaction
        {
            get
            {
                return this.transactionField;
            }
            set
            {
                this.transactionField = value;
                this.RaisePropertyChanged("transaction");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NotamProposalRejectType NotamProposalReject
        {
            get
            {
                return this.notamProposalRejectField;
            }
            set
            {
                this.notamProposalRejectField = value;
                this.RaisePropertyChanged("NotamProposalReject");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalUpdateResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamProposalResponseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamProposalResponseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class SingleNotamProposalResponseType : NotamProposalBaseType
    {

        private string notamProposalIdentifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NotamProposalIdentifier
        {
            get
            {
                return this.notamProposalIdentifierField;
            }
            set
            {
                this.notamProposalIdentifierField = value;
                this.RaisePropertyChanged("NotamProposalIdentifier");
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SingleNotamProposalResponseType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalBaseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        private NotamTypeType typeField;

        private bool typeFieldSpecified;

        private string referredSeriesField;

        private string referredNumberField;

        private string referredYearField;

        private QLineProposalType qLineField;

        private string coordinatesField;

        private string radiusField;

        private string[] itemAField;

        private string startValidityField;

        private string endValidityField;

        private EstimationType estimationField;

        private bool estimationFieldSpecified;

        private string itemDField;

        private string itemEField;

        private MultiLanguageType multiLanguageField;

        private string itemFField;

        private string itemGField;

        private string itemXField;

        private string operatorField;

        private string accountableSourceField;

        private string notesToNofField;

        private string originatorField;

        private string organisationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 2)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 3)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public NotamTypeType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("Type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
                this.RaisePropertyChanged("TypeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string ReferredSeries
        {
            get
            {
                return this.referredSeriesField;
            }
            set
            {
                this.referredSeriesField = value;
                this.RaisePropertyChanged("ReferredSeries");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 6)]
        public string ReferredNumber
        {
            get
            {
                return this.referredNumberField;
            }
            set
            {
                this.referredNumberField = value;
                this.RaisePropertyChanged("ReferredNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 7)]
        public string ReferredYear
        {
            get
            {
                return this.referredYearField;
            }
            set
            {
                this.referredYearField = value;
                this.RaisePropertyChanged("ReferredYear");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public QLineProposalType QLine
        {
            get
            {
                return this.qLineField;
            }
            set
            {
                this.qLineField = value;
                this.RaisePropertyChanged("QLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
                this.RaisePropertyChanged("Coordinates");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 10)]
        public string Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
                this.RaisePropertyChanged("Radius");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemA", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
        public string[] ItemA
        {
            get
            {
                return this.itemAField;
            }
            set
            {
                this.itemAField = value;
                this.RaisePropertyChanged("ItemA");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        public string StartValidity
        {
            get
            {
                return this.startValidityField;
            }
            set
            {
                this.startValidityField = value;
                this.RaisePropertyChanged("StartValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        public string EndValidity
        {
            get
            {
                return this.endValidityField;
            }
            set
            {
                this.endValidityField = value;
                this.RaisePropertyChanged("EndValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        public EstimationType Estimation
        {
            get
            {
                return this.estimationField;
            }
            set
            {
                this.estimationField = value;
                this.RaisePropertyChanged("Estimation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimationSpecified
        {
            get
            {
                return this.estimationFieldSpecified;
            }
            set
            {
                this.estimationFieldSpecified = value;
                this.RaisePropertyChanged("EstimationSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        public string ItemD
        {
            get
            {
                return this.itemDField;
            }
            set
            {
                this.itemDField = value;
                this.RaisePropertyChanged("ItemD");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        public string ItemE
        {
            get
            {
                return this.itemEField;
            }
            set
            {
                this.itemEField = value;
                this.RaisePropertyChanged("ItemE");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
        public MultiLanguageType MultiLanguage
        {
            get
            {
                return this.multiLanguageField;
            }
            set
            {
                this.multiLanguageField = value;
                this.RaisePropertyChanged("MultiLanguage");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 18)]
        public string ItemF
        {
            get
            {
                return this.itemFField;
            }
            set
            {
                this.itemFField = value;
                this.RaisePropertyChanged("ItemF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 19)]
        public string ItemG
        {
            get
            {
                return this.itemGField;
            }
            set
            {
                this.itemGField = value;
                this.RaisePropertyChanged("ItemG");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 20)]
        public string ItemX
        {
            get
            {
                return this.itemXField;
            }
            set
            {
                this.itemXField = value;
                this.RaisePropertyChanged("ItemX");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 21)]
        public string Operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
                this.RaisePropertyChanged("Operator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 22)]
        public string AccountableSource
        {
            get
            {
                return this.accountableSourceField;
            }
            set
            {
                this.accountableSourceField = value;
                this.RaisePropertyChanged("AccountableSource");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 23)]
        public string NotesToNof
        {
            get
            {
                return this.notesToNofField;
            }
            set
            {
                this.notesToNofField = value;
                this.RaisePropertyChanged("NotesToNof");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 24)]
        public string Originator
        {
            get
            {
                return this.originatorField;
            }
            set
            {
                this.originatorField = value;
                this.RaisePropertyChanged("Originator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 25)]
        public string Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
                this.RaisePropertyChanged("Organisation");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class QLineProposalType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fIRField;

        private string code23Field;

        private string code45Field;

        private TrafficType trafficField;

        private bool trafficFieldSpecified;

        private PurposeType purposeField;

        private bool purposeFieldSpecified;

        private ScopeType scopeField;

        private bool scopeFieldSpecified;

        private string lowerField;

        private string upperField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string FIR
        {
            get
            {
                return this.fIRField;
            }
            set
            {
                this.fIRField = value;
                this.RaisePropertyChanged("FIR");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Code23
        {
            get
            {
                return this.code23Field;
            }
            set
            {
                this.code23Field = value;
                this.RaisePropertyChanged("Code23");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string Code45
        {
            get
            {
                return this.code45Field;
            }
            set
            {
                this.code45Field = value;
                this.RaisePropertyChanged("Code45");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public TrafficType Traffic
        {
            get
            {
                return this.trafficField;
            }
            set
            {
                this.trafficField = value;
                this.RaisePropertyChanged("Traffic");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TrafficSpecified
        {
            get
            {
                return this.trafficFieldSpecified;
            }
            set
            {
                this.trafficFieldSpecified = value;
                this.RaisePropertyChanged("TrafficSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public PurposeType Purpose
        {
            get
            {
                return this.purposeField;
            }
            set
            {
                this.purposeField = value;
                this.RaisePropertyChanged("Purpose");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PurposeSpecified
        {
            get
            {
                return this.purposeFieldSpecified;
            }
            set
            {
                this.purposeFieldSpecified = value;
                this.RaisePropertyChanged("PurposeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public ScopeType Scope
        {
            get
            {
                return this.scopeField;
            }
            set
            {
                this.scopeField = value;
                this.RaisePropertyChanged("Scope");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ScopeSpecified
        {
            get
            {
                return this.scopeFieldSpecified;
            }
            set
            {
                this.scopeFieldSpecified = value;
                this.RaisePropertyChanged("ScopeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 6)]
        public string Lower
        {
            get
            {
                return this.lowerField;
            }
            set
            {
                this.lowerField = value;
                this.RaisePropertyChanged("Lower");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 7)]
        public string Upper
        {
            get
            {
                return this.upperField;
            }
            set
            {
                this.upperField = value;
                this.RaisePropertyChanged("Upper");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalUpdateResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamProposalUpdateResponseType notamProposalUpdateField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalUpdateResponseType NotamProposalUpdate
        {
            get
            {
                return this.notamProposalUpdateField;
            }
            set
            {
                this.notamProposalUpdateField = value;
                this.RaisePropertyChanged("NotamProposalUpdate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalUpdateBaseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string notamProposalIdentifierField;

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        private NotamTypeType typeField;

        private bool typeFieldSpecified;

        private string referredSeriesField;

        private string referredNumberField;

        private string referredYearField;

        private QLineProposalType qLineField;

        private string coordinatesField;

        private string radiusField;

        private string[] itemAField;

        private string startValidityField;

        private string endValidityField;

        private EstimationType estimationField;

        private bool estimationFieldSpecified;

        private string itemDField;

        private string itemEField;

        private MultiLanguageType multiLanguageField;

        private string itemFField;

        private string itemGField;

        private string itemXField;

        private string operatorField;

        private string accountableSourceField;

        private string notesToNofField;

        private string originatorField;

        private string organisationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NotamProposalIdentifier
        {
            get
            {
                return this.notamProposalIdentifierField;
            }
            set
            {
                this.notamProposalIdentifierField = value;
                this.RaisePropertyChanged("NotamProposalIdentifier");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 3)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 4)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public NotamTypeType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("Type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
                this.RaisePropertyChanged("TypeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
        public string ReferredSeries
        {
            get
            {
                return this.referredSeriesField;
            }
            set
            {
                this.referredSeriesField = value;
                this.RaisePropertyChanged("ReferredSeries");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 7)]
        public string ReferredNumber
        {
            get
            {
                return this.referredNumberField;
            }
            set
            {
                this.referredNumberField = value;
                this.RaisePropertyChanged("ReferredNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 8)]
        public string ReferredYear
        {
            get
            {
                return this.referredYearField;
            }
            set
            {
                this.referredYearField = value;
                this.RaisePropertyChanged("ReferredYear");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public QLineProposalType QLine
        {
            get
            {
                return this.qLineField;
            }
            set
            {
                this.qLineField = value;
                this.RaisePropertyChanged("QLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
                this.RaisePropertyChanged("Coordinates");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 11)]
        public string Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
                this.RaisePropertyChanged("Radius");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemA", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        public string[] ItemA
        {
            get
            {
                return this.itemAField;
            }
            set
            {
                this.itemAField = value;
                this.RaisePropertyChanged("ItemA");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        public string StartValidity
        {
            get
            {
                return this.startValidityField;
            }
            set
            {
                this.startValidityField = value;
                this.RaisePropertyChanged("StartValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        public string EndValidity
        {
            get
            {
                return this.endValidityField;
            }
            set
            {
                this.endValidityField = value;
                this.RaisePropertyChanged("EndValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        public EstimationType Estimation
        {
            get
            {
                return this.estimationField;
            }
            set
            {
                this.estimationField = value;
                this.RaisePropertyChanged("Estimation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimationSpecified
        {
            get
            {
                return this.estimationFieldSpecified;
            }
            set
            {
                this.estimationFieldSpecified = value;
                this.RaisePropertyChanged("EstimationSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        public string ItemD
        {
            get
            {
                return this.itemDField;
            }
            set
            {
                this.itemDField = value;
                this.RaisePropertyChanged("ItemD");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
        public string ItemE
        {
            get
            {
                return this.itemEField;
            }
            set
            {
                this.itemEField = value;
                this.RaisePropertyChanged("ItemE");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 18)]
        public MultiLanguageType MultiLanguage
        {
            get
            {
                return this.multiLanguageField;
            }
            set
            {
                this.multiLanguageField = value;
                this.RaisePropertyChanged("MultiLanguage");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 19)]
        public string ItemF
        {
            get
            {
                return this.itemFField;
            }
            set
            {
                this.itemFField = value;
                this.RaisePropertyChanged("ItemF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 20)]
        public string ItemG
        {
            get
            {
                return this.itemGField;
            }
            set
            {
                this.itemGField = value;
                this.RaisePropertyChanged("ItemG");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 21)]
        public string ItemX
        {
            get
            {
                return this.itemXField;
            }
            set
            {
                this.itemXField = value;
                this.RaisePropertyChanged("ItemX");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 22)]
        public string Operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
                this.RaisePropertyChanged("Operator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 23)]
        public string AccountableSource
        {
            get
            {
                return this.accountableSourceField;
            }
            set
            {
                this.accountableSourceField = value;
                this.RaisePropertyChanged("AccountableSource");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 24)]
        public string NotesToNof
        {
            get
            {
                return this.notesToNofField;
            }
            set
            {
                this.notesToNofField = value;
                this.RaisePropertyChanged("NotesToNof");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 25)]
        public string Originator
        {
            get
            {
                return this.originatorField;
            }
            set
            {
                this.originatorField = value;
                this.RaisePropertyChanged("Originator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 26)]
        public string Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
                this.RaisePropertyChanged("Organisation");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalUpdateType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamProposalUpdateBaseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalUpdateBaseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalUpdateMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private transactionType transactionField;

        private RequestHeaderNotamType requestHeaderField;

        private NotamProposalUpdateType notamProposalUpdateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp", Order = 0)]
        public transactionType transaction
        {
            get
            {
                return this.transactionField;
            }
            set
            {
                this.transactionField = value;
                this.RaisePropertyChanged("transaction");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NotamProposalUpdateType NotamProposalUpdate
        {
            get
            {
                return this.notamProposalUpdateField;
            }
            set
            {
                this.notamProposalUpdateField = value;
                this.RaisePropertyChanged("NotamProposalUpdate");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStoreResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamProposalResponseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamProposalResponseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStoreResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamProposalStoreResponseType notamProposalStoreField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalStoreResponseType NotamProposalStore
        {
            get
            {
                return this.notamProposalStoreField;
            }
            set
            {
                this.notamProposalStoreField = value;
                this.RaisePropertyChanged("NotamProposalStore");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStoreType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamProposalBaseType notamProposalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamProposalBaseType NotamProposal
        {
            get
            {
                return this.notamProposalField;
            }
            set
            {
                this.notamProposalField = value;
                this.RaisePropertyChanged("NotamProposal");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamProposalStoreMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private transactionType transactionField;

        private RequestHeaderNotamType requestHeaderField;

        private NotamProposalStoreType notamProposalStoreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.teamead.com/ws/aimsl/dp", Order = 0)]
        public transactionType transaction
        {
            get
            {
                return this.transactionField;
            }
            set
            {
                this.transactionField = value;
                this.RaisePropertyChanged("transaction");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public NotamProposalStoreType NotamProposalStore
        {
            get
            {
                return this.notamProposalStoreField;
            }
            set
            {
                this.notamProposalStoreField = value;
                this.RaisePropertyChanged("NotamProposalStore");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        private NotamTypeType typeField;

        private string referredSeriesField;

        private string referredNumberField;

        private string referredYearField;

        private QLineType qLineField;

        private string coordinatesField;

        private string radiusField;

        private string[] itemAField;

        private string startValidityField;

        private string endValidityField;

        private EstimationType estimationField;

        private bool estimationFieldSpecified;

        private string itemDField;

        private string itemEField;

        private string itemFField;

        private string itemGField;

        private string itemXField;

        private string operatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 2)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 3)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public NotamTypeType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("Type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string ReferredSeries
        {
            get
            {
                return this.referredSeriesField;
            }
            set
            {
                this.referredSeriesField = value;
                this.RaisePropertyChanged("ReferredSeries");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 6)]
        public string ReferredNumber
        {
            get
            {
                return this.referredNumberField;
            }
            set
            {
                this.referredNumberField = value;
                this.RaisePropertyChanged("ReferredNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 7)]
        public string ReferredYear
        {
            get
            {
                return this.referredYearField;
            }
            set
            {
                this.referredYearField = value;
                this.RaisePropertyChanged("ReferredYear");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public QLineType QLine
        {
            get
            {
                return this.qLineField;
            }
            set
            {
                this.qLineField = value;
                this.RaisePropertyChanged("QLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public string Coordinates
        {
            get
            {
                return this.coordinatesField;
            }
            set
            {
                this.coordinatesField = value;
                this.RaisePropertyChanged("Coordinates");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "nonNegativeInteger", Order = 10)]
        public string Radius
        {
            get
            {
                return this.radiusField;
            }
            set
            {
                this.radiusField = value;
                this.RaisePropertyChanged("Radius");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemA", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
        public string[] ItemA
        {
            get
            {
                return this.itemAField;
            }
            set
            {
                this.itemAField = value;
                this.RaisePropertyChanged("ItemA");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        public string StartValidity
        {
            get
            {
                return this.startValidityField;
            }
            set
            {
                this.startValidityField = value;
                this.RaisePropertyChanged("StartValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        public string EndValidity
        {
            get
            {
                return this.endValidityField;
            }
            set
            {
                this.endValidityField = value;
                this.RaisePropertyChanged("EndValidity");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        public EstimationType Estimation
        {
            get
            {
                return this.estimationField;
            }
            set
            {
                this.estimationField = value;
                this.RaisePropertyChanged("Estimation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EstimationSpecified
        {
            get
            {
                return this.estimationFieldSpecified;
            }
            set
            {
                this.estimationFieldSpecified = value;
                this.RaisePropertyChanged("EstimationSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        public string ItemD
        {
            get
            {
                return this.itemDField;
            }
            set
            {
                this.itemDField = value;
                this.RaisePropertyChanged("ItemD");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        public string ItemE
        {
            get
            {
                return this.itemEField;
            }
            set
            {
                this.itemEField = value;
                this.RaisePropertyChanged("ItemE");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
        public string ItemF
        {
            get
            {
                return this.itemFField;
            }
            set
            {
                this.itemFField = value;
                this.RaisePropertyChanged("ItemF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 18)]
        public string ItemG
        {
            get
            {
                return this.itemGField;
            }
            set
            {
                this.itemGField = value;
                this.RaisePropertyChanged("ItemG");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 19)]
        public string ItemX
        {
            get
            {
                return this.itemXField;
            }
            set
            {
                this.itemXField = value;
                this.RaisePropertyChanged("ItemX");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 20)]
        public string Operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
                this.RaisePropertyChanged("Operator");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamChecklistResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamType[] notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Notam", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamType[] Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamChecklistResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamChecklistResponseType notamChecklistField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamChecklistResponseType NotamChecklist
        {
            get
            {
                return this.notamChecklistField;
            }
            set
            {
                this.notamChecklistField = value;
                this.RaisePropertyChanged("NotamChecklist");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamChecklistType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class CheckType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamChecklistMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamChecklistType notamChecklistField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamChecklistType NotamChecklist
        {
            get
            {
                return this.notamChecklistField;
            }
            set
            {
                this.notamChecklistField = value;
                this.RaisePropertyChanged("NotamChecklist");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamEstResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamEstResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamEstResponseType notamEstField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamEstResponseType NotamEst
        {
            get
            {
                return this.notamEstField;
            }
            set
            {
                this.notamEstField = value;
                this.RaisePropertyChanged("NotamEst");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamEstType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamEstMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamEstType notamEstField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamEstType NotamEst
        {
            get
            {
                return this.notamEstField;
            }
            set
            {
                this.notamEstField = value;
                this.RaisePropertyChanged("NotamEst");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamListPartElementType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nOFField;

        private string seriesField;

        private string numberField;

        private string yearField;

        private string partField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NOF
        {
            get
            {
                return this.nOFField;
            }
            set
            {
                this.nOFField = value;
                this.RaisePropertyChanged("NOF");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
                this.RaisePropertyChanged("Series");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "positiveInteger", Order = 2)]
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                this.RaisePropertyChanged("Number");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer", Order = 3)]
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
                this.RaisePropertyChanged("Year");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string Part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
                this.RaisePropertyChanged("Part");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingPartResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private NotamListPartElementType[] notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Notam", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamListPartElementType[] Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingPartResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamMissingPartResponseType notamMissingPartField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamMissingPartResponseType NotamMissingPart
        {
            get
            {
                return this.notamMissingPartField;
            }
            set
            {
                this.notamMissingPartField = value;
                this.RaisePropertyChanged("NotamMissingPart");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingPartType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingPartMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamMissingPartType notamMissingPartField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamMissingPartType NotamMissingPart
        {
            get
            {
                return this.notamMissingPartField;
            }
            set
            {
                this.notamMissingPartField = value;
                this.RaisePropertyChanged("NotamMissingPart");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingResponseMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ResponseHeaderNotamType responseHeaderField;

        private NotamMissingResponseType notamMissingField;

        private ErrorType[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ResponseHeaderNotamType ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
                this.RaisePropertyChanged("ResponseHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamMissingResponseType NotamMissing
        {
            get
            {
                return this.notamMissingField;
            }
            set
            {
                this.notamMissingField = value;
                this.RaisePropertyChanged("NotamMissing");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Error", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ErrorType[] Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
                this.RaisePropertyChanged("Errors");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private CheckType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CheckType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamMissingMessageType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private RequestHeaderNotamType requestHeaderField;

        private NotamMissingType notamMissingField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public RequestHeaderNotamType RequestHeader
        {
            get
            {
                return this.requestHeaderField;
            }
            set
            {
                this.requestHeaderField = value;
                this.RaisePropertyChanged("RequestHeader");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public NotamMissingType NotamMissing
        {
            get
            {
                return this.notamMissingField;
            }
            set
            {
                this.notamMissingField = value;
                this.RaisePropertyChanged("NotamMissing");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.teamead.com/ws/inodp")]
    public partial class NotamQueryResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private ActionType actionField;

        private SingleNotamQueryType notamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public ActionType Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("Action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public SingleNotamQueryType Notam
        {
            get
            {
                return this.notamField;
            }
            set
            {
                this.notamField = value;
                this.RaisePropertyChanged("Notam");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
