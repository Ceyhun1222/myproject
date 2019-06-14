using System;
using System.Collections.Generic;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace VisualizerEnvironment.Util
{
    public class LicenseInitializer
    {
        public static LicenseInitializer Instance=new LicenseInitializer();

        /// <summary>
        /// Raised when ArcGIS runtime binding hasn't been established. 
        /// </summary>
        public event EventHandler ResolveBindingEvent;

        /// <summary>
        /// Initialize the application with the specified product and extension license code.
        /// </summary>    
        /// <returns>Initialization is successful.</returns>
        /// <remarks>
        /// Make sure an active ArcGIS runtime has been bound before license initialization.
        /// </remarks>
        public bool InitializeApplication(esriLicenseProductCode[] productCodes, esriLicenseExtensionCode[] extensionLics)
        {
            //Cache product codes by enum int so can be sorted without custom sorter
            _mRequestedProducts = new List<int>();
            foreach (var code in productCodes)
            {
                var requestCodeNum = Convert.ToInt32(code);
                if (!_mRequestedProducts.Contains(requestCodeNum))
                {
                    _mRequestedProducts.Add(requestCodeNum);
                }
            }
            AddExtensions(extensionLics);

            // Make sure an active runtime has been bound before calling any ArcObjects code. 
            if (RuntimeManager.ActiveRuntime == null)
            {
                EventHandler temp = ResolveBindingEvent;
                if (temp != null)
                {
                    temp(this, new EventArgs());
                }
            }

            return Initialize();
        }

        /// <summary>
        /// A summary of the status of product and extensions initialization.
        /// </summary>
        public string LicenseMessage()
        {
            if (RuntimeManager.ActiveRuntime == null)
            {
                return MessageNoRuntimeBinding;
            }

            if (_mAoInit == null)
            {
                return MessageNoAoInitialize;
            }

            string prodStatus = string.Empty;
            string extStatus = string.Empty;
            if (_mProductStatus == null || _mProductStatus.Count == 0)
            {
                prodStatus = MessageNoLicensesRequested + Environment.NewLine;
            }
            else if (_mProductStatus.ContainsValue(esriLicenseStatus.esriLicenseAlreadyInitialized)
                || _mProductStatus.ContainsValue(esriLicenseStatus.esriLicenseCheckedOut))
            {
                prodStatus = ReportInformation(_mAoInit as ILicenseInformation,
                    _mAoInit.InitializedProduct(),
                    esriLicenseStatus.esriLicenseCheckedOut) + Environment.NewLine;
            }
            else
            {
                //Failed...
                foreach (KeyValuePair<esriLicenseProductCode, esriLicenseStatus> item in _mProductStatus)
                {
                    prodStatus += ReportInformation(_mAoInit as ILicenseInformation,
                        item.Key, item.Value) + Environment.NewLine;
                }
            }

            foreach (KeyValuePair<esriLicenseExtensionCode, esriLicenseStatus> item in _mExtensionStatus)
            {
                string info = ReportInformation(_mAoInit as ILicenseInformation, item.Key, item.Value);
                if (!string.IsNullOrEmpty(info))
                    extStatus += info + Environment.NewLine;
            }

            return (prodStatus + extStatus).Trim();
        }

        /// <summary>
        /// Shuts down AoInitialize object and check back in extensions to ensure
        /// any ESRI libraries that have been used are unloaded in the correct order.
        /// </summary>
        /// <remarks>Once Shutdown has been called, you cannot re-initialize the product license
        /// and should not make any ArcObjects call.</remarks>
        public void ShutdownApplication()
        {
            if (_mHasShutDown)
                return;

            //Check back in extensions
            if (_mAoInit != null)
            {
                foreach (KeyValuePair<esriLicenseExtensionCode, esriLicenseStatus> item in _mExtensionStatus)
                {
                    if (item.Value == esriLicenseStatus.esriLicenseCheckedOut)
                        _mAoInit.CheckInExtension(item.Key);
                }
                _mAoInit.Shutdown();
            }

            _mRequestedProducts = null;
            _mRequestedExtensions = null;
            _mExtensionStatus = null;
            _mProductStatus = null;

            _mHasShutDown = true;
        }

        /// <summary>
        /// Indicates if the extension is currently checked out.
        /// </summary>
        public bool IsExtensionCheckedOut(esriLicenseExtensionCode code)
        {
            return _mAoInit != null && _mAoInit.IsExtensionCheckedOut(code);
        }

        /// <summary>
        /// Set the extension(s) to be checked out for your ArcObjects code. 
        /// </summary>
        public bool AddExtensions(params esriLicenseExtensionCode[] requestCodes)
        {
            if (_mRequestedExtensions == null)
                _mRequestedExtensions = new List<esriLicenseExtensionCode>();
            foreach (var code in requestCodes)
            {
                if (!_mRequestedExtensions.Contains(code))
                    _mRequestedExtensions.Add(code);
            }

            return _mHasInitializeProduct && CheckOutLicenses(InitializedProduct);
        }

        /// <summary>
        /// Check in extension(s) when it is no longer needed.
        /// </summary>
        public void RemoveExtensions(params esriLicenseExtensionCode[] requestCodes)
        {
            if (_mExtensionStatus == null || _mExtensionStatus.Count == 0)
                return;

            foreach (var code in requestCodes)
            {
                if (!_mExtensionStatus.ContainsKey(code)) continue;

                if (_mAoInit.CheckInExtension(code) == esriLicenseStatus.esriLicenseCheckedIn)
                {
                    _mExtensionStatus[code] = esriLicenseStatus.esriLicenseCheckedIn;
                }
            }
        }

        /// <summary>
        /// Get/Set the ordering of product code checking. If true, check from lowest to 
        /// highest license. True by default.
        /// </summary>
        public bool InitializeLowerProductFirst
        {
            get
            {
                return _mProductCheckOrdering;
            }
            set
            {
                _mProductCheckOrdering = value;
            }
        }

        /// <summary>
        /// Retrieves the product code initialized in the ArcObjects application
        /// </summary>
        public esriLicenseProductCode InitializedProduct
        {
            get
            {
                if (_mAoInit != null)
                {
                    try
                    {
                        return _mAoInit.InitializedProduct();
                    }
                    catch
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }

        #region Helper methods

        private bool Initialize()
        {
            if (RuntimeManager.ActiveRuntime == null)
                return false;

            if (_mRequestedProducts == null || _mRequestedProducts.Count == 0)
                return false;

            var productInitialized = false;

            _mRequestedProducts.Sort();
            if (!InitializeLowerProductFirst) //Request license from highest to lowest
                _mRequestedProducts.Reverse();

            _mAoInit = new AoInitializeClass();
            var currentProduct = new esriLicenseProductCode();
            foreach (int prodNumber in _mRequestedProducts)
            {
                var prod = (esriLicenseProductCode)System.Enum.ToObject(typeof(esriLicenseProductCode), prodNumber);
                esriLicenseStatus status = _mAoInit.IsProductCodeAvailable(prod);
                if (status == esriLicenseStatus.esriLicenseAvailable)
                {
                    status = _mAoInit.Initialize(prod);
                    if (status == esriLicenseStatus.esriLicenseAlreadyInitialized ||
                        status == esriLicenseStatus.esriLicenseCheckedOut)
                    {
                        productInitialized = true;
                        currentProduct = _mAoInit.InitializedProduct();
                    }
                }

                _mProductStatus.Add(prod, status);

                if (productInitialized)
                    break;
            }

            _mHasInitializeProduct = productInitialized;
            _mRequestedProducts.Clear();

            //No product is initialized after trying all requested licenses, quit
            if (!productInitialized)
            {
                return false;
            }

            //Check out extension licenses
            return CheckOutLicenses(currentProduct);
        }

        private bool CheckOutLicenses(esriLicenseProductCode currentProduct)
        {
            var allSuccessful = true;
            //Request extensions
            if (_mRequestedExtensions != null && currentProduct != 0)
            {
                foreach (esriLicenseExtensionCode ext in _mRequestedExtensions)
                {
                    esriLicenseStatus licenseStatus = _mAoInit.IsExtensionCodeAvailable(currentProduct, ext);
                    if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)//skip unavailable extensions
                    {
                        licenseStatus = _mAoInit.CheckOutExtension(ext);
                    }
                    allSuccessful = (allSuccessful && licenseStatus == esriLicenseStatus.esriLicenseCheckedOut);
                    if (_mExtensionStatus.ContainsKey(ext))
                        _mExtensionStatus[ext] = licenseStatus;
                    else
                        _mExtensionStatus.Add(ext, licenseStatus);
                }

                _mRequestedExtensions.Clear();
            }

            return allSuccessful;
        }


        private static string ReportInformation(ILicenseInformation licInfo, esriLicenseProductCode code, esriLicenseStatus status)
        {
            string prodName = string.Empty;
            try
            {
                prodName = licInfo.GetLicenseProductName(code);
            }
            catch
            {
                prodName = code.ToString();
            }

            var statusInfo = string.Empty;

            switch (status)
            {
                case esriLicenseStatus.esriLicenseAlreadyInitialized:
                case esriLicenseStatus.esriLicenseCheckedOut:
                    statusInfo = string.Format(MessageProductAvailable, prodName);
                    break;
                default:
                    statusInfo = string.Format(MessageProductNotLicensed, prodName);
                    break;
            }

            return statusInfo;
        }
        private static string ReportInformation(ILicenseInformation licInfo, esriLicenseExtensionCode code, esriLicenseStatus status)
        {
            var extensionName = string.Empty;
            try
            {
                extensionName = licInfo.GetLicenseExtensionName(code);
            }
            catch
            {
                extensionName = code.ToString();
            }

            var statusInfo = string.Empty;

            switch (status)
            {
                case esriLicenseStatus.esriLicenseAlreadyInitialized:
                case esriLicenseStatus.esriLicenseCheckedOut:
                    statusInfo = string.Format(MessageExtensionAvailable, extensionName);
                    break;
                case esriLicenseStatus.esriLicenseCheckedIn:
                    break;
                case esriLicenseStatus.esriLicenseUnavailable:
                    statusInfo = string.Format(MessageExtensionUnavailable, extensionName);
                    break;
                case esriLicenseStatus.esriLicenseFailure:
                    statusInfo = string.Format(MessageExtensionFailed, extensionName);
                    break;
                default:
                    statusInfo = string.Format(MessageExtensionNotLicensed, extensionName);
                    break;
            }

            return statusInfo;
        }
        #endregion

        private const string MessageNoRuntimeBinding = "Invalid ArcGIS runtime binding.";
        private const string MessageNoAoInitialize = "ArcObjects initialization failed.";
        private const string MessageNoLicensesRequested = "Product: No licenses were requested";
        private const string MessageProductAvailable = "Product: {0}: Available";
        private const string MessageProductNotLicensed = "Product: {0}: Not Licensed";
        private const string MessageExtensionAvailable = " Extension: {0}: Available";
        private const string MessageExtensionNotLicensed = " Extension: {0}: Not Licensed";
        private const string MessageExtensionFailed = " Extension: {0}: Failed";
        private const string MessageExtensionUnavailable = " Extension: {0}: Unavailable";

        #region Private members

        private IAoInitialize _mAoInit;
        private bool _mHasShutDown;
        private bool _mHasInitializeProduct;

        private List<int> _mRequestedProducts;
        private List<esriLicenseExtensionCode> _mRequestedExtensions;
        private Dictionary<esriLicenseProductCode, esriLicenseStatus> _mProductStatus = new Dictionary<esriLicenseProductCode, esriLicenseStatus>();
        private Dictionary<esriLicenseExtensionCode, esriLicenseStatus> _mExtensionStatus = new Dictionary<esriLicenseExtensionCode, esriLicenseStatus>();

        private bool _mProductCheckOrdering = true; //default from low to high
        #endregion
    
        private LicenseInitializer()
        {
            ResolveBindingEvent += BindingArcGisRuntime;
        }

        static void BindingArcGisRuntime(object sender, EventArgs e)
        {
            if (RuntimeManager.Bind(ProductCode.Desktop)) return;
            // Failed to bind, announce and force exit
            Console.WriteLine("Invalid ArcGIS runtime binding. Application will shut down.");
            Environment.Exit(0);
        }
    }
}
