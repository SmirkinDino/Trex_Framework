using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>
/// 
/// SmirkinDino 2017.06.01
/// 
/// </summary>
public static class ExtendCloner
{
    private static IFormatter HandleFormatter;

    private static Stream HandleStream;

    public static T DeepClone<T>(this T _source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable");
        }

        if (System.Object.ReferenceEquals(_source, null))
        {
            return default(T);
        }

        if (HandleFormatter == null)
        {
            HandleFormatter = new BinaryFormatter();
        }

        if (HandleStream == null)
        {
            HandleStream = new MemoryStream();
        }

        using (HandleStream)
        {
            HandleFormatter.Serialize(HandleStream, _source);

            HandleStream.Seek(0, SeekOrigin.Begin);

            return (T)HandleFormatter.Deserialize(HandleStream);
        }
    }
}
