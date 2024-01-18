using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class ExportBL : IExportBL
    {
        public bool ExportToExcel(List<UserViewModel> selectedList, string trainingName)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("SelectedEmployees");

                    worksheet.Cells[2, 2].Value = "Training:";
                    worksheet.Cells[2, 2].Style.Font.Bold = true;
                    worksheet.Cells[2, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    worksheet.Cells[2, 3].Value = trainingName;

                    using (var range = worksheet.Cells[2, 2, 2, 3])
                    {
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    var headers = new string[] { "First Name", "Last Name", "Email", "Mobile Number", "Manager First Name", "Manager Last Name" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[4, i + 2].Value = headers[i];

                        using (var range = worksheet.Cells[4, i + 2])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            range.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    for (int i = 0; i < selectedList.Count; i++)
                    {
                        var user = selectedList[i];
                        worksheet.Cells[i + 5, 2].Value = user.FirstName;
                        worksheet.Cells[i + 5, 3].Value = user.LastName;
                        worksheet.Cells[i + 5, 4].Value = user.Email;
                        worksheet.Cells[i + 5, 5].Value = user.MobileNum;
                        worksheet.Cells[i + 5, 6].Value = user.ManagerFirstName;
                        worksheet.Cells[i + 5, 7].Value = user.ManagerLastName;

                        using (var range = worksheet.Cells[i + 5, 2, i + 5, 7])
                        {
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }

                    worksheet.Cells.AutoFitColumns();

                    FileInfo excelFile = new FileInfo(GetSavePath(trainingName));
                    package.SaveAs(excelFile);
                    return true;
                }
            }

            catch (Exception)
            {
                return false;
                throw;
            }
        }

        private string GetSavePath(string trainingName)
        {
            string downloadsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string targetFolderPath = Path.Combine(downloadsFolderPath, "Training Selections");

            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            string saveFilePath = Path.Combine(targetFolderPath, $"{trainingName}.xlsx");
            return saveFilePath;
        }
    }
}