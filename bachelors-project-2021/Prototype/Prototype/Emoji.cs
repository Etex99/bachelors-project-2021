using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public class Emoji
	{
		public int ID = 0;
		public string Name = "default";
		public string Impact = "default";
		public List<string> activities = null;
		public string ImageSource { get; set; } = "missing.png";

		public Emoji()
		{
			activities = new List<string>() { "foo", "bar" };
		}

		public Emoji(int ID, string Name, string impact, List<string> activities, string ImageSource)
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
 
