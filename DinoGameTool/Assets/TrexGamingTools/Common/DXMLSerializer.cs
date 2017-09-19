using Dino_Core.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DXMLSerializer {

    public static void SerializeObjectToXml<T>(string _filePath, T _obj, bool _apend = true)
    {
        SerializeObjectToXml(_filePath, _obj, null, _apend);
    }

    public static void SerializeObjectToXml<T>(string _filePath, T _obj, Type[] _extraTypes, bool _apend = true)
    {
        if (!_apend && File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        if (_obj != null)
        {
            using (StreamWriter _writer = new StreamWriter(_filePath, false))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T), _extraTypes);

                    xmlSerializer.Serialize(_writer, _obj);
                }
                catch (Exception _error)
                {
                    ExtendLib.DLog("DXMLSerializer", _error);
                }
                finally
                {
                    _writer.Close();
                }
            }
        }
    }

    public static object DeSerializeXmlToObject(string _filePath, Type _type, Type[] _extraTypes)
    {
        object _result = default(object);


        if (File.Exists(_filePath))
        {
            using (StreamReader _reader = new StreamReader(_filePath))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer _xmlSerializer = new System.Xml.Serialization.XmlSerializer(_type, _extraTypes);

                    _result = _xmlSerializer.Deserialize(_reader);
                }
                catch (Exception _error)
                {
                    ExtendLib.DLog("DXMLSerializer", _error);
                }
                finally
                {
                    _reader.Close();
                }

                return _result;
            }
        }


        return default(object);
    }

    public static object DeSerializeXmlToObject(string _filePath, Type _type)
    {
        return DeSerializeXmlToObject(_filePath, _type, null);
    }
}
