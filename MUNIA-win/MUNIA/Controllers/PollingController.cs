using System;
using System.Threading;

namespace MUNIA.Controllers {
	public class PollingController : IController {
		private IController _controller;

		public PollingController(IController controller, int updateDelay) {
			_controller = controller;
			_updateDelay = updateDelay;
			if (!controller.RequiresPolling)
				throw new InvalidOperationException("Use me only on controllers that require polling");
		}

		public ControllerState GetState() {
			return _polledState;
		}

		public bool IsAvailable => _controller.IsAvailable;

		public string DevicePath => _controller.DevicePath;

		public string Name => _controller.Name;

		public ControllerType Type => _controller.Type;

		public bool RequiresPolling => false;
		public bool IsAxisTrigger(int axisNum) {
			return _controller.IsAxisTrigger(axisNum);
		}

		public bool Activate() {
			if (!_controller.Activate()) return false;
			StartPollingThread();
			return true;
		}

		public void Deactivate() {
			_controller.Deactivate();
			StopPollingThread();
		}

		public event EventHandler StateUpdated;

		private readonly int _updateDelay;
		private bool _killThread;
		private Thread _pollerThread;
		private AutoResetEvent _are = new AutoResetEvent(false);
		private ControllerState _polledState;

		private void StartPollingThread() {
			_are.Reset();
			if (_pollerThread == null) {
				_pollerThread = new Thread(PollerThread);
				_pollerThread.IsBackground = true;
				_pollerThread.Start();
			}
		}

		private void PollerThread() {
			while (!_killThread) {
				var newState = _controller.GetState();
				if (!Equals(newState, _polledState)) {
					_polledState = newState;
					OnStateUpdated();
				}
				_are.WaitOne(_updateDelay);
			}
		}

		private void StopPollingThread() {
			if (_pollerThread != null) {
				_killThread = true;
				_are.Set();
				_pollerThread.Join();
				_pollerThread = null;
			}
		}

		protected virtual void OnStateUpdated() {
			StateUpdated?.Invoke(this, EventArgs.Empty);
		}

		public void Dispose() {
			Deactivate();
			_controller.Dispose();
		}
	}

}