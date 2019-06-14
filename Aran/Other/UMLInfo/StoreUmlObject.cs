using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ParseMDL
{
    public class StoreUmlObject
    {
        private List<byte> _byteList;
        private byte [] _readBytes;
        private int _readIndex;

        public StoreUmlObject ()
        {
            _byteList = null;
            _readBytes = null;
            _readIndex = 0;
        }

        public void SaveObject (UmlObject umlObject, string fileName)
        {
            _byteList = new List<byte> ();
            PutUmlObject (umlObject);
            FileStream stream = new FileStream (fileName, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Write (_byteList.ToArray (), 0, _byteList.Count);
            stream.Close ();
        }

        public UmlObject LoadObject (string fileName)
        {
            if (!File.Exists (fileName))
                throw new Exception ("File not exists");

            FileStream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read);
            _readBytes = new byte [stream.Length];
            stream.Read (_readBytes, 0, _readBytes.Length);
            stream.Close ();

            return GetUmlObject ();
        }

        #region Put/Get

        private void PutInt16 (Int16 value)
        {
            byte [] ba = BitConverter.GetBytes (value);
            _byteList.AddRange (ba);
        }

        private Int16 GetInt16 ()
        {
            Int16 value = BitConverter.ToInt16 (_readBytes, _readIndex);
            _readIndex += 2;
            return value;
        }

        private void PutInt32 (Int32 value)
        {
            byte [] ba = BitConverter.GetBytes (value);
            _byteList.AddRange (ba);
        }

        private Int32 GetInt32 ()
        {
            Int32 value = BitConverter.ToInt32 (_readBytes, _readIndex);
            _readIndex += 4;
            return value;
        }

        private void PutString (string value)
        {
            int lenth = (value == null ? -1 : value.Length);
            PutInt32 (lenth);

            for (int i = 0; i < lenth; i++)
            {
                PutInt16 ((short) value [i]);
            }
        }

        private string GetString ()
        {
            int length = GetInt32 ();
            if (length == -1)
                return null;

            char [] ca = new char [length];
            for (int i = 0; i < length; i++)
            {
                ca [i] = (char) GetInt16 ();
            }
            return new string (ca);
        }

        private void PutUmlList (UmlList umlList)
        {
            PutString (umlList.Type);
            PutInt32 (umlList.Count);

            foreach (IUmlListItem item in umlList)
            {
                PutUmlListItem (item);
            }
        }

        private UmlList GetUmlList ()
        {
            UmlList umlList = new UmlList ();
            umlList.Type = GetString ();

            int len = GetInt32 ();
            for (int i = 0; i < len; i++)
            {
                umlList.Add (GetUmlListItem ());
            }

            return umlList;
        }

        private void PutUmlListItem (IUmlListItem value)
        {
            PutInt32 ((int) value.ItemType);

            if (value.ItemType == ListItemType.UmlString)
            {
                PutUmlStringItem ((UmlStringItem) value);
            }
            else
            {
                PutUmlObject ((UmlObject) value);
            }
        }

        private IUmlListItem GetUmlListItem ()
        {
            ListItemType itemType = (ListItemType) GetInt32 ();
            if (itemType == ListItemType.UmlString)
            {
                return GetUmlStringItem ();
            }
            else
            {
                return GetUmlObject ();
            }
        }

        private void PutUmlPropertyValue (IUmlPropertyValue value)
        {
            PutInt32 ((int) value.PropertyType);
            if (value.PropertyType == UmlPropertyType.UmlList)
                PutUmlList ((UmlList) value);
            else if (value.PropertyType == UmlPropertyType.UmlObject)
                PutUmlObject ((UmlObject) value);
            else //if (value.PropertyType == UmlPropertyType.UmlString)
                PutUmlStringItem ((UmlStringItem) value);
        }

        private IUmlPropertyValue GetUmlPropertyValue ()
        {
            UmlPropertyType propType = (UmlPropertyType) GetInt32 ();
            if (propType == UmlPropertyType.Null)
                return new UmlPropertyNull ();
            else if (propType == UmlPropertyType.UmlList)
                return GetUmlList ();
            else if (propType == UmlPropertyType.UmlObject)
                return GetUmlObject ();
            else //if (propType == UmlPropertyType.UmlString)
                return GetUmlStringItem ();
        }

        private void PutUmlObject (UmlObject value)
        {
            PutString (value.Type);
            PutString (value.Name);
            PutInt32 (value.Propreties.Count);

            foreach (UmlProperty item in value.Propreties)
            {
                PutUmlProperty (item);
            }
        }

        private UmlObject GetUmlObject ()
        {
            UmlObject umlObject = new UmlObject ();
            umlObject.Type = GetString ();
            umlObject.Name = GetString ();

            int len = GetInt32 ();
            for (int i = 0; i < len; i++)
            {
                umlObject.Propreties.Add (GetUmlProperty ());
            }

            return umlObject;
        }

        private void PutUmlProperty (UmlProperty value)
        {
            PutString (value.Name);
            PutUmlPropertyValue (value.Value);
        }

        private UmlProperty GetUmlProperty ()
        {
            UmlProperty umlProp = new UmlProperty ();
            umlProp.Name = GetString ();
            umlProp.Value = GetUmlPropertyValue ();
            return umlProp;
        }

        private void PutUmlStringItem (UmlStringItem value)
        {
            PutString (value.Value);
        }

        private UmlStringItem GetUmlStringItem ()
        {
            return new UmlStringItem (GetString ());
        }

        #endregion
    }
}
