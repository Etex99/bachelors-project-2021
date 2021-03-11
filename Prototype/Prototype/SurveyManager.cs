using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
    public class SurveyManager
    {
        private Survey survey;
        private List<string> surveyTemplates;

        public bool SaveSurvey(Survey survey, string name)
        {
            return true;
        }

        public Survey LoadSurvey(string name)
        {
            return new Survey();
        }

        public List<string> GetTemplates(string fileName)
        {
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