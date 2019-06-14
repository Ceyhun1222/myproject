using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DB;
using Aran.Aim.Objects;
using System.Collections;
using System.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Data
{
	public class ComplexLoaderDataListener : IDataListener
	{
		public ComplexLoaderDataListener ()
		{
		}

		#region IDataListener members

		public bool GetObject (AObject aObject, AObjectFilter refInfo)
		{
            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex (refInfo.Type);
			string dbEntityName = (classInfo.AimObjectType == AimObjectType.Object ? "obj_" : "bl_") + classInfo.Name;

			AimPropInfo propInfo = Reader.GetPropInfo (refInfo.Type, refInfo.Property);

            if (propInfo.TypeIndex == (int)ObjectType.MdMetadata)
                return true;

			try
			{
				if (AimMetadata.IsChoice (propInfo.TypeIndex))
				{
					#region Choice

					string choiceTableName = aObject.ObjectType.ToString ();

					IDbCommand command = Connection.CreateCommand ();
					command.CommandText = string.Format (
						"SELECT " +
						"\"{0}\".prop_type, \"{0}\".choice_type, \"{0}\".target_guid, \"{0}\".target_id, \"{1}\".\"Id\" " +
						"FROM \"{0}\", \"{1}\" " +
						"WHERE " +
						"\"{0}\".\"Id\" = \"{1}\".\"{3}\" AND " +
						"\"{1}\".\"Id\" IN ({2})",
						choiceTableName,
						dbEntityName, 
						IDListStr,
						propInfo.Name);

					var dataReader = command.ExecuteReader ();
					var choiceRefList = new List<KeyValuePair<ChoiceRef, long>> ();

					while (dataReader.Read ())
					{
                        try
                        {
                            var choiceRef = Reader.GetChoiceRef(dataReader);
                            long ownerId = dataReader.GetInt64(dataReader.FieldCount - 1);
                            choiceRefList.Add(new KeyValuePair<ChoiceRef, long>(choiceRef, ownerId)); 
                        }
                        catch (Exception ex)
                        {
                            dataReader.Close();
                            throw ex;
                        }
						
					}

					dataReader.Close ();

					var dict = FillChoiceClassList (choiceRefList, aObject.ObjectType);

					foreach (DBEntity ownerObjItem in _list)
					{
						if (dict.ContainsKey (ownerObjItem.Id))
						{
							IList list = dict [ownerObjItem.Id];
							if (list.Count > 0)
								(ownerObjItem as IAimObject).SetValue (propInfo.Index, list [0] as IAimProperty);
						}

						(ownerObjItem as IEditDBEntity).AddLoaded (propInfo.Index);
					}
					
					#endregion
				}
				else
				{
					#region Object

					string objectTableName = "obj_" + aObject.ObjectType;
					AimPropInfoList simplePropList = new AimPropInfoList ();
					AimPropInfoList complexPropList = new AimPropInfoList ();

					Dictionary<AimPropInfo, int> columnIndices;

					string columns = Reader.SeparateProperties (propInfo.PropType.Properties, simplePropList,
						complexPropList, objectTableName, out columnIndices);

					IDbCommand command = Connection.CreateCommand ();
					command.CommandText = string.Format (
						"SELECT {3}, \"{1}\".\"Id\" " +
						"FROM \"{0}\",\"{1}\" " +
						"WHERE " +
						"\"{0}\".\"Id\" = \"{1}\".\"{4}\" AND " +
						"\"{1}\".\"Id\" IN ({2})",
						objectTableName,
						dbEntityName,
						IDListStr,
						columns,
						propInfo.Name);

					var dataReader = command.ExecuteReader ();

					var dict = new Dictionary<long, IAimObject> ();

					ComplexLoaderDataListener listener = new ComplexLoaderDataListener ();
					listener.Connection = Connection;
					listener.List = new List<IAimObject> ();

					while (dataReader.Read ())
					{
						AObject aObjItem = AimObjectFactory.CreateAObject ((ObjectType) propInfo.TypeIndex);
						Reader.ReadSimpleProperties (aObjItem, dataReader, simplePropList, columnIndices);

						long ownerId = dataReader.GetInt64 (dataReader.FieldCount - 1);
						dict.Add (ownerId, aObjItem);

						SetAddListener (aObjItem, listener);
					}
					dataReader.Close ();

					foreach (DBEntity ownerObjItem in _list)
					{
						try
						{
							if (ownerObjItem == null)
								continue;

							var typeIndex = AimMetadata.GetAimTypeIndex (ownerObjItem);
							if (typeIndex == classInfo.Index)
							{
								if (dict.ContainsKey (ownerObjItem.Id))
									(ownerObjItem as IAimObject).SetValue (propInfo.Index, dict [ownerObjItem.Id] as IAimProperty);

								(ownerObjItem as IEditDBEntity).AddLoaded (propInfo.Index);
							}
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}

					#endregion
				}
			}
			catch (Exception ex)
			{
				string message = CommonData.GetErrorMessage (ex);
				//throw new Exception (message);
			}

			return false;
		}

		public AObjectList<T> GetObjects<T> (AObjectFilter refInfo) where T : AObject, new ()
		{
            if (refInfo.Type == (int)ObjectType.MdMetadata)
                return null;

			AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex (refInfo.Type);
			AimPropInfo propInfo = Reader.GetPropInfo (refInfo.Type, refInfo.Property);

			string refDbEntityName = (classInfo.AimObjectType == AimObjectType.Object ? "obj_" : "bl_") + classInfo.Name;
			
			IDbCommand command = Connection.CreateCommand ();
			IDataReader dataReader = null;

			try
			{
				if (AimMetadata.IsAbstractFeatureRefObject (propInfo.TypeIndex))
				{
					#region IsAbstractFeatureRefObject

					command.CommandText = string.Format (
						"SELECT targetTableIndex, target_guid, \"{0}_id\" " +
						"FROM \"{0}_link\" " +
						"WHERE prop_Index = {1} AND \"{0}_id\" IN ({2})",
						refDbEntityName,
						refInfo.Property,
						IDListStr);

					dataReader = command.ExecuteReader ();

					var dict = new Dictionary<long, List<IAimObject>> ();

					ComplexLoaderDataListener absFeatRefObjListener = new ComplexLoaderDataListener ();
					absFeatRefObjListener.Connection = Connection;
					absFeatRefObjListener.List = new List<IAimObject> ();

					ComplexLoaderDataListener frAimObjListener = new ComplexLoaderDataListener ();
					frAimObjListener.Connection = Connection;
					frAimObjListener.List = new List<IAimObject> ();

					while (dataReader.Read ())
					{
						T absFeatRefObj = new T ();
						SetAddListener (absFeatRefObj, absFeatRefObjListener);

						AimPropInfo [] propInfos = AimMetadata.GetAimPropInfos (absFeatRefObj);
						int featureRefTypeIndex = -1;

						foreach (var item in propInfos)
						{
							if (item.Index == (int) PropertyAbstractFeatureRefObject.Feature)
							{
								featureRefTypeIndex = item.TypeIndex;
								break;
							}
						}

						AimObject frAimObj = AimObjectFactory.Create (featureRefTypeIndex);
						
						DBEntity frAimObjDbEntiry = frAimObj as DBEntity;
						if (frAimObjDbEntiry != null)
							SetAddListener (frAimObjDbEntiry, frAimObjListener);
						
						IAbstractFeatureRef absFeatRef = (frAimObj as IAbstractFeatureRef);
						absFeatRef.FeatureTypeIndex = Convert.ToInt32 (dataReader [0]);
						absFeatRef.Identifier = dataReader.GetGuid (1);
						
						((IAimObject) absFeatRefObj).SetValue ((int) PropertyAbstractFeatureRefObject.Feature, 
							frAimObj as IAimProperty);

						long ownerId = dataReader.GetInt64 (dataReader.FieldCount - 1);
						GetOrCreateDictItem (dict,ownerId).Add (absFeatRefObj);
					}

					SetListValues (dict, propInfo, refInfo.Id);

					#endregion
				}
				else if (propInfo.IsFeatureReference)
				{
					#region IsFeatureReference

					command.CommandText = string.Format (
						"SELECT target_guid, \"{0}_id\" FROM \"{0}_link\" " +
						"WHERE prop_Index = {1} AND \"{0}_id\" IN ({2}) ",
						refDbEntityName, refInfo.Property, IDListStr);

					dataReader = command.ExecuteReader ();

					var dict = new Dictionary<long, List<IAimObject>> ();

					while (dataReader.Read ())
					{
						FeatureRefObject featRef = new FeatureRefObject ();
						featRef.Feature = new FeatureRef ();
						featRef.Feature.Identifier = (Guid) dataReader [0];

						long ownerId = dataReader.GetInt64 (1);
						GetOrCreateDictItem (dict, ownerId).Add (featRef);
					}

					SetListValues (dict, propInfo, refInfo.Id);

					#endregion
				}
				else if (AimMetadata.IsChoice (propInfo.TypeIndex))
				{
					#region IsChoice

					command.CommandText = string.Format (
						"SELECT t1.prop_type, t1.choice_type, t1.target_guid, t1.target_id, t2.\"{1}_id\" " +
						"FROM \"{0}\" t1, \"{1}_link\" t2 " +
						"WHERE t1.\"Id\" = t2.target_id AND prop_index = {2} AND t2.\"{1}_id\" IN ({3})",
						propInfo.PropType.Name,
						refDbEntityName,
						refInfo.Property,
						IDListStr);

					dataReader = command.ExecuteReader ();

					var choiceRefList = new List<KeyValuePair<ChoiceRef, long>> ();

					while (dataReader.Read ())
					{
						var choiceRef = Reader.GetChoiceRef (dataReader);
						long ownerId = dataReader.GetInt64 (dataReader.FieldCount - 1);
						choiceRefList.Add (new KeyValuePair<ChoiceRef, long> (choiceRef, ownerId));
					}

					dataReader.Close ();

					var dict = FillChoiceClassList (choiceRefList, (ObjectType) propInfo.TypeIndex);

					SetListValues (dict, propInfo, refInfo.Id);

					#endregion
				}
				else
				{
					#region Object

					string tableName = "obj_" + propInfo.PropType.Name;

					AimPropInfoList simplePropList = new AimPropInfoList ();
					AimPropInfoList complexPropList = new AimPropInfoList ();

					Dictionary<AimPropInfo, int> columnIndices;
					string columns = Reader.SeparateProperties (propInfo.PropType.Properties, simplePropList, 
						complexPropList, tableName, out columnIndices);

					command.CommandText = string.Format (
						"SELECT {0} " +
						" ,\"{2}_link\".\"{2}_id\" " +
						"FROM \"{1}\", \"{2}_link\" WHERE " +
						"\"{1}\".\"Id\" = \"{2}_link\".target_id AND " +
						"\"{2}_link\".prop_Index = {3} AND " +
						"\"{2}_link\".\"{2}_id\" IN ({4})",
						columns,
						tableName,
						refDbEntityName,
						refInfo.Property,
						IDListStr);

					dataReader = command.ExecuteReader ();

					List<long> refObjIdList = new List<long> ();

					IList list = Reader.GetDBEntityList (propInfo.TypeIndex, dataReader, simplePropList, "",
						columnIndices, Connection, refObjIdList);

					var dict = new Dictionary<long, List<IAimObject>> ();

					for (int i = 0; i < list.Count; i++)
					{
						AimObject propAimObj = list [i] as AimObject;
						long ownerId = refObjIdList [i];
						GetOrCreateDictItem (dict, ownerId).Add (propAimObj);
					}

					SetListValues (dict, propInfo, refInfo.Id);

					#endregion
				}

				dataReader.Close ();
			}
			catch (Exception ex)
			{
				if (dataReader != null && !dataReader.IsClosed)
					dataReader.Close ();

				string message = CommonData.GetErrorMessage (ex);
				throw new Exception (message);
			}

			return null;
		}

		public AObject GetAbstractObject (AbstractType abstractType, AObjectFilter refInfo)
		{
			AimPropInfo propInfo = Reader.GetPropInfo (refInfo.Type, refInfo.Property);
			
			string tableName;
			if (AimMetadata.GetAimObjectType (refInfo.Type) == AimObjectType.Feature)
				tableName = "bl_";
			else
				tableName = "obj_";
			tableName += AimMetadata.GetAimTypeName (refInfo.Type);

			IDbCommand command = Connection.CreateCommand ();
			command.CommandText = string.Format ("SELECT \"{0}\", \"ref_{0}\", \"Id\" FROM \"{1}\" " +
													"WHERE \"Id\" IN ({2})",
													propInfo.Name,
													tableName,
													IDListStr);
			
			try
			{
				var dictByRefType = new Dictionary<int, List<KeyValuePair <int,long>>> ();
				var dataReader = command.ExecuteReader ();

				while (dataReader.Read ())
				{
					if (dataReader.IsDBNull (0) || dataReader.IsDBNull (1))
						continue;

					int refId = dataReader.GetInt32 (0);
					int refType = dataReader.GetInt32 (1);
					long ownerId = dataReader.GetInt64 (2);

					List<KeyValuePair<int, long>> dictItemList;
					if (!dictByRefType.TryGetValue (refType, out dictItemList))
					{
						dictItemList = new List<KeyValuePair<int, long>> ();
						dictByRefType.Add (refType, dictItemList);
					}
					dictItemList.Add (new KeyValuePair<int,long> (refId, ownerId));
				}
				dataReader.Close ();

				var dict = new Dictionary<long, IAimObject> ();

				ComplexLoaderDataListener listener = new ComplexLoaderDataListener ();
				listener.Connection = Connection;
				listener.List = new List<IAimObject> ();

				foreach (int refTypeKey in dictByRefType.Keys)
				{
					List<KeyValuePair<int, long>> dictItemList = dictByRefType [refTypeKey];

					if (dictItemList.Count == 0)
						continue;

					tableName = "obj_" + AimMetadata.GetAimTypeName (refTypeKey);

					AimPropInfoList allPropInfoList = new AimPropInfoList ();
					allPropInfoList.AddRange (AimMetadata.GetAimPropInfos (refTypeKey));
					AimPropInfoList simplePropList = new AimPropInfoList ();
					AimPropInfoList complexPropList = new AimPropInfoList ();
					Dictionary<AimPropInfo, int> columnIndices;
					string columns = Reader.SeparateProperties (allPropInfoList, simplePropList,
						complexPropList, "", out columnIndices);

					string idListStr = dictItemList [0].Key.ToString ();
					for (int i = 1; i < dictItemList.Count; i++)
						idListStr += "," + dictItemList [i].Key;

					command.CommandText = string.Format (
						"SELECT {0} FROM \"{1}\" WHERE \"Id\" IN ({2})",
						columns, tableName, idListStr);

					dataReader = command.ExecuteReader ();

					while (dataReader.Read ())
					{
						AimObject aimObj = AimObjectFactory.Create (refTypeKey);
						Reader.ReadSimpleProperties ((DBEntity) aimObj, dataReader, simplePropList, columnIndices);
						SetAddListener (aimObj as DBEntity, listener);

						long ownerId = dictItemList.Where (pair => 
							pair.Key == (aimObj as DBEntity).Id).First ().Value;
						dict.Add (ownerId, aimObj);
					}

					dataReader.Close ();
				}

				foreach (DBEntity ownerObjItem in _list)
				{
					IAimObject propValObj;
					if (dict.TryGetValue (ownerObjItem.Id, out propValObj))
						(ownerObjItem as IAimObject).SetValue (propInfo.Index, propValObj as IAimProperty);

					(ownerObjItem as IEditDBEntity).AddLoaded (propInfo.Index);
				}
			}
			catch (Exception ex)
			{
				throw new Exception (CommonData.GetErrorMessage (ex));
			}

			return null;
		}

		public IList GetAbstractObjects (AbstractType abstractType, AObjectFilter refInfo)
		{
			throw new NotImplementedException ();
		}

		#endregion

		public IDbConnection Connection { get; set; }

		public IList List
		{
			get { return _list; }
			set
			{
				_list = value;
				_idListStr = null;
				_dict = null;
			}
		}

		private void FillChoiceClass (ChoiceRef choiceRef, AObject aObject)
		{
			IEditChoiceClass editChoiceObj = (IEditChoiceClass) aObject;
			editChoiceObj.RefType = choiceRef.PropType;
			if (choiceRef.IsFeature)
			{
				editChoiceObj.RefValue = (IAimProperty) choiceRef.AimObj;
			}
			else
			{
				editChoiceObj.RefValue = (IAimProperty) choiceRef.AimObj;
				string tableName = "obj_" + AimMetadata.GetAimTypeName (choiceRef.ValueType);
				IDbCommand command = Connection.CreateCommand ();

				AimPropInfoList allPropInfoList = new AimPropInfoList ();
				allPropInfoList.AddRange (AimMetadata.GetAimPropInfos (choiceRef.ValueType));
				AimPropInfoList simplePropList = new AimPropInfoList ();
				AimPropInfoList complexPropList = new AimPropInfoList ();

				Dictionary<AimPropInfo, int> columnIndices;
				string columns = Reader.SeparateProperties (allPropInfoList, simplePropList, complexPropList, "", out columnIndices);

				command.CommandText = string.Format ("SELECT {0} FROM \"{1}\" WHERE \"Id\" = {2}",
													columns, tableName, choiceRef.Id);
				IDataReader dataReader = command.ExecuteReader ();
				if (dataReader.Read ())
				{
					List<AObject> objList = new List<AObject> ();
					objList.Add (aObject);

					ComplexLoaderDataListener myListener = new ComplexLoaderDataListener ();
					myListener.List = objList;
					myListener.Connection = Connection;
					((IEditDBEntity) editChoiceObj.RefValue).Listener = myListener;

					Reader.ReadSimpleProperties ((DBEntity) editChoiceObj.RefValue, dataReader, simplePropList, columnIndices);
				}
				dataReader.Close ();
			}
		}

		private Dictionary <long, List<IAimObject>> FillChoiceClassList (
			List<KeyValuePair<ChoiceRef, long>> choiceRefList, ObjectType objectType)
		{
			var resultDict = new Dictionary<long, List<IAimObject>> ();

			ComplexLoaderDataListener listener = new ComplexLoaderDataListener ();
			listener.List = new List<AObject> ();
			listener.Connection = Connection;

			var dictByValueType = new Dictionary<int, List<KeyValuePair<ChoiceRef, long>>> ();

			foreach (var choicePair in choiceRefList)
			{
				ChoiceRef choiceRef = choicePair.Key;

				if (choiceRef.IsFeature)
				{
					AObject aObj = AimObjectFactory.CreateAObject (objectType);
					(aObj as IEditChoiceClass).RefType = choiceRef.PropType;
					(aObj as IEditChoiceClass).RefValue = choiceRef.AimObj as IAimProperty;

					GetOrCreateDictItem (resultDict, choicePair.Value).Add (aObj);
				}
				else
				{
					List<KeyValuePair<ChoiceRef, long>> dictListItem;
					if (!dictByValueType.TryGetValue (choiceRef.ValueType, out dictListItem))
					{
						dictListItem = new List<KeyValuePair<ChoiceRef, long>> ();
						dictByValueType.Add (choiceRef.ValueType, dictListItem);
					}
					dictListItem.Add (choicePair);
				}
			}

			foreach (int valueTypeKey in dictByValueType.Keys)
			{
				List<KeyValuePair<ChoiceRef, long>> dictListItem = dictByValueType [valueTypeKey];
				var dictById = new Dictionary<long, KeyValuePair<ChoiceRef, long>> ();

				foreach (var item in dictListItem)
					dictById.Add (item.Key.Id, item);

				if (dictListItem.Count == 0)
					continue;

				var sb = new StringBuilder ();
				sb.Append (dictListItem [0].Key.Id);

				for (int i = 1; i < dictListItem.Count; i++)
				{
					sb.Append (',');
					sb.Append (dictListItem [i].Key.Id);
				}

				var idListStr = sb.ToString ();

				string tableName = "obj_" + AimMetadata.GetAimTypeName (valueTypeKey);
				IDbCommand command = Connection.CreateCommand ();
				AimPropInfoList allPropInfoList = new AimPropInfoList ();
				allPropInfoList.AddRange (AimMetadata.GetAimPropInfos (valueTypeKey));
				AimPropInfoList simplePropList = new AimPropInfoList ();
				AimPropInfoList complexPropList = new AimPropInfoList ();

				Dictionary<AimPropInfo, int> columnIndices;
				string columns = Reader.SeparateProperties (allPropInfoList, simplePropList, 
					complexPropList, "", out columnIndices);

				command.CommandText = string.Format ("SELECT {0}, \"Id\" FROM \"{1}\" WHERE \"Id\" IN ({2})",
													columns, tableName, idListStr);
				
				IDataReader dataReader = command.ExecuteReader ();
				
				while (dataReader.Read ())
				{
					long ownerId = dataReader.GetInt64 (dataReader.FieldCount - 1);
					KeyValuePair<ChoiceRef, long> currPair; //= dictListItem.Where (pr => pr.Key.Id == ownerId).First ();

					if (!dictById.TryGetValue (ownerId, out currPair))
						continue;    

					AObject aObj = AimObjectFactory.CreateAObject (objectType);
					IEditChoiceClass editChoiceClass = aObj as IEditChoiceClass;
					editChoiceClass.RefType = currPair.Key.PropType;
					editChoiceClass.RefValue = currPair.Key.AimObj as IAimProperty;

					Reader.ReadSimpleProperties ((DBEntity) editChoiceClass.RefValue, dataReader, simplePropList, columnIndices);

					GetOrCreateDictItem (resultDict, currPair.Value).Add (aObj);

					SetAddListener (editChoiceClass.RefValue as DBEntity, listener);
				}
				dataReader.Close ();
			}

			return resultDict;
		}

		private string IDListStr
		{
			get
			{
				if (_idListStr == null)
				{
					if (_list == null || _list.Count == 0)
						_idListStr = " -1 ";
					else
					{
						var sb = new StringBuilder ();
						sb.Append ((_list [0] as DBEntity).Id);
						for (int i = 1; i < _list.Count; i++)
						{
							sb.Append (',');
							sb.Append ((_list [i] as DBEntity).Id);
						}
						_idListStr = sb.ToString ();
					}
				}
				return _idListStr;
			}
		}

		private DBEntity GetListItem (long id)
		{
			if (_dict == null)
			{
				_dict = new Dictionary<long, DBEntity> ();
				foreach (DBEntity dbEntry in _list)
					_dict.Add (dbEntry.Id, dbEntry);
			}

			return _dict [id];
		}

		private void SetListValues (Dictionary <long, List <IAimObject>> dict, AimPropInfo propInfo, long ownerId)
		{
			foreach (DBEntity ownerObjItem in _list)
			{
				if (dict.ContainsKey (ownerObjItem.Id))
				{
					List<IAimObject> absFeatRefObjListById = dict [ownerObjItem.Id];

					IAimObject ownerObj = ownerObjItem as IAimObject;
					IList propList = ownerObj.GetValue (propInfo.Index) as IList;
					if (propList == null)
					{
						propList = AimObjectFactory.CreateAObjectList ((ObjectType) propInfo.TypeIndex);
						ownerObj.SetValue (propInfo.Index, propList as IAimProperty);
					}

					foreach (IAimObject propValItem in absFeatRefObjListById)
						propList.Add (propValItem);
				}

				(ownerObjItem as IEditDBEntity).AddLoaded (propInfo.Index);
			}
		}

		private List<IAimObject> GetOrCreateDictItem (Dictionary<long, List<IAimObject>> dict, long keyId)
		{
			List<IAimObject> propValListById;
			if (!dict.TryGetValue (keyId, out propValListById))
			{
				propValListById = new List<IAimObject> ();
				dict.Add (keyId, propValListById);
			}
			return propValListById;
		}

		private void SetAddListener (DBEntity dbEntry, ComplexLoaderDataListener listener)
		{
			(dbEntry as IEditDBEntity).Listener = listener;
			listener.List.Add (dbEntry);
		}

		private IList _list;
		private string _idListStr;
		private Dictionary<long, DBEntity> _dict;
    }
}
