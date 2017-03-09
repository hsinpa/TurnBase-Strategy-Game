using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utility
{
    /// <summary>
    /// Class that describes a CSV file and helps to extract informations from it.
    /// </summary>
    public class CSVFile
    {
        /// <summary>
        /// File's original data.
        /// </summary>
        public string source { get; protected set; }

        /// <summary>
        /// This file lines.
        /// </summary>
        public string[][] lines { get; protected set; }

        /// <summary>
        /// Number of data lines.
        /// </summary>
        public int length { get { return lines.Length - 1; } }

        /// <summary>
        /// All tokens without regard for format.
        /// </summary>
        public string[] tokens { get; protected set; }
                
        /// <summary>
        /// Returns the CSV file keys, which usually are located at the first line.
        /// </summary>
        public string[] keys { get; protected set; }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_source"></param>
        public CSVFile(string p_source)
        {
            source = p_source;
			string[] l =source.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            lines = new string[l.Length][];
            List<string> res = new List<string>();
                            
            for (int i = 0; i < lines.Length; i++) {
				string[] tks =CSVReader.SplitCsvLine( l[i] );

                lines[i] = tks;

//                if (i != 0) {
//                	if (tks.Length != lines[0].Length) {
//                		Debug.LogError( "CSV Parse Error");
//                	}
//                }
                res.AddRange(tks);
            }

            keys = new string[lines[0].Length];
            for (int i = 0; i < keys.Length; i++) keys[i] = lines[0][i].Trim();

            tokens = res.ToArray();
        }

        /// <summary>
        /// Try to sample a value from the file and convert it to the desired format.
        /// If the process fails, the default value is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_line"></param>
        /// <param name="p_key"></param>
        /// <returns></returns>
        public T Get<T>(int p_line, string p_key) { return Get<T>(p_line, p_key,default(T)); }

        /// <summary>
        /// Try to sample a value from the file and convert it to the desired format.
        /// If the process fails, the default value is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_line"></param>
        /// <param name="p_key"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public T Get<T>(int p_line,string p_key,T p_default)
        {
            int lid = p_line+1;
            if (lid <= 0)                return p_default;
            if (lid >=lines.Length)      return p_default;            
            int k = System.Array.IndexOf(keys, p_key);            
            if (k < 0)                   return p_default;
            if (k >= lines[lid].Length) return p_default;            
            return Parse<T>(lines[lid][k], p_default);
        }

        /// <summary>
        /// Samples a token by its id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public T GetToken<T>(int p_id) { return GetToken<T>(p_id, default(T)); }

        /// <summary>
        /// Samples a token by its id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_id"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public T GetToken<T>(int p_id,T p_default) 
        {
            int k = p_id;
            if (k < 0) return p_default;
            if (k >= tokens.Length) return p_default;
            return Parse<T>(tokens[k], p_default);
        }

        /// <summary>
        /// Parses a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_value"></param>
        /// <returns></returns>
        public T Parse<T>(string p_value) { return Parse<T>(p_value, default(T)); }

        /// <summary>
        /// Helper parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public T Parse<T>(string p_value,T p_default)
        {
            if (typeof(T) == typeof(string)) { return (T)(object)p_value; }
            if (typeof(T) == typeof(int)) { int res = 0; if (!int.TryParse(p_value, out res))           res = (int)(object)p_default; return (T)(object)res; }
            if (typeof(T) == typeof(float)) { float res = 0.0f; if (!float.TryParse(p_value, out res))  return p_default; return (T)(object)res; }
            if (typeof(T) == typeof(bool)) { return (T)(object)(p_value.ToLower() == "true"); }
            return p_default;
        }

    }
}
