namespace Holding
{
    public abstract class Changed:IChanged
    {
        private bool _modelChanged;
     
        public bool ModelChanged
        {
            get { return _modelChanged; }
            set 
            {
                _modelChanged = value;
                if (ModelChangedEventHandler != null)
                    ModelChangedEventHandler(this, new ModelChangedEventArgs(value));
            }

        }

        public bool IsApply { get; set; }
      
        protected void ChangeModelChanged(object oldValue, object newValue, object applValue)
        {
            if ((oldValue==null) && (newValue==null) || applValue==null)
                return;

            if (oldValue==null || oldValue.Equals(applValue) && ( newValue==null || !newValue.Equals(applValue)))
            {
                if (ModelChangedEventHandler != null)
                    ModelChangedEventHandler(this, new ModelChangedEventArgs(true));
            }
            else
            if ((oldValue!=null && !oldValue.Equals(applValue)) && (newValue!=null && newValue.Equals(applValue)))
            {
                if (ModelChangedEventHandler != null)
                    ModelChangedEventHandler(this, new ModelChangedEventArgs(false));
            }
        }

        public abstract void SetApplyParams();
        
        public event ModelChangedEventHandler ModelChangedEventHandler;


    }
}