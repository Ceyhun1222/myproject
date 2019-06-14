using System;
using System.Collections.Generic;
using System.Text;

namespace ARAN.GeometryClasses
{
    public class Registry
    {
        static public int GetInstance(string className) 
        {
            int handle = 0;
            //throw RegistryException;
            return handle;
        }
        static public void FreeInstance(int handle)
        { 
        }
        static public int IsInProcess(int handle)
        {
            return 0;
        }
        static public void SetHandler(int handle, int handlerData, RegistryMethod method)
        { 
        }
        static public void Unsethandler(int handle)
        { 
        }
        static public void UnregisterClass(int handle)
        { 
        }
        static public int LoadService(string className, string path, string entryName, int priority)
        {
            return 0;
        }
        static public void BeginMessage(int handle, int command)
        { 
        }
        static public void EndMessage(int handle)
        { 
        }
        static public void BeginEvent(int handle, int command)
        { 
        }
        static public void EndEvent(int handle)
        { 
        }
        static public void PutByte(int handle, int value)
        { 
        }
        static public void PutShort(int handle, int value)
        { 
        }
        static public void PutInt(int handle, int value)
        { 
        }
        static public void PutFloat(int handle, float value)
        { 
        }
        static public void PutDouble(double handle, double value)
        { 
        }
        static public void PutString(int handle, int value)
        { 
        }
        static public void PutArray(int handle, byte[] dataPtr)
        { 
        }
        static public void PutHandle (int handle, int h)
        {
        }
        static public void PutWideString (int handle, String str)
        {
        }
        static public void PutBool (int handle,bool value)
        {
        }
    
        static public int GetByte (int handle)
        {
            return 0;
        }
    
        static public int GetShort (int handle)
        {
            return 0;
        }
    
        static public int GetInt (int handle)
        {
            return 0;
        }
    
        static public int GetFloat (int handle)
        {
            return 0;
        }
    
        static public int GetDouble (int handle)
        {
            return 0;
        }
    
        static public int GetString (int handle)
        {
            return 0;
        }
    
        static public byte[] GetArray (int handle)
        {
            return null;
        }
    
        static public int GetHandle (int handle)
        {
            return 0;
        }
    
        static public byte[] GetWideString (int handle)
        {
            return null;
        }
    
        static public bool GetBool (int handle)
        {
            return true;
        }
    }
}
