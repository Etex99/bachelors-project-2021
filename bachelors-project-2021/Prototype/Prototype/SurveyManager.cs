using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace Prototype
{
    public class SurveyManager
    {
        private static SurveyManager instance = null;
        private Survey survey;
        private string[] surveyTemplates;
        private readonly string folder;

        //Constructor for surveymanager that is going to be when surveymanager is used
        private SurveyManager() {
            survey = new Survey();
            folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        //Method for saving the survey with name of user's choosing
        public bool SaveSurvey(string name)
        {   
            string jsonString = JsonSerializer.Serialize(survey);
            string path = Path.Combine(folder, name.ToLower());
            File.WriteAllText(path, jsonString);

            return true;
        }

        //Method for loading the survey by selecting the file with name
        public Survey LoadSurvey(string name)
        {
            string path = Path.Combine(folder, name.ToLower());
            string jsontext = File.ReadAllText(path);
            survey = JsonSerializer.Deserialize<Survey>(jsontext);

            return survey;
        }
        
        //Method for getting the names of all the saved files in the folder
        public List<string> GetTemplates()
        {
            surveyTemplates = Directory.GetFiles(folder);
            List<string> surveyNames = new List<string>();

            foreach (string name in surveyTemplates)
            {
                string filename = name.Substring(name.LastIndexOf('/') + 1);
                filename = filename.Substring(0, filename.LastIndexOf("."));
                surveyNames.Add(filename);
                Console.WriteLine(filename);
            }
            return surveyNames;
        }

        //Method for deleting survey by using the name of existing survey
        public void DeleteSurvey(string name)
        {
            string path = Path.Combine(folder, name);
            File.Delete(path);
        }

        //Method for getting the survey 
        public Survey GetSurvey()
        {
            return survey;
        }

        //Method for resetting the survey for starting with a blank survey
        public void ResetSurvey()
        {
            survey = new Survey();
        }

        //method for getting an instance of surveymanager so there isn't more than one surveymanager object
        public static SurveyManager GetInstance() {
            if (instance != null) {
                return instance;
			}
            instance = new SurveyManager();
            return instance;
		}
    }
}