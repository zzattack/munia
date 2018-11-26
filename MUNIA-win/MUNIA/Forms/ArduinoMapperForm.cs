using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Interop;

namespace MUNIA.Forms {
	public partial class ArduinoMapperForm : Form {
		public readonly ArduinoMapping Mapping;

		private ArduinoMapperForm() {
			InitializeComponent();
			cbDeviceType.DataSource = Enum.GetValues(typeof(ControllerType));

			lbSerialPorts.SelectedValueChanged += lbSerialPorts_SelectedValueChanged;
			cbDeviceType.SelectedValueChanged += cbDeviceType_SelectedValueChanged;
		}

		public ArduinoMapperForm(ArduinoMapping mapping) : this() {
			this.Mapping = mapping.Clone();
			UsbNotification.DeviceArrival += UsbDeviceNotification;
			UsbNotification.DeviceRemovalComplete += UsbDeviceNotification;
			RefreshListbox();
		}

		private void UsbDeviceNotification(object sender, UsbNotificationEventArgs args) {
			if (args.DeviceType == DeviceType.DBT_DEVTYP_PORT)
				RefreshListbox();
		}

		private void RefreshListbox() {
			lbSerialPorts.Items.Clear();
			var ports = SerialPortInfo.GetPorts().ToList();
			foreach (var spi in ports) {
				lbSerialPorts.Items.Add(spi);
			}
			foreach (var entry in Mapping) {
				// if port was mapped, but now unavailable, don't hide the original mapping
				if (ports.All(p => p.Name != entry.Key))
					lbSerialPorts.Items.Add(new SerialPortInfo { Name = entry.Key, Description = "Unavailable" });
			}
			if (!ports.Any() && !Mapping.Any())
				lbSerialPorts.Items.Add("None available");
		}

		private void lbSerialPorts_SelectedValueChanged(object sender, EventArgs e) {
			var spi = lbSerialPorts.SelectedItem as SerialPortInfo;
			gb.Enabled = spi != null;
			if (spi == null) return;
			if (Mapping.ContainsKey(spi.Name))
				cbDeviceType.SelectedItem = Mapping[spi.Name];
			else
				cbDeviceType.SelectedItem = ControllerType.None;
		}

		private void cbDeviceType_SelectedValueChanged(object sender, EventArgs e) {
			var spi = lbSerialPorts.SelectedItem as SerialPortInfo;
			if (spi != null)
				Mapping[spi.Name] = (ControllerType)cbDeviceType.SelectedItem;
		}

		private void btnTest_Click(object sender, EventArgs e) {
			var dev = ArduinoController.CreateDevice(lbSerialPorts.SelectedItem as SerialPortInfo, (ControllerType)cbDeviceType.SelectedItem);
			if (dev != null) {
				new GamepadTesterForm(dev).ShowDialog();
			}
		}
	}

	public class ArduinoMapping : Dictionary<string, ControllerType> {
		public ArduinoMapping() {
		}

		public ArduinoMapping(IDictionary<string, ControllerType> dictionary) : base(dictionary) { }

		public ArduinoMapping Clone() {
			return new ArduinoMapping(this);
		}
	}
}
