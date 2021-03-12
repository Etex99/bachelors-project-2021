using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public class Emoji
	{
		public int ID = 0;
		public string Name = "default";
		public int Impact = 0;
		public List<string> activities = null;
		public string Buttons { get; set; }
		public string ImageSource { get; set; } = "missing.txt";

		public Emoji()
		{
			activities = new List<string>() { "foo", "bar" };
		}

		public Emoji(int ID, string Name, int impact, List<string> activities, string ImageSource)
		{
			this.ID = ID;
			this.Name = Name;
			this.Impact = impact;
			this.activities = activities;
			this.ImageSource = ImageSource;
		}

		public override string ToString()
		{
			string value = "";

			value += $"ID: {ID}, ";
			value += $"Name: {Name}, ";
			value += $"Impact: {Impact}, ";

			value += "Activities: [";			
			foreach (var item in activities)
			{
				value += $"{item} ";
			}
			value += "], ";

			value += $"ImageSource: {ImageSource}";

			return value;
		}
	}
}
 
