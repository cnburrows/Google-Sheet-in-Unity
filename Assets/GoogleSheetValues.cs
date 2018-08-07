using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetValues : MonoBehaviour
{
    public GoogleSheetIO sheetService;
    public String sheetID;
    public String pageName = "";
    public String cellRange = "";
    [Serializable]
    public class Data
    {
        public String[] columns;
    }
    public Data[] rows;
    private ValueRange GoogleValues()
    {
        ValueRange valueRange = new ValueRange();
        valueRange.Values = new List<IList<object>>();
        valueRange.MajorDimension = "ROWS";
        foreach (Data d in rows)
        {
            var oblist = new List<object>();
            foreach (String s in d.columns)
            {
                oblist.Add(s);
            }
            valueRange.Values.Add(oblist);
        }
        return valueRange;
    }
    public void AppendGoogleValues()
    {
        sheetService.AppendData(sheetID, GoogleValues(), pageName + "!" + cellRange);
    }
    public void GetGoogleValues()
    {
        ValueRange valueRange = sheetService.GetData(sheetID, pageName + "!" + cellRange);
        rows = new Data[valueRange.Values.Count];
        for (int i1 = 0; i1 < valueRange.Values.Count; i1++)
        {
            Data data = new Data();
            IList<object> l = valueRange.Values[i1];
            data.columns = new String[l.Count];
            for (int i2 = 0; i2 < l.Count; i2++)
            {
                data.columns[i2] = l[i2].ToString();
            }
            rows[i1] = data;
        }
    }
    public void ChangeGoogleValue(int row, int column, String value)
    {
        row -= 1;
        column -= 1;
        if (rows.Length > row && rows[row].columns.Length > column)
        {
            rows[row].columns[column] = value;
        }
        else
        {
            throw new System.ArgumentException("Row and column index must be within array range");
        }
    }
    public void ChangeRowSize(int rowSize)
    {
        Data[] oldRows = rows;
        rows = new Data[rowSize];
        for (int i = 0; i < rows.Length; i++)
        {
            if (i < oldRows.Length)
            {
                rows[i] = oldRows[i];
            }
        }
    }
    public void ChangeColumnSize(int row, int columnSize)
    {
        row -= 1;
        String[] oldColumns = rows[row].columns;
        rows[row].columns = new string[columnSize];
        for (int i = 0; i < rows[row].columns.Length; i++)
        {
            if (i < oldColumns.Length)
            {
                rows[row].columns[i] = oldColumns[i];
            }
        }
    }
}
