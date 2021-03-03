using Google.Apis.Sheets.v4;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System;

namespace SheetsApi.Client.Sheets
{
    public class SheetsServiceClient
    {
        private readonly SheetsService _sheetsService;

        public SheetsServiceClient(string appName, IConfigurableHttpClientInitializer clientInitializer)
        {
            _sheetsService = new SheetsService(
    			new BaseClientService.Initializer
				{
					HttpClientInitializer = clientInitializer,
					ApplicationName = appName
				});
        }

        public IList<IList<Object>> GetCells(DataRequest dataRequest)
        {            
            SpreadsheetsResource.ValuesResource.GetRequest request =
                _sheetsService.Spreadsheets.Values.Get(dataRequest.SpreadsheetId, dataRequest.Range);

            ValueRange response = request.Execute();
            
            return response.Values;            
        }
    }
}