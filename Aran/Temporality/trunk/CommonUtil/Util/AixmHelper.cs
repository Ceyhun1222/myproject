using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using System.Xml;
using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Other;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Util;

namespace Aran.Temporality.CommonUtil.Util
{
    public class AixmHelper
    {
        public string FileName;
        public bool IsOpened;

        public void Open(string xmlPath, Action onFirst, Action onMiddle,
            Action<AixmFeatureList, ObservableCollection<AixmFeatureList>> onNext)
        {
            FileName = xmlPath;
            IsOpened = false;

            if (File.Exists(FileName))
            {
                //form featTypeDict at 1 time
                var featTypeDict = new SortedList<Guid, FeatureType>();



                using (var xmlReader = XmlReader.Create(FileName))
                {
                    var aixmBasicMess1 = new AixmBasicMessage(MessageReceiverType.Panda);
                    aixmBasicMess1.ReadXmlAndNotify(xmlReader, (afl, collection) =>
                    {
                        foreach (var feature in afl)
                        {
                            if (!featTypeDict.ContainsKey(feature.Identifier))
                            {
                                featTypeDict.Add(feature.Identifier, feature.FeatureType);
                            }
                        }
                        collection.Clear(); //do not keep in memory 
                        if (onFirst != null)
                        {
                            onFirst();
                        }
                    });
                }

                if (onMiddle != null)
                {
                    onMiddle();
                }
                MemoryUtil.CompactLoh();
                GC.WaitForPendingFinalizers();

                //now read at 2 time
                using (var xmlReader = XmlReader.Create(FileName))
                {
                    var aixmBasicMess1 = new AixmBasicMessage(MessageReceiverType.Panda);
                    AbstractFeatureRefTypeReadingHandle.Handle = absFeat =>
                    {
                        IAbstractFeatureRef afr = absFeat;
                        FeatureType featType;
                        if (featTypeDict.TryGetValue(afr.Identifier, out featType))
                        {
                            afr.FeatureTypeIndex = (int) featType;
                        }
                    };
                    aixmBasicMess1.ReadXmlAndNotify(xmlReader, onNext);
                    AbstractFeatureRefTypeReadingHandle.Handle = null;
                }
                IsOpened = true;
            }
        }
    }
}
