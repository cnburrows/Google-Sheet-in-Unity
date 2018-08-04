using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetValues : MonoBehaviour {
    public GoogleSheetIO sheetService;
    public String sheetID;
    public String pageName = "";
    public String cellRange = "";
    public bool organizeByRow = true;
    [Serializable]
    public class Data
    {
        public String[] dataInner;
    }
    public Data[] dataOuter;
    private ValueRange GoogleValues()
    {
        ValueRange valueRange = new ValueRange();
        valueRange.Values = new List<IList<object>>();
        if (organizeByRow) {
            valueRange.MajorDimension = "ROWS";
        }else {
            valueRange.MajorDimension = "COLUMN";
        }
        foreach (Data d in dataOuter) {
            var oblist = new List<object>();
            foreach (String s in d.dataInner) {
                oblist.Add(s);
            }
            valueRange.Values.Add(oblist);
        }
        return valueRange;
    }
    public void AppendGoogleValues()
    {
        sheetService.AppendData(sheetID, GoogleValues(), pageName+"!"+cellRange);
    }
    public void GetGoogleValues()
    {
        ValueRange valueRange = sheetService.GetData(sheetID, pageName + "!" + cellRange);
        dataOuter = new Data[valueRange.Values.Count];
        for(int i1 = 0; i1 < valueRange.Values.Count; i1++) {
            Data data = new Data();
            IList<object> l = valueRange.Values[i1];
            data.dataInner = new String[l.Count];
            for (int i2 = 0; i2 < l.Count; i2++) {
                data.dataInner[i2] = l[i2].ToString();
            }
            dataOuter[i1] = data;
        }
    }
}
