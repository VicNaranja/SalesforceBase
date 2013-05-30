using Newtonsoft.Json.Linq;
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
            ListaLogs.Clear();

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


        //TEST Modifica un objeto log en salesforce
        public async void modificarLog(LogModel log)
        {
            //creamos un objeto con los campos que queremos modificar
            var updateLog = new JObject();
            updateLog.Add("Tipo_Log__c",log.TipoLog);
            updateLog.Add("Aplicacion__c",log.App);
            updateLog.Add("Descripcion__c",log.Descripcion);

            var sfdcApi = new SFDCRestApi();
            var jsonResult = await sfdcApi.Request("PATCH", "sobjects/LOG__c/" + log.Id, updateLog.ToString());

        }

        //TEST Inserta un objeto log en salesforce
        public async void insertarLog(LogModel log)
        {
            //creamos un objeto con los campos que queremos sincronizar
            var updateLog = new JObject();
            updateLog.Add("Tipo_Log__c", log.TipoLog);
            updateLog.Add("Aplicacion__c", log.App);
            updateLog.Add("Descripcion__c", log.Descripcion);

            var sfdcApi = new SFDCRestApi();
            var jsonResult = await sfdcApi.Request("POST", "sobjects/LOG__c", updateLog.ToString());

        }


        //TEST Inserta un objeto log en salesforce
        public async void borrarLog(LogModel log)
        {
            var sfdcApi = new SFDCRestApi();
            var jsonResult = await sfdcApi.Request("DELETE", "sobjects/LOG__c/" + log.Id);

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
