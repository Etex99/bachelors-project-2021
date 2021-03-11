using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text.Json;
using System.IO;

namespace Prototype
{
    public class SurveyManager
    {
        private Survey survey;
        private List<string> surveyTemplates;
        private string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public bool SaveSurvey(Survey survey, string name)
        {
            string jsonString = JsonSerializer.Serialize(survey);
            if(Device.RuntimePlatform == Device.Android)
            {
                string path = Path.Combine(folder, "test.txt" /*name*/);
                File.WriteAllText(path, jsonString);
            }
            return true;
        }

        public Survey LoadSurvey(string name)
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                string path = Path.Combine(folder, "test.txt" /*name*/);
                string jsontext = File.ReadAllText(path);
                survey = JsonSerializer.Deserialize<Survey>(jsontext);
            }
            return survey;
        }

        public string[] GetTemplates()
        {
            string[] surveyTemplates = Directory.GetFiles(folder);
            return surveyTemplates;
        }

        public void DeleteSurvey(string name)
        {

        }

        public Survey GetSurvey()
        {
            return new Survey();
        }

        public void ResetSurvey()
        {

        }
    }
}