using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype
{
	public static class Const
	{
		public static int vote1PerEmojiTime = 10;
		public static int vote2Time = 20;

		public static List<string> intros = new List<string>() {
			"Tänään minusta tuntuu",
			"Minun päiväni on ollut",
			"Tämä tuntui mielestäni",
			"Mitä mieltä olit tunnista?",
			"Tämä tunti oli mielestäni"
		};

		public static Dictionary<int, List<string>> activities = new Dictionary<int, List<string>>() {
			{ 0, new List<string>() {
				"Jokainen kertoo mikä oli kivaa",
				"Kehu vieressä istuvaa",
			}},
			{ 1, new List<string>() {
				"Kerrotaan ohjaajalle mikä hämmästyttää",
				"Kerrotaan ryhmälle mikä hämmästyttää",
			}},
			{ 2, new List<string>() {
				"Jokainen sanoo jonkin iloisen asian",
				"Jokainen kertoo ohjaajalle yhden mietteen",
				"Jokainen kertoo ryhmälle yhden mietteen",
			}},
			{ 3, new List<string>() {
				"Positiivinen palloleikki",
				"Kirjoitetaan ohjaajalle salattu lappu",
			}},
			{ 4, new List<string>() {
				"5 minuutin tauko",
				"Siirretään oppitunti ulos",
			}},
			{ 5, new List<string>() {
				"Jokainen kertoo yhden asia mikä mietityttää",
				"Jokainen kysyy kysymyksen ohjaajalta",
				"Jokainen kysyy kysymyksen ryhmältä",
			}},
			{ 6, new List<string>() {
				"Matkustetaan kuuhun",
				"Katsotaan video",
				"Kerrotaan vitsi",
			}}
		};
	
		public static class Network {
			public static int ServerUDPClientPort = 43256;
			public static int ServerTCPListenerPort = 43257;
			public static int ClientUDPClientPort = 43258;
		}
	}
}
