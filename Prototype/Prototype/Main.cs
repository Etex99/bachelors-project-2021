using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{	
	//Singleton Main class
	public class Main
	{
		private static Main instance = null;
		public enum MainState
		{
			Default = 0,
			Browsing = 1,
			Editing = 2,
			Hosting = 3,
			Participating = 4
		}
		private MainState state;

		//private SurveyManager survey;
		//private SurveyClient client;
		//private SurveyHost host;
		
		private Main() {
			state = MainState.Default;
		}
		
		public void BrowseSurveys() {
			Console.WriteLine($"DEBUG: Browsing surveys from file location: [insert folder path here]");
			state = MainState.Browsing;
		}

		public void EditSurvey() {
			Console.WriteLine($"DEBUG: Editing selected survey: [insert name of surveymanager survey here]");
			state = MainState.Editing;
		}

		public void JoinSurvey(string RoomCode) {
			Console.WriteLine($"DEBUG: Attempting new client instance with RoomCode: {RoomCode}");
			state = MainState.Participating;
			//client = new SurveyClient(RoomCode);
			//...

		}

		public void HostSurvey() {
			Console.WriteLine($"DEBUG: Creating new host instance with selected survey: [insert name of surveymanager survey here]");
			//host = new SurveyHost(surveyMan.GetSurvey());
			//...
		}

		public MainState GetMainState() {
			return state;
		}
		
		//Singleton instance getter
		public static Main GetInstance() {
			if (instance != null) {
				return instance;
			}
			instance = new Main();
			return instance;
		}
	}
}
