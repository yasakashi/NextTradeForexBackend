using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Base.Common.Export
{
    /// <summary>
    /// Export Report To Excel
    /// </summary>
    public class Export2Excel
    {
        public void CreateExcel(string fileName, string reportTitle,string tablename, List<string> titles, Dictionary<int, List<string>> values)
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                worksheet.Cell("A1").Value = reportTitle;
                worksheet.Range(2, 1, 2, titles.Count).Merge().AddToNamed("Titles").Value = tablename;
                worksheet.Range(3, 1, 3, titles.Count).AddToNamed("Titles");
                int col = 1;
                
                foreach (string title in titles)
                {
                    worksheet.Cell(3, col++).Value = title;
                }
                int row = 4;
                foreach (KeyValuePair<int, List<string>> kvp in values)
                {
                    col = 1;
                    foreach (string val in kvp.Value)
                    {
                        worksheet.Cell(row, col++).Value = val;
                    }
                    row++;
                }
                // write datat to sheet
                //var rangeWithData = worksheet.Cell(4, 1).InsertData(dataTable.AsEnumerable());


                var titlesStyle = workbook.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titlesStyle.Fill.BackgroundColor = XLColor.Cyan;

                // Format all titles in one shot
                workbook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

                workbook.SaveAs(fileName);
            }
        }

        public void CreateExcel(string fileName, string reportTitle, string tablename, List<string> titles, Dictionary<int, List<string>> values, List<string> titlesdetail, Dictionary<int, List<string>> valuesdetail)
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                worksheet.Cell("A1").Value = reportTitle;
                worksheet.Range(2, 1, 2, titles.Count).Merge().AddToNamed("Titles").Value = tablename;
                worksheet.Range(3, 1, 3, titles.Count).AddToNamed("Titles");
                int col = 1;
                foreach (string title in titles)
                {
                    worksheet.Cell(3, col++).Value = title;
                }
                int row = 4;
                foreach (KeyValuePair<int, List<string>> kvp in values)
                {
                    col = 1;
                    foreach (string val in kvp.Value)
                    {
                        worksheet.Cell(row, col++).Value = val;
                    }
                    row++;
                }
                row++;
                col = 1;
                foreach (string title in titlesdetail)
                {
                    worksheet.Cell(row, col++).Value = title;
                }
                row++;
                foreach (KeyValuePair<int, List<string>> kvp in valuesdetail)
                {
                    col = 1;
                    foreach (string val in kvp.Value)
                    {
                        worksheet.Cell(row, col++).Value = val;
                    }
                    row++;
                }
                // write datat to sheet
                //var rangeWithData = worksheet.Cell(4, 1).InsertData(dataTable.AsEnumerable());


                var titlesStyle = workbook.Style;
                titlesStyle.Font.Bold = true;
                titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titlesStyle.Fill.BackgroundColor = XLColor.Cyan;

                // Format all titles in one shot
                workbook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

                workbook.SaveAs(fileName);
            }
        }
    }

}

