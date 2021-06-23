using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace ARF_Editor.Tools
{
    class INISettings
    {
        private INIFile iniFile;
        public Dictionary<string, Dictionary<string, string>> objects
        {
            get => this.iniFile.objects;
            set => this.iniFile.objects = value;
        }
        
        public static string EmptySettings()
        {
            return "[Database]\npath=";
        }
        public INISettings()
        {
            string path = System.Environment.GetEnvironmentVariable("appdata") + @"\ARF-Editor\settings.ini";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, EmptySettings());
            }
            this.iniFile = new INIFile(System.Environment.GetEnvironmentVariable("appdata") + @"\ARF-Editor\settings.ini"); 
        }

        public INISettings Set(string header, string value, string newValue)
        {
            var newerObjects = this.iniFile.objects;
            newerObjects[header][value] = newValue;
            iniFile.objects = newerObjects;
            iniFile.Update();

            return this;
        }

        public override string ToString()
        {
            string s = "";
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in this.objects)
            {
                s += kvp.Key + "\n{\n";
                foreach (KeyValuePair<string, string> _kvp in kvp.Value)
                    s += $"     {_kvp.Key}: {_kvp.Value}\n";
                s += "}\n";
            }
            return s;
        }
    }
    
    public class INIFile
    {

        string path;
        public Dictionary<string, Dictionary<string, string>> objects = new Dictionary<string, Dictionary<string, string>>();
        
        public INIFile(string path)
        {
            this.path = path;

            #region reading
            string[] fullContent;
            // Liest den Content von der Datei aus
            fullContent = File.ReadAllLines(path);

            //Alle Headernamen
            string[] headers = fullContent.Where(c => c.StartsWith("[") && c.EndsWith("]")).ToArray();
            // Alle Header mit content vom Header
            List<(string, string[])> _headers = new List<(string, string[])>();
            

            // Der momentane HeaderIndex
            int headerN = 0;
            // Die Elemente vor dem Header
            List<string> lastElements = new List<string>();
            for(int i = fullContent.Length - 1; i >= 0 ; i--)
            {
                // Wenn die aktuelle Line ein Header ist
                if (headers.Length > headerN && fullContent[i] == headers[headerN])
                {
                    headerN++;
                    // Fügt den Content hinzu
                    _headers.Add( (fullContent[i].Replace("[", "").Replace("]", ""), lastElements.ToArray().Reverse().ToArray()) );
                    lastElements = new List<string>();
                    continue;
                }
                lastElements.Add(fullContent[i]);
            }
            #endregion
            #region mapping
            for (int i = 0; i < _headers.Count; i++)
            {
                Dictionary<string, string> headerDict = new Dictionary<string, string>();
                for (int j = 0; j < _headers[i].Item2.Length; j++)
                {
                    headerDict.Add(_headers[i].Item2[j].Split("=")[0].Replace("\n", ""), _headers[i].Item2[j].Split("=")[1].Replace("\n", ""));
                }
                this.objects.Add(_headers[i].Item1, headerDict);
            }
            #endregion

        }
        /// <summary>
        /// Aktuallisiert die INI Datei indem der aktuelle Content reingeschrieben wird
        /// </summary>
        public void Update()
        {
            File.WriteAllText(this.path, this.ToString());
        }
        static void Update(INIFile f)
        {
            File.WriteAllText(f.path, f.ToString());
        }

        public override string ToString()
        {
            string s = "";
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in this.objects)
            {
                s += "[" + kvp.Key + "]\n";
                foreach (KeyValuePair<string, string> _kvp in kvp.Value)
                    s += $"{_kvp.Key}={_kvp.Value}\n";
            }
            return s;
        }
    }
}
