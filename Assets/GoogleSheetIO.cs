using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

[RequireComponent(typeof(GoogleAuthorization))]

public class GoogleSheetIO : MonoBehaviour {
    public static String ApplicationName = "Unity Google Sheet";
    static String[] scopes = { SheetsService.Scope.Spreadsheets };
    SheetsService service;
    UserCredential credential;
    bool serviceRunOnce = true;
    private void StartService() {
        if (serviceRunOnce) {
            credential = gameObject.GetComponent<GoogleAuthorization>().GoogleAuthorizationBegin(scopes);
            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            serviceRunOnce = false;
        }
    }
    public void AppendData(String sheetID, ValueRange valueRange, String range) {
        StartService();
        SpreadsheetsResource.ValuesResource.AppendRequest request = service.Spreadsheets.Values.Append(valueRange, sheetID, range);
        //request.ValueInputOption = valueInputOption;
        //request.InsertDataOption = insertDataOption;

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
        request.Execute();
    }
    public ValueRange GetData(String sheetID, String range)
    {
        StartService();
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(sheetID, range);

        ValueRange response = request.Execute();
        return response;
    }
}