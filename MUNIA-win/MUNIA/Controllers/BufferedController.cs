using System;
using System.Collections.Generic;
using System.Diagnostics;
using MuniaInput;

namespace MUNIA.Controllers {
	public class BufferedController : MuniaController {
		private readonly MuniaController _real;
		private readonly Stopwatch _sw = Stopwatch.StartNew();
		private readonly Queue<ControllerState> _q = new Queue<ControllerState>();


		public BufferedController(MuniaController real) : base(null) {
			_real = real;
			_real.StateUpdated += RealOnStateUpdated;
		}

		private void RealOnStateUpdated(object sender, EventArgs eventArgs) {
			// todo: update queue
			
		}

		private void CleanQueue() {
			// todo: remove stuff that is older than what we'd ever request
		}

		public override List<int> Axes { get; } // todo: look back in queue
		public override List<bool> Buttons { get; } // todo: look back in queue
		public TimeSpan Delay { get; set; }

		protected override bool Parse(byte[] ev) {
			throw new InvalidOperationException();
		}
	}

	public class ControllerState {
		public ControllerState(TimeSpan t, MuniaController c) {
			Time = t;
			Axes.AddRange(c.Axes);
			Buttons.AddRange(c.Buttons);
		}

		public TimeSpan Time { get; private set; }
		public List<int> Axes { get; } = new List<int>();
		public List<bool> Buttons { get; } = new List<bool>();
	}

}
