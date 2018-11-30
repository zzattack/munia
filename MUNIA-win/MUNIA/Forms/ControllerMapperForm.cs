using System.Windows.Forms;

namespace MUNIA.Forms {
	public partial class ControllerMapperForm : Form {
		public ControllerMapperForm() {
			InitializeComponent();

			this.mapperManager.MappingSelected += OnMappingSelected;
		}

		private void OnMappingSelected(object sender, MappingSelectionArgs e) {
			Controls.Remove(mapperManager);
			var mapEditor = new ControllerMapperEditor(e.Controller, e.Target, e.PreviewSkin, e.Mapping);
			mapEditor.Dock = DockStyle.Fill;
			Controls.Add(mapEditor);
			mapEditor.FinishEditing += (o, args) => {
				ConfigManager.Save();
				Close();
			};
		}
	}
}
