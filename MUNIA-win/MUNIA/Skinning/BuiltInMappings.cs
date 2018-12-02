using System;
using System.Collections.Generic;
using MUNIA.Controllers;

namespace MUNIA.Skinning {
	public static class BuiltInMappings {

		public static IEnumerable<ControllerMapping> Get() {
			yield return CreateXInputSNES();
			yield return CreateXInputN64();
			yield return CreateXInputNGC();
			yield return CreateXInputPS2();
		}

		private static ControllerMapping CreateXInputSNES() => new ControllerMapping {
			UUID = Guid.Parse("92137e4b-c6ac-4305-b897-e9c4a49aed33"),
			IsBuiltIn = true,
			Source = ControllerMapping.SourceType.XInput,
			MappedType = ControllerType.SNES,
			AxisMaps = new List<ControllerMapping.AxisMap> { },
			ButtonMaps = new List<ControllerMapping.ButtonMap> {
				new ControllerMapping.ButtonMap(0, 3),
				new ControllerMapping.ButtonMap(1, 7),
				new ControllerMapping.ButtonMap(2, 2),
				new ControllerMapping.ButtonMap(3, 6),
				new ControllerMapping.ButtonMap(4, 5),
				new ControllerMapping.ButtonMap(5, 4),
				new ControllerMapping.ButtonMap(6, 1),
				new ControllerMapping.ButtonMap(7, 0),
				new ControllerMapping.ButtonMap(11, 8),
				new ControllerMapping.ButtonMap(12, 9),
				new ControllerMapping.ButtonMap(13, 10),
				new ControllerMapping.ButtonMap(14, 11),
			}
		};

		private static ControllerMapping CreateXInputN64() => new ControllerMapping {
			UUID = Guid.Parse("b2d119c8-5c05-416a-a068-fc573449c063"),
			IsBuiltIn = true,
			Source = ControllerMapping.SourceType.XInput,
			MappedType = ControllerType.N64,
			AxisMaps = new List<ControllerMapping.AxisMap> {
				new ControllerMapping.AxisMap(0, 0, false),
				new ControllerMapping.AxisMap(1, 1, false),
			},
			ButtonMaps = new List<ControllerMapping.ButtonMap> {
				new ControllerMapping.ButtonMap(0, 3),
				new ControllerMapping.ButtonMap(2, 2),
				new ControllerMapping.ButtonMap(4, 9),
				new ControllerMapping.ButtonMap(5, 8),
				new ControllerMapping.ButtonMap(6, 1),
				new ControllerMapping.ButtonMap(7, 0),
				new ControllerMapping.ButtonMap(11, 10),
				new ControllerMapping.ButtonMap(12, 11),
				new ControllerMapping.ButtonMap(13, 12),
				new ControllerMapping.ButtonMap(14, 13),
			},
			AxisToButtonMaps = new List<ControllerMapping.AxisToButtonMap> {
				new ControllerMapping.AxisToButtonMap(3, 4, 0.2),
				new ControllerMapping.AxisToButtonMap(3, 5, -0.2),
				new ControllerMapping.AxisToButtonMap(4, 6, 0.2),
				new ControllerMapping.AxisToButtonMap(4, 7, -0.2),
			},
		};

		private static ControllerMapping CreateXInputNGC() => new ControllerMapping {
			UUID = Guid.Parse("8e822109-dbfc-4cc2-9099-8f5f11f9157f"),
			IsBuiltIn = true,
			Source = ControllerMapping.SourceType.XInput,
			MappedType = ControllerType.NGC,
			AxisMaps = new List<ControllerMapping.AxisMap> {
				new ControllerMapping.AxisMap(0, 0, false),
				new ControllerMapping.AxisMap(1, 1, false),
				new ControllerMapping.AxisMap(2, 2, true),
				new ControllerMapping.AxisMap(3, 3, false),
				new ControllerMapping.AxisMap(4, 4, false),
				new ControllerMapping.AxisMap(5, 5, true),
			},
			AxisToButtonMaps = new List<ControllerMapping.AxisToButtonMap> {
				new ControllerMapping.AxisToButtonMap(2, 7, 0.7), // trigger L
				new ControllerMapping.AxisToButtonMap(5, 6, 0.7), // trigger R
			},
			ButtonMaps = new List<ControllerMapping.ButtonMap> {
				new ControllerMapping.ButtonMap(0, 0), // A
				new ControllerMapping.ButtonMap(1, 2), // B
				new ControllerMapping.ButtonMap(2, 1), // X
				new ControllerMapping.ButtonMap(3, 3), // Y
				new ControllerMapping.ButtonMap(5, 5), // Start
				new ControllerMapping.ButtonMap(7, 4), // Right shoulder->Z
				new ControllerMapping.ButtonMap(11, 8), // D-up
				new ControllerMapping.ButtonMap(12, 9), // D-down
				new ControllerMapping.ButtonMap(13, 10), // D-left
				new ControllerMapping.ButtonMap(14, 11), // D-right
			},
		};

		private static ControllerMapping CreateXInputPS2() => new ControllerMapping {
			UUID = Guid.Parse("23fa5623-bdb8-46c1-9d0b-fc8d4e33286b"),
			IsBuiltIn = true,
			Source = ControllerMapping.SourceType.XInput,
			MappedType = ControllerType.PS2,
			AxisMaps = new List<ControllerMapping.AxisMap> {
				new ControllerMapping.AxisMap(0, 0, false),
				new ControllerMapping.AxisMap(1, 1, false),
				new ControllerMapping.AxisMap(2, 2, true),
				new ControllerMapping.AxisMap(3, 3, false),
				new ControllerMapping.AxisMap(4, 4, false),
				new ControllerMapping.AxisMap(5, 5, true),
			},
			AxisToButtonMaps = new List<ControllerMapping.AxisToButtonMap> {
				new ControllerMapping.AxisToButtonMap(2, 10, 0.7), // trigger L
				new ControllerMapping.AxisToButtonMap(5, 11, 0.7), // trigger R
			},
			ButtonMaps = new List<ControllerMapping.ButtonMap> {
				new ControllerMapping.ButtonMap(0, 0), // cross
				new ControllerMapping.ButtonMap(1, 2), // square
				new ControllerMapping.ButtonMap(2, 1), // circle
				new ControllerMapping.ButtonMap(3, 3), // triangle
				new ControllerMapping.ButtonMap(4, 8), // select
				new ControllerMapping.ButtonMap(5, 9), // start
				new ControllerMapping.ButtonMap(6, 4), // left stick button
				new ControllerMapping.ButtonMap(7, 5), // right stick button
				new ControllerMapping.ButtonMap(8, 6), // left shoulder
				new ControllerMapping.ButtonMap(9, 7), // right shoulder
				new ControllerMapping.ButtonMap(11, 12), // D-up
				new ControllerMapping.ButtonMap(12, 13), // D-down
				new ControllerMapping.ButtonMap(13, 14), // D-left
				new ControllerMapping.ButtonMap(14, 15), // D-right
			},
		};

	}
}
