using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PAN.Warranty.Models
{
    public class ExcelLib
    {
        public const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public ExcelLib() {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async global::System.Threading.Tasks.Task<List<T>> ReadImportTemplateAsync<T>(IFormFile formFile)
        {
            var dataList = new List<T>();
            if (formFile != null || formFile.Length > 0)
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Not Support file extension");
                }

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        try
                        {
                            var rowCount = worksheet.Dimension.Rows;
                            var colCount = worksheet.Dimension.Columns;
                            int headerRow = 1;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 1].Value.ToString() == "") break;
                                Type objType = typeof(T);
                                var obj = (T)Activator.CreateInstance(objType);
                                for (int col = 1; col <= colCount; col++)
                                {
                                    if (worksheet.Cells[headerRow, col].Value == null)
                                    {
                                        continue;
                                    }
                                    string headerName = worksheet.Cells[headerRow, col].Value.ToString();
                                    PropertyInfo propertyInfo = null;
                                    if (objType.GetProperties().Any(p => p.GetCustomAttributes().Any(
                                        o => o is ImportAttribute && ((o as ImportAttribute).DisplayName ?? "").Equals(headerName, StringComparison.OrdinalIgnoreCase)))
                                    )
                                    {
                                        propertyInfo = objType.GetProperties().Single(p => p.GetCustomAttributes().Any(o => o is ImportAttribute && ((o as ImportAttribute).DisplayName ?? "").Equals(headerName, StringComparison.OrdinalIgnoreCase)));
                                    }
                                    else
                                    {
                                        propertyInfo = objType.GetProperties().Single(p => p.Name.Equals(headerName, StringComparison.OrdinalIgnoreCase));
                                    }
                                    object value = worksheet.Cells[row, col].Value;
                                    try
                                    {
                                        propertyInfo.SetValue(obj, propertyInfo.GetType() == typeof(string) ? (value ?? "").ToString() : value);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.Message.Contains("cannot be converted") && ex.Message.Contains("System.DateTime"))
                                        {
                                            propertyInfo.SetValue(obj, DateTime.Parse(value.ToString(), CultureInfo.CreateSpecificCulture("vi-VN")));
                                        }
                                        else if (ex.Message.Contains("Double"))
                                        {
                                            double doubleValue = double.Parse(value.ToString());
                                            if (ex.Message.Contains("Int64"))
                                            {
                                                propertyInfo.SetValue(obj, (long)doubleValue);
                                            }
                                            else if (ex.Message.Contains("Int32"))
                                            {
                                                propertyInfo.SetValue(obj, (int)doubleValue);
                                            }
                                            else if (ex.Message.Contains("String"))
                                            {
                                                propertyInfo.SetValue(obj, value.ToString());
                                            }
                                            else throw ex;
                                        }
                                        else if (ex.Message.Contains("Boolean"))
                                        {
                                            if (ex.Message.Contains("String"))
                                            {
                                                propertyInfo.SetValue(obj, value.ToString());
                                            }
                                            else throw ex;
                                        }
                                        else throw ex;
                                    }
                                }
                                dataList.Add(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("{0}{1}Mẫu import không đúng định dạng, tải lại mẫu import từ hệ thống", ex.StackTrace, Environment.NewLine));
                        }
                    }
                }
            }
            return dataList;
        }

        public List<object[]> GetImportTemplateDataArray(Type sourceType)
        {
            var datasource = new List<object[]>();
            if (sourceType != null)
            {
                var headerData = new List<string>();
                foreach (PropertyInfo property in sourceType.GetProperties())
                {
                    if (property.GetCustomAttributes().Any(o => o is ImportAttribute))
                    {
                        var importAttribute = property.GetCustomAttributes().Single(o => o is ImportAttribute) as ImportAttribute;
                        headerData.Add(string.IsNullOrEmpty(importAttribute.DisplayName) ? property.Name : importAttribute.DisplayName);
                    }
                }
                datasource.Add(headerData.ToArray());
            }
            return datasource;
        }

        public List<object[]> GetExportDataArray<T>(List<T> source)
        {
            var datasource = new List<object[]>();
            if (source != null && source.Count > 0)
            {
                var headerData = new List<string>();
                for (int i = 0; i < source.Count; i++)
                {
                    var sourceObj = source[i];
                    var rowData = new List<object>();
                    foreach (PropertyInfo property in sourceObj.GetType().GetProperties())
                    {
                        if (property.GetCustomAttributes().Any(o => o is ExportAttribute))
                        {
                            var exportAttribute = property.GetCustomAttributes().Single(o => o is ExportAttribute) as ExportAttribute;
                            if (i == 0)
                            {
                                headerData.Add(string.IsNullOrEmpty(exportAttribute.DisplayName) ? property.Name : exportAttribute.DisplayName);
                            }
                            object propertyValue = property.GetValue(sourceObj);
                            if (!string.IsNullOrEmpty(exportAttribute.DisplayFormat))
                            {
                                rowData.Add(string.Format(string.Concat("{0:", exportAttribute.DisplayFormat, "}"), propertyValue));
                            }
                            else
                            {
                                rowData.Add(propertyValue);
                            }
                        }
                    }
                    if (i == 0)
                    {
                        datasource.Add(headerData.ToArray());
                    }
                    datasource.Add(rowData.ToArray());
                }
            }
            return datasource;
        }

        public DataTable GetExportDatatable(List<object> source)
        {
            var datasource = new DataTable();
            if (source != null && source.Count > 0)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var sourceObj = source[i];
                    if (i == 0)
                    {
                        foreach (PropertyInfo property in sourceObj.GetType().GetProperties())
                        {
                            if (property.GetCustomAttributes().Any(o => o is ExportAttribute))
                            {
                                var exportAttribute = property.GetCustomAttributes().Single(o => o is ExportAttribute) as ExportAttribute;
                                var columnName = string.IsNullOrEmpty(exportAttribute.DisplayName) ? property.Name : exportAttribute.DisplayName;
                                datasource.Columns.Add(columnName);
                            }
                        }
                    }
                    DataRow dataRow = datasource.NewRow();
                    foreach (PropertyInfo property in sourceObj.GetType().GetProperties())
                    {
                        if (property.GetCustomAttributes().Any(o => o is ExportAttribute))
                        {
                            var exportAttribute = property.GetCustomAttributes().Single(o => o is ExportAttribute) as ExportAttribute;
                            var columnName = string.IsNullOrEmpty(exportAttribute.DisplayName) ? property.Name : exportAttribute.DisplayName;
                            object propertyValue = property.GetValue(sourceObj);
                            if (!string.IsNullOrEmpty(exportAttribute.DisplayFormat))
                            {
                                dataRow[columnName] = string.Format(string.Concat("{0:", exportAttribute.DisplayFormat, "}"), propertyValue);
                            }
                            else
                            {
                                dataRow[columnName] = propertyValue;
                            }
                        }
                    }
                    datasource.Rows.Add(dataRow);
                }
            }
            return datasource;
        }

        public string GenXlsxFile(List<object[]> datasource, string fileName, string sheetName = "Data")
        {
            string filePath = Path.GetFullPath("~/wwwroot/export").Replace("~\\", "");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = string.Concat(filePath, "/", fileName);
            if (filePath.ToLower().IndexOf(".xlsx") < 0)
            {
                filePath = string.Concat(filePath, ".xlsx");
            }
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                workSheet.Cells.LoadFromArrays(datasource);
                package.Save();
            }
            return filePath;
        }

        public MemoryStream GenXlsxStream(List<object[]> datasource, string sheetName = "Data")
        {
            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage(memoryStream))
            {
                
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                workSheet.Cells.LoadFromArrays(datasource);
                package.Save();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        public MemoryStream ExportXlsxStream<T>(List<T> source, string sheetName = "Data")
        {
            return GenXlsxStream(GetExportDataArray(source), sheetName);
        }

        public MemoryStream ExportImportTemplateStream(Type sourceType, string sheetName = "Data")
        {
            return GenXlsxStream(GetImportTemplateDataArray(sourceType), sheetName);
        }

    }
}
