using GemBox.Spreadsheet;
using JoinReward.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace JoinReward.Models.JoinReward
{
    public class ExportExcel
    {
        public byte[] DocumentTemExportExcel(List<ExportExcelCustomer> exportExcelCustomers)
        {
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/ReportTemplate";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "TemplateCustomerRegister.xlsx");

            ExcelFile xlWorkBook = ExcelFile.Load(path);
            ExcelWorksheet v_worksheet = xlWorkBook.Worksheets[0];
            int startRow = 2;
            int startCol = 0;
            int stt = 1;
            for (int i = 0; i < exportExcelCustomers.Count; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    v_worksheet.Cells[startRow + i, startCol + j].Style.WrapText = true;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    switch (j)
                    {
                        case 0:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = stt;
                            break;
                        case 1:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].FullName;
                            break;
                        case 2:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].PhoneNumber;
                            break;
                        case 3:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].IdentificationNumber;
                            break;
                        case 4:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].ProductName;
                            break;
                        case 5:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].Model;
                            break;
                        case 6:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].Province;
                            break;
                        case 7:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].Districts;
                            break;
                        case 8:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].Address;
                            break;
                        case 9:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].PurchaseDate;
                            break;
                        case 10:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].CreatedDate;
                            break;
                        case 11:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].FileIDFront;
                            break;
                        case 12:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].FileIDBackside;
                            break;
                        case 13:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].FileWarranty;
                            break;
                        case 14:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = exportExcelCustomers[i].FileProduct;
                            break;
                    }
                }
                stt += 1;
            }
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileBytes = System.IO.File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return fileBytes;
        }

        public byte[] ExportExcelCustomerWin(List<AssignAscSearchDTO> assignAscSearchDTOs)
        {
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/ReportTemplate";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "TemplateCustomerWin.xlsx");

            ExcelFile xlWorkBook = ExcelFile.Load(path);
            ExcelWorksheet v_worksheet = xlWorkBook.Worksheets[0];
            int startRow = 2;
            int startCol = 0;
            int stt = 1;
            for (int i = 0; i < assignAscSearchDTOs.Count; i++)
            {
                for (int j = 0; j < 29; j++)
                {
                    v_worksheet.Cells[startRow + i, startCol + j].Style.WrapText = true;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    v_worksheet.Cells[startRow + i, startCol + j].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    switch (j)
                    {
                        case 0:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = stt;
                            break;
                        case 1:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].CustomerName;
                            break;
                        case 2:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].LuckyCode;
                            break;
                        case 3:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].PhoneNumber;
                            break;
                        case 4:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].IdentificationNumber;
                            break;
                        case 5:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].PRIZE_WIN_TEXT;
                            break;
                        case 6:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].ModelName;
                            break;
                        case 7:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].EngineNo;
                            break;
                        case 8:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].ProvinceName;
                            break;
                        case 9:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].DistrictName;
                            break;
                        case 10:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].WARD_NAME;
                            break;
                        case 11:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].Address;
                            break;
                        case 12:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].BankNumber;
                            break;
                        case 13:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].BankAccountName;
                            break;
                        case 14:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].BankName;
                            break;
                        case 15:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].BankBranch;
                            break;
                        case 16:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].PROVINCE_BRANCH;
                            break;
                        case 17:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].IDENTIFICATION_AUTHORIZATION;
                            break;
                        case 18:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].COUNT_SUBMIT;
                            break;
                        case 19:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].CustomerStatus;
                            break;
                        case 20:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].PanStatus;
                            break;
                        case 21:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].ASC_STATUS;
                            break;
                        case 22:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].Note;
                            break;
                        case 23:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].NOTE_OF_ASC;
                            break;
                        case 24:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].NOTE_OF_CALL_CENTER;
                            break;
                        case 25:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].NOTE_OF_AGENCY;
                            break;
                        case 26:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].NOTE_CUS_REJECT;
                            break;
                        case 27:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].NOTE_OF_CUSTOMER;
                            break;
                        case 28:
                            v_worksheet.Cells[startRow + i, startCol + j].Value = assignAscSearchDTOs[i].AGENCY_NAME;
                            break;
                    }
                }
                stt += 1;
            }
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileBytes = System.IO.File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return fileBytes;
        }
    }
}
