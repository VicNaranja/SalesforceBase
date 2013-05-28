using SalesforceBase.SFDC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceBase.ViewModels
{
    public class ListaLogsModel : INotifyPropertyChanged
    {
        public ObservableCollection<LogModel> ListaLogs { get; set; }

        public ListaLogsModel()
        {
            ListaLogs = new ObservableCollection<LogModel>();
        }

        public async void obtenerUltimosLogs()
        {
            var sfdcApi = new SFDCRestApi();
            var jsonResult = await sfdcApi.Request("GET", "query?q=SELECT+id,Aplicacion__c,Tipo_Log__c,Descripcion__c+FROM+LOG__c+ORDER+BY+CreatedDate+desc+limit+100 ");

            var records = jsonResult["records"];

            foreach (var log in records)
            {
                ListaLogs.Add(new LogModel
                {
                    App = log["Aplicacion__c"].ToString(),
                    TipoLog = log["Tipo_Log__c"].ToString(),
                    Descripcion = log["Descripcion__c"].ToString(),
                });
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
