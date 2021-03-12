using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public static class Const
	{
		public static List<string> intros = new List<string>() {
			"intro1",
			"intro2",
			"intro3",
			"intro4",
			"intro5"
		};

		public static Dictionary<int, List<string>> activities = new Dictionary<int, List<string>>() {
			{ 0, new List<string>() {
				"emoji 0 activity 1",
				"emoji 0 activity 2",
				"emoji 0 activity 3",
				"emoji 0 activity 4",
				"emoji 0 activity 5"
			}},
			{ 1, new List<string>() {
				"emoji 1 activity 1",
				"emoji 1 activity 2",
				"emoji 1 activity 3",
				"emoji 1 activity 4",
				"emoji 1 activity 5"
			}},
			{ 2, new List<string>() {
				"emoji 2 activity 1",
				"emoji 2 activity 2",
				"emoji 2 activity 3",
				"emoji 2 activity 4",
				"emoji 2 activity 5"
			}},
			{ 3, new List<string>() {
				"emoji 3 activity 1",
				"emoji 3 activity 2",
				"emoji 3 activity 3",
				"emoji 3 activity 4",
				"emoji 3 activity 5"
			}},
			{ 4, new List<string>() {
				"emoji 4 activity 1",
				"emoji 4 activity 2",
				"emoji 4 activity 3",
				"emoji 4 activity 4",
				"emoji 4 activity 5"
			}},
			{ 5, new List<string>() {
				"emoji 5 activity 1",
				"emoji 5 activity 2",
				"emoji 5 activity 3",
				"emoji 5 activity 4",
				"emoji 5 activity 5"
			}},
			{ 6, new List<string>() {
				"emoji 6 activity 1",
				"emoji 6 activity 2",
				"emoji 6 activity 3",
				"emoji 6 activity 4",
				"emoji 6 activity 5"
			}}
		};
	}
}
