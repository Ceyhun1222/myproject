using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Data
{
    internal class DeltaProp
    {
        public byte [] ToByteArray ( object value)
        {
			List<byte> byteList = new List<byte> ( );
			byteList.AddRange ( BitConverter.GetBytes ( true ) );
			if ( value is Guid )
			{
				byteList.AddRange ( ( ( Guid ) value ).ToByteArray ( ) );
				return byteList.ToArray ( );
			}

			if ( value is bool )
			{
				byteList.AddRange ( BitConverter.GetBytes ( ( bool ) value ) );
				return byteList.ToArray ( );
			}
			if ( value is double )
			{
				byteList.AddRange ( BitConverter.GetBytes ( ( double ) value ) );
				return byteList.ToArray ( );
			}

			if ( value is uint )
			{
				byteList.AddRange ( BitConverter.GetBytes ( ( uint ) value ) );
				return byteList.ToArray ( );
			}

			if ( value is int )
			{
				byteList.AddRange ( BitConverter.GetBytes ( ( int ) value ) );
				return byteList.ToArray ( );
			}
			if ( value is long )
			{
				byteList.AddRange ( BitConverter.GetBytes ( ( long ) value ) );
				return byteList.ToArray ( );
			}
			if ( value is string )
			{
				byteList.AddRange ( Encoding.UTF8.GetBytes ( ( string ) value ) );
				return byteList.ToArray ( );
			}

            if (value is DateTime)
            {
                long tmp = ( ( DateTime ) value ).ToBinary ();
				byteList.AddRange ( BitConverter.GetBytes ( tmp ) );
				return byteList.ToArray ( );
            }

			if ( value is TimePeriod )
			{								
				TimePeriod timePeriod = (TimePeriod) value;
				long tmp;
				if ( timePeriod.BeginPosition != null )
				{
					byteList.AddRange ( BitConverter.GetBytes ( true ) );
					tmp = timePeriod.BeginPosition.ToBinary ( );
					byteList.AddRange ( BitConverter.GetBytes ( tmp ) );
				}
				else
				{
					byteList.AddRange ( BitConverter.GetBytes ( false ) );
				}

				byteList.AddRange ( BitConverter.GetBytes ( timePeriod.EndPosition.HasValue ) );

				if ( timePeriod.EndPosition.HasValue )
				{
					tmp = timePeriod.EndPosition.Value.ToBinary ( );
					byteList.AddRange ( BitConverter.GetBytes ( tmp ) );
				}
				return byteList.ToArray ( );
			}

            if ( value is TextNote )
            {
                _textNote = ( TextNote ) value;
                byteList.AddRange ( Encoding.UTF8.GetBytes ( _textNote.Value ) );
                byteList.AddRange ( BitConverter.GetBytes ( ( int ) _textNote.Lang ) );
                return byteList.ToArray ();
            }

            if ( value is IEditValClass )
            {
                _editValClass = ( IEditValClass ) value;
				//double siValue = Converters.ConverterToSI.Convert ( _editValClass, double.NaN );
				//byteList.AddRange ( BitConverter.GetBytes ( siValue ) );
                byteList.AddRange ( BitConverter.GetBytes ( _editValClass.Value ) );
                byteList.AddRange ( BitConverter.GetBytes ( _editValClass.Uom ) );
                return byteList.ToArray ();
            }

            if ( value is IAbstractFeatureRef )
            {
                _absFeatRef = ( IAbstractFeatureRef ) value;
                byteList.AddRange ( _absFeatRef.Identifier.ToByteArray () );
                byteList.AddRange ( BitConverter.GetBytes ( _absFeatRef.FeatureTypeIndex ) );
                return byteList.ToArray ();
            }

            if ( value is List<long> )
            {
                _idList = ( List<long> ) value;
				if ( _idList.Count == 0 )
					return BitConverter.GetBytes ( false );
				
				byteList.AddRange ( BitConverter.GetBytes ( _idList.Count ) );
                foreach ( long id in _idList )
                    byteList.AddRange ( BitConverter.GetBytes ( id ) );
                return byteList.ToArray ();
            }

            if ( value is AObject )
            {
                _absObj = ( AObject ) value;
                byteList.AddRange ( BitConverter.GetBytes ( _absObj.Id ) );
                byteList.AddRange ( BitConverter.GetBytes ( ( int ) _absObj.ObjectType ) );
                return byteList.ToArray ();
            }


            if ( value is IList )
            {                
				IList featList = (IList)value;
				if ( featList.Count == 0 )
					return BitConverter.GetBytes ( false );
				
				byteList.AddRange ( BitConverter.GetBytes ( featList.Count ) );

                if ( featList [0] is FeatureRefObject )
                {
                    FeatureRefObject featRefObj;
                    for ( int i = 0; i <= featList.Count - 1; i++ )
                    {
                        featRefObj = featList [i] as FeatureRefObject;
                        byteList.AddRange ( featRefObj.Feature.Identifier.ToByteArray () );
                    }
                    return byteList.ToArray ();
                }

                // AbstractFeatureRefObject
                for ( int i = 0; i <= featList.Count - 1; i++ )
                {
                    IAimProperty absFeatRefProp = ( ( IAimObject ) featList [i] ).GetValue ( ( int ) PropertyAbstractFeatureRefObject.Feature );
                    IAbstractFeatureRef absFeatRef = ( IAbstractFeatureRef ) absFeatRefProp;
                    byteList.AddRange ( absFeatRef.Identifier.ToByteArray () );
                    byteList.AddRange ( BitConverter.GetBytes ( absFeatRef.FeatureTypeIndex ) );
                }
                return byteList.ToArray ();
            }
            throw new Exception ( "Not Found property type in DeltaProp !" );
        }

		/// <summary>
		/// Gets value from byte[].
		/// Returns true if property is AranField 
		/// else returns false which means that has to be fullfilled outside
		/// </summary>
		/// <param name="propInfo"></param>
		/// <param name="byteArray"></param>
		/// <param name="aimProp"></param>
		/// <returns></returns>
		public bool FromByteArray ( AimPropInfo propInfo, byte[] byteArray, out IAimProperty aimProp)
		{
			bool hasValue = BitConverter.ToBoolean ( byteArray, 0 );
			if ( !hasValue )
			{
				aimProp = null;
				return true;
			}
			AimObject aimObj = AimObjectFactory.Create ( propInfo.TypeIndex );
			aimProp = ( IAimProperty ) aimObj;
			object value;

			if ( aimProp.PropertyType == AimPropertyType.AranField )
			{
				IEditAimField editAimField = ( IEditAimField ) aimProp;
				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysGuid )
				{
					byte [ ] guidByteArray = new byte [ 16 ];
					Array.Copy ( byteArray, 1, guidByteArray, 0, 16 );
					value = new Guid ( guidByteArray );
					editAimField.FieldValue = value;
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysBool )
				{
					value = BitConverter.ToBoolean ( byteArray, 1 );
					editAimField.FieldValue = value;
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysDouble )
				{
					value = BitConverter.ToDouble ( byteArray, 1 );
					editAimField.FieldValue = value;
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysUInt32)
				{
					value = BitConverter.ToUInt32 ( byteArray, 1 );
					editAimField.FieldValue = value;
					return true;
				}


				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysInt32 || AimMetadata.IsEnum ( propInfo.TypeIndex ) )
				{
					value = BitConverter.ToInt32 ( byteArray, 1 );
					editAimField.FieldValue = value;
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysString )
				{
					byte [ ] stringByteAr = new byte [ byteArray.Length - 1 ];
					Array.Copy ( byteArray, 1, stringByteAr, 0, byteArray.Length - 1 );
					value = Encoding.UTF8.GetString ( stringByteAr );
					editAimField.FieldValue = value;
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) AimFieldType.SysDateTime )
				{
					long dateTimeLong = BitConverter.ToInt64 ( byteArray, 1 );
					value = DateTime.FromBinary ( dateTimeLong );
					editAimField.FieldValue = value;
					return true;
				}
			}
			else if ( aimProp.PropertyType == AimPropertyType.DataType )
			{
				if ( propInfo.TypeIndex == (int) DataType.TimeSlice )
				{
					hasValue = BitConverter.ToBoolean ( byteArray, 1 );
					TimePeriod timePeriod = new TimePeriod ( );
					long dateTimeLong;
					if ( !hasValue )
						timePeriod.BeginPosition = DateTime.MinValue;
					else
					{
						dateTimeLong = BitConverter.ToInt64 ( byteArray, 2 );
						timePeriod.BeginPosition = DateTime.FromBinary ( dateTimeLong );
					}

					hasValue = BitConverter.ToBoolean ( byteArray, 10 );
					if ( !hasValue )
					{
						timePeriod.EndPosition = null;
					}
					else
					{
						dateTimeLong = BitConverter.ToInt64 ( byteArray, 10 );
						timePeriod.EndPosition = DateTime.FromBinary ( dateTimeLong );
					}
					
					return true;
				}

				if ( propInfo.TypeIndex == ( int ) DataType.TextNote )
				{
					TextNote txtNote = new TextNote ( );
					byte [ ] stringByteAr = new byte [ byteArray.Length - 1 ];
					Array.Copy ( byteArray, 1, stringByteAr, 0, byteArray.Length - 1 );
					txtNote.Value = Encoding.UTF8.GetString ( stringByteAr );
					txtNote.Lang = ( Enums.language ) BitConverter.ToInt32 ( byteArray, byteArray.Length - 1 );
					aimProp = txtNote;
					return true;
				}

				if ( aimProp is IEditValClass )
				{
					IEditValClass valClass = ( IEditValClass ) aimProp;
					valClass.Value = BitConverter.ToDouble ( byteArray, 1 );
					valClass.Uom = BitConverter.ToInt32 ( byteArray, 9 );
					aimProp = ( ValClassBase ) valClass;
					return true;
				}

				if ( AimMetadata.IsAbstractFeatureRef ( propInfo.TypeIndex ) )
				{
					IAbstractFeatureRef absFeatRef = ( IAbstractFeatureRef ) aimProp;
					byte [ ] guidByteAr = new byte [ byteArray.Length - 1 ];
					int i = 1;
					while ( i < 17 )
					{
						guidByteAr [ i - 1 ] = byteArray [ i ];
						i++;
					}
					absFeatRef.Identifier = new Guid ( guidByteAr );
					absFeatRef.FeatureTypeIndex = ( int ) BitConverter.ToInt32 ( byteArray, 16 );
					aimProp = ( AbstractFeatureRefBase ) absFeatRef;
					return true;
				}
			}
			else if ( propInfo.IsList)
			{
				IList propList;

				int count = BitConverter.ToInt32 ( byteArray, 1 );
	
				// AbstractFeatureRefObject
				// FeatureReference
				// IsChoice
				// Object

				if ( AimMetadata.IsAbstractFeatureRefObject ( propInfo.TypeIndex ) )
				{

					aimProp = ( IAimProperty ) AimObjectFactory.CreateList ( propInfo.TypeIndex );
					propList = ( IList ) aimProp;
					for ( int i = 0; i < count; i++ )
					{
						IAbstractFeatureRef absFeatRef = ( IAbstractFeatureRef ) aimProp;
						absFeatRef.FeatureTypeIndex = BitConverter.ToInt32 ( byteArray, 5 + i * 20 );
						byte [ ] guidByteArray = new byte [ 16 ];
						Array.Copy ( byteArray, 9 + i * 20, guidByteArray, 0, 16 );
						absFeatRef.Identifier = new Guid ( guidByteArray );
						//( ( IAimObject ) absFeatRefObj ).SetValue ( ( int ) PropertyAbstractFeatureRefObject.Feature, aimProp );
						//listProp.Add ( absFeatRefObj );
					}
					return true;
				}
				if ( propInfo.IsFeatureReference )
				{
					return false;
				}

				if ( AimMetadata.IsChoice ( propInfo.TypeIndex ) )
				{
					return false;
				}

				if ( propInfo.PropType.AimObjectType == AimObjectType.Object )
				{
					propList = new List<long> ( );
					for ( int i = 0; i < count; i++ )
					{
						propList.Add ( BitConverter.ToInt64 ( byteArray, 5 + i * 8 ) );
					}
					return false;
				}
			}

			throw new Exception ( "Not found Property Type to convert from byte array !" );
		}

        private IEditValClass _editValClass;
        private TextNote _textNote;
        private IAbstractFeatureRef _absFeatRef;
        private AObject _absObj;
        private List<long> _idList;
    }
}
