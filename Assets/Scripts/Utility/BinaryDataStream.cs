using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utility
{
    public class BinaryDataStream : MonoBehaviour
    {
        public static void Save<T>(T serializableObject, string fileName)
        {
            var path = Application.persistentDataPath + "/saves/";
            Directory.CreateDirectory(path);

            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(path + fileName + ".dat", FileMode.Create);

            try
            {
                formatter.Serialize(fileStream, serializableObject);
            }
            catch (SerializationException e)
            {
                Debug.Log("Save failed: " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }

        public static bool Exist(string fileName)
        {
            var path = Application.persistentDataPath + "/saves/";
            var fullPath = path + fileName + ".dat";
            return File.Exists(fullPath);
        }

        public static T Read<T>(string fileName)
        {
            var path = Application.persistentDataPath + "/saves/";
            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(path + fileName + ".dat", FileMode.Open);
            var returnObject = default(T);

            try
            {
                returnObject = (T)formatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                Debug.Log("Read failed: " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }

            return returnObject;
        }
    }
}