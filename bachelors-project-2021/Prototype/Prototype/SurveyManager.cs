using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace Prototype
{
    public class SurveyManager
    {
        private static SurveyManager instance;
        private Survey survey;
        private string[] surveyTemplates;
        private readonly string folder;

        private SurveyManager() {
            survey = new Survey();
            folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        
        public bool SaveSurvey(string name = "test.txt")
        {   
            string jsonString = JsonSerializer.Serialize(survey);
            string path = Path.Combine(folder, name.ToLower());
            File.WriteAllText(path, jsonString);

            return true;
        }

        public Survey LoadSurvey(string name = "test.txt")
        {
            string path = Path.Combine(folder, name.ToLower());
            string jsontext = File.ReadAllText(path);
            survey = JsonSerializer.Deserialize<Survey>(jsontext);

            return survey;
        }

        public List<string> GetTemplates()
        {
            surveyTemplates = Directory.GetFiles(folder);
            List<string> surveyNames = new List<string>();

            foreach (string name in surveyTemplates)
            {
                string filename = name.Substring(name.LastIndexOf('/') + 1);
                filename = filename.Substring(0, filename.LastIndexOf("."));
                surveyNames.Add(filename);
            }
            return surveyNames;
        }

        public void DeleteSurvey(string name = "test.txt")
        {
            string path = Path.Combine(folder, name);
            Directory.Delete(path);
        }

        public Survey GetSurvey()
        {
            return survey;
        }

        public void ResetSurvey()
        {
            survey = new Survey();
        }

        public static SurveyManager GetInstance() {
            if (instance != null) {
                return instance;
			}
            instance = new SurveyManager();
            return instance;
		}
    }
}