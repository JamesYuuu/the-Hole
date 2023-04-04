using System.Collections.Generic;
using UnityEngine;

namespace Dialog.Scripts
{
    /// <summary>
    /// Parses the text file and sends the output back to the DialogBehaviour
    /// Does not handle interaction.
    /// </summary>
    public class DialogParser : MonoBehaviour 
    {
        private static DialogParser _instance;
        private static string eTag = "::";
        public static DialogParser GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Initialises singleton.
        /// </summary>
        private void Awake()
        {
            // if there is another instance, destroy this. else make this the singleton
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// Breaks a text file into its 'speeches'
        /// and enumerates their speaking speed.
        /// </summary>
        /// <param name="source">The text file.</param>
        /// <returns>A Queue of (Speed, speech) tuples.</returns>
        public Queue<(TextSpeed speed, string speech)> ParseTextFileAsQueue(TextAsset source)
        {
            var result = new Queue<(TextSpeed speed, string speech)>();
            
            // Split text into pages using @
            string[] chunks = source.text.Split("@"[0]);
            foreach (string chunk in chunks)
            {
                // convert speed of text into enums
                TextSpeed spd = chunk.Contains("~Slow~") // if condition ? true : false;
                    ? TextSpeed.Slow
                    : chunk.Contains("~Fast~")
                    ? TextSpeed.Fast
                    : TextSpeed.Normal;
                
                // remove tags from speech
                string spchTxt = chunk
                    .Replace("~Fast~", "")
                    .Replace("~Slow~", "")
                    .Replace("\n", "")
                    .Replace("@", "");
                
                result.Enqueue((spd, spchTxt));
            }
            return result;
        }

        public List<(TextSpeed speed, string speech)> ParseTextFileAsList(TextAsset source)
        {
            return new List<(TextSpeed speed, string speech)>(
                ParseTextFileAsQueue(source));
        }
        
        public Queue<(TextSpeed speed, string speech, int eventIdx)>
            ParseEventTextFileAsQueue(TextAsset source, List<string> eventTagKeys)
        {
            var result = new Queue<(TextSpeed, string, int)>();
            
            // Split text into pages using @
            string[] chunks = source.text.Split("@"[0]);
            foreach (string chunk in chunks)
            {
                // convert speed of text into enums
                TextSpeed spd = chunk.Contains("~Slow~") // if condition ? true : false;
                    ? TextSpeed.Slow
                    : chunk.Contains("~Fast~")
                    ? TextSpeed.Fast
                    : TextSpeed.Normal;
                
                // remove tags from speech
                string spchTxt = chunk
                    .Replace("~Fast~", "")
                    .Replace("~Slow~", "")
                    .Replace("\n", "")
                    .Replace("@", "");

                int currEventKey = -1;
                if (spchTxt.Contains(eTag))
                {
                    for (int i = 0; i < eventTagKeys.Count; i++)
                    {
                        string currSubstr = eTag + eventTagKeys[i];
                        if (spchTxt.Contains(currSubstr))
                        {
                            currEventKey = i;
                            spchTxt = spchTxt.Replace(currSubstr, "");
                            break;
                        }
                    }
                }
                result.Enqueue((spd, spchTxt, currEventKey));
            }
            return result;
        }
    }
}
