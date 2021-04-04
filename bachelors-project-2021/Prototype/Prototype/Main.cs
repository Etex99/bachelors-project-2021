using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
		public MainState state;
		public SurveyClient client = null;
		public SurveyHost host = null;
		
		private Main() {
			state = MainState.Default;
		}
		
		public void BrowseSurveys() {
			Console.WriteLine($"DEBUG: Browsing surveys");
			state = MainState.Browsing;
		}

		public void EditSurvey() {
			Console.WriteLine($"DEBUG: Editing selected survey");
			state = MainState.Editing;
		}

		public async Task<bool> JoinSurvey(string RoomCode) {
			Console.WriteLine($"DEBUG: Attempting new client instance with RoomCode: {RoomCode}");
			state = MainState.Participating;
			client = new SurveyClient();
			return await Task.Run(() => client.LookForHost(RoomCode));
		}

		public void HostSurvey() {
			Console.WriteLine($"DEBUG: Creating new host instance with selected survey");
			state = MainState.Hosting;
			host = new SurveyHost();
			Task.Run(() => host.RunSurvey());
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
