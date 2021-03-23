using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public class Emoji
	{
		public int ID {get; set;} = 0;
		public string Name { get; set; } = "default";
		public string Impact { get; set; } = "default";
		public IList<string> activities { get; set; } = null;
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
 
