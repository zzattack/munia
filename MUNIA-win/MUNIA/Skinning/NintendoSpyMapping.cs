using System.Collections.Generic;
using MUNIA.Controllers;

namespace MUNIA.Skinning {
	
	static class NintendoSpyMapping {
		public class ControllerMap {
			public Dictionary<string, int> ButtonMap = new Dictionary<string, int>();
			public Dictionary<string, int> AxisMap = new Dictionary<string, int>();
		}

		public static readonly Dictionary<string, ControllerType> TypeMap = new Dictionary<string, ControllerType> {
			["n64"] = ControllerType.N64,
			["gamecube"] = ControllerType.NGC,
			["snes"] = ControllerType.SNES,
			// ["pc360"] = ControllerType.Unknown, // todo
			// ["classiccontroller"] = ControllerType.ClassicController, // todo
		};

		// mapping between NintendoSpy and MUNIA button/axes indices
		public static Dictionary<ControllerType, ControllerMap> ControllerMaps = new Dictionary<ControllerType, ControllerMap> {

			// SNES controller mapping
			[ControllerType.SNES] = new ControllerMap {
				ButtonMap = {
					["a"] = 7,
					["b"] = 3,
					["x"] = 6,
					["y"] = 2,
					["l"] = 5,
					["r"] = 4,
					["start"] = 0,
					["select"] = 1,
					["up"] = 8,
					["down"] = 9,
					["left"] = 10,
					["right"] = 11,
				}
			},

			// N64 controller mapping
			[ControllerType.N64] = new ControllerMap {
				ButtonMap = {
					["a"] = 3,
					["b"] = 2,
					["z"] = 1,
					["l"] = 9,
					["r"] = 8,
					["start"] = 0,
					["cup"] = 7,
					["cdown"] = 6,
					["cleft"] = 5,
					["cright"] = 4,
					["up"] = 10,
					["down"] = 11,
					["left"] = 12,
					["right"] = 13,
				},
				AxisMap = {
					["stick_x"] = 0,
					["stick_y"] = 1,
				}
			},

			// NGC controller mapping
			[ControllerType.NGC] = new ControllerMap {
				ButtonMap = {
					["a"] = 0,
					["b"] = 1,
					["x"] = 2,
					["y"] = 3,
					["start"] = 4,
					["z"] = 5,
					["l"] = 7,
					["r"] = 6,
					["up"] = 8,
					["down"] = 9,
					["left"] = 10,
					["right"] = 11,
				},
				AxisMap = {
					["lstick_x"] = 0,
					["lstick_y"] = 1,
					["cstick_x"] = 3,
					["cstick_y"] = 4,
					["trig_l"] = 2,
					["trig_r"] = 5,
				}
			},


		};	
	}
}
