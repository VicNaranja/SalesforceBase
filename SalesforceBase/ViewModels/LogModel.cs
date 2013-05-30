using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceBase.ViewModels
{
    public class LogModel : INotifyPropertyChanged
    {
        private string _id;
        /// <summary>
        /// Tipo del log
        /// </summary>
        /// <returns></returns>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private string _tipoLog;
        /// <summary>
        /// Tipo del log
        /// </summary>
        /// <returns></returns>
        public string TipoLog
        {
            get
            {
                return _tipoLog;
            }
            set
            {
                if (value != _tipoLog)
                {
                    _tipoLog = value;
                    NotifyPropertyChanged("TipoLog");
                }
            }
        }


        private string _app;
        /// <summary>
        /// Nombre de la aplicación
        /// </summary>
        /// <returns></returns>
        public string App
        {
            get
            {
                return _app;
            }
            set
            {
                if (value != _app)
                {
                    _app = value;
                    NotifyPropertyChanged("App");
                }
            }
        }



        private string _descripcion;
        /// <summary>
        /// Descripcion del log
        /// </summary>
        /// <returns></returns>
        public string Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                if (value != _descripcion)
                {
                    _descripcion = value;
                    NotifyPropertyChanged("Descripcion");
                }
            }
        }
        




        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
