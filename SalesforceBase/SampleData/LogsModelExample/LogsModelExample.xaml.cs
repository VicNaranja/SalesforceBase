//      *********    NO MODIFIQUE ESTE ARCHIVO     *********
//      Este archivo se regenera mediante una herramienta de diseño.
//       Si realiza cambios en este archivo, puede causar errores.
namespace Expression.Blend.SampleData.LogsModelExample
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class LogsModelExample { }
#else

	public class LogsModelExample : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public LogsModelExample()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/SalesforceBase;component/SampleData/LogsModelExample/LogsModelExample.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private ListaLogs _ListaLogs = new ListaLogs();

		public ListaLogs ListaLogs
		{
			get
			{
				return this._ListaLogs;
			}
		}
	}

	public class ListaLogsItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _App = string.Empty;

		public string App
		{
			get
			{
				return this._App;
			}

			set
			{
				if (this._App != value)
				{
					this._App = value;
					this.OnPropertyChanged("App");
				}
			}
		}

		private string _TipoLog = string.Empty;

		public string TipoLog
		{
			get
			{
				return this._TipoLog;
			}

			set
			{
				if (this._TipoLog != value)
				{
					this._TipoLog = value;
					this.OnPropertyChanged("TipoLog");
				}
			}
		}

		private string _Descripcion = string.Empty;

		public string Descripcion
		{
			get
			{
				return this._Descripcion;
			}

			set
			{
				if (this._Descripcion != value)
				{
					this._Descripcion = value;
					this.OnPropertyChanged("Descripcion");
				}
			}
		}
	}

	public class ListaLogs : System.Collections.ObjectModel.ObservableCollection<ListaLogsItem>
	{ 
	}
#endif
}
