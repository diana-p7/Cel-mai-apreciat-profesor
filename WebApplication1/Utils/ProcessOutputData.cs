using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ProfApreciat.Models;
using SpreadsheetLight;

namespace ProfApreciat.Utils
{
    public class ProcessOutputData
    {
        
        public static void WriteOutputFile(List<Profesor> profesori)
        {
            Dictionary<string, List<int>> remuneratiFaza1 = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> remuneratiFaza2 = new Dictionary<string, List<int>>();
            Dictionary<string, Dictionary<Profesor, int>> facultateVoturiTitulariLicentaProfesori = new Dictionary<string, Dictionary<Profesor, int>>();
            Dictionary<string, Dictionary<Profesor, int>> facultateVoturiTitulariLicentaMasterProfesori = new Dictionary<string, Dictionary<Profesor, int>>();

            string destFilePath = @"C:\Excel\0X_Cel_mai_apreciat_profesor_2021_univ_de_lucru_voturi_licenta.xlsx";

            if (!File.Exists(GlobalValues.Path_FILE_INPUT))
            {
                return;
            }

            if (File.Exists(destFilePath))
            {
                File.Delete(destFilePath);
            }

            File.Copy(GlobalValues.Path_FILE_INPUT, destFilePath);

            using (XLWorkbook workbook = new XLWorkbook(destFilePath))
            {
                var worksheets = workbook.Worksheets.Where(s => s.Name == "nr votanti").ToList();

                if (worksheets == null || worksheets.Count <= 0)
                {
                    return;
                }

                var ws = worksheets[0];
                ws.Column(6).InsertColumnsAfter(1);
                ws.Column(7).FirstCell().Value = "VOTURI LICENTA";
                ws.Column(7).Width = 15;
                ws.Column(7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(7).InsertColumnsAfter(1);
                ws.Column(8).FirstCell().Value = "VOTURI MASTER";
                ws.Column(8).Width = 15;
                ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ws.Column(8).InsertColumnsAfter(1);
                ws.Column(9).FirstCell().Value = "PROCENTAJ";
                ws.Column(9).Width = 10;
                ws.Column(9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ws.Column(9).InsertColumnsAfter(1);
                ws.Column(10).FirstCell().Value = "REMUNERAT FAZA 1";
                ws.Column(10).Width = 25;
                ws.Column(10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                ws.Column(10).InsertColumnsAfter(1);
                ws.Column(11).FirstCell().Value = "REMUNERAT FAZA 2";
                ws.Column(11).Width = 25;
                ws.Column(11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                int columnCount = ws.ColumnCount();    

                int resultIndex = 0;
                var rows = ws.Rows().ToList();

                if (rows.Count == 0)
                {
                    return;
                }

                int nrVoturiLicenta = 0;
                int nrVoturiMaster = 0;
                int nrAbsolventiProgrameDeStudiu = 0;
                int totalVoturi = 0;

                for (int rowIndex = 4; rowIndex < rows.Count; rowIndex++)
                {
                    if (resultIndex >= profesori.Count)
                    {
                        break;
                    }

                    nrVoturiLicenta = 0;
                    nrVoturiMaster = 0;
                    var row = rows[rowIndex];
                    var cells = row.Cells().ToList();
                    var currResult = profesori[resultIndex];

                    cells[0].Value = currResult.ID_Profesor;

                    cells[1].Value = currResult.Nume; 

                    cells[2].Value = currResult.Prenume;

                    cells[3].Value = currResult.Email;

                    cells[4].Value = currResult.GradDidactic;

                    for (int columnindex = 10; columnindex < cells.Count; columnindex++)
                    {
                        var vot = currResult.RezultatVotProfesorProgramStudius.Where(v => v.ProgramStudiu.DenumireScurta == ws.Column(columnindex).FirstCell().Value.ToString()).ToList();

                        if (vot.Count == 0)
                        {
                            continue;
                        }

                        ws.Column(columnindex).Cell(rowIndex + 1).Value = vot.First().NumarVoturi;
                        ws.Column(columnindex).Cell(rowIndex + 1).Style.Font.Bold = true;
                        ws.Column(columnindex).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        if (vot.First().ProgramStudiu.ID_TipCiclu == 1)
                        {
                            nrVoturiLicenta += vot.First().NumarVoturi;
                        }
                        else
                        {
                            nrVoturiMaster += vot.First().NumarVoturi;
                        }

                    }

                    ws.Column(7).Cell(rowIndex + 1).Value = nrVoturiLicenta;
                    ws.Column(7).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Column(8).Cell(rowIndex + 1).Value = nrVoturiMaster;
                    ws.Column(8).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    totalVoturi = nrVoturiLicenta + nrVoturiMaster;
                    nrAbsolventiProgrameDeStudiu = currResult.RezultatVotProfesorProgramStudius.Select(r => r.ProgramStudiu).Select(ps => ps.NumarAbsolventi).Sum();
                    float procent = (totalVoturi * 1.0f) / nrAbsolventiProgrameDeStudiu;

                    ws.Column(9).Cell(rowIndex + 1).Value = procent.ToString();
                    ws.Column(9).Cell(rowIndex + 1).Style.NumberFormat.Format = "0.000%";
                    ws.Column(9).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    resultIndex++;

                    if (!facultateVoturiTitulariLicentaProfesori.ContainsKey(currResult.FacultateServiciu))
                    {
                        facultateVoturiTitulariLicentaProfesori.Add(currResult.FacultateServiciu, new Dictionary<Profesor, int>());
                    }

                    if (!facultateVoturiTitulariLicentaMasterProfesori.ContainsKey(currResult.FacultateServiciu))
                    {
                        facultateVoturiTitulariLicentaMasterProfesori.Add(currResult.FacultateServiciu, new Dictionary<Profesor, int>());
                    }

                    if(currResult.EligibilRemunerare == true)
                    {
                        facultateVoturiTitulariLicentaProfesori[currResult.FacultateServiciu].Add(currResult, nrVoturiLicenta);
                        facultateVoturiTitulariLicentaMasterProfesori[currResult.FacultateServiciu].Add(currResult, totalVoturi);
                    }
                                  
                }

                
                foreach (var entry in facultateVoturiTitulariLicentaProfesori)
                {
                    var eligibiliDict = entry.Value;
                    var eligibiliOrderedList = eligibiliDict.OrderByDescending(el => el.Value).ToList();
                    double percentof = eligibiliOrderedList.Count * GlobalValues.PROCENT_ELIGIBILI / 100.0;
                    int nrSelected = 0;
                    if (percentof < 0.5 || percentof + 0.5 >= Math.Ceiling(percentof))
                    {
                        nrSelected = (int)Math.Ceiling(percentof);
                    }
                    else
                    {
                        nrSelected = (int)Math.Floor(percentof);
                    }
                    remuneratiFaza1.Add(entry.Key, eligibiliOrderedList.Take(nrSelected).Select(e => e.Key.ID_Profesor).ToList());
                }

                foreach (var entry in facultateVoturiTitulariLicentaMasterProfesori)
                {
                    var eligibiliDict = entry.Value;
                    var eligibiliOrderedList = eligibiliDict.OrderByDescending(el => el.Value).ToList();
                    remuneratiFaza2.Add(entry.Key, new List<int>());

                    double percentof = eligibiliOrderedList.Count * GlobalValues.PROCENT_ELIGIBILI / 100.0;
                    int nrSelected = 0;
                    if (percentof + 0.5 >= Math.Ceiling(percentof))
                    {
                        nrSelected = (int)Math.Ceiling(percentof);
                    }
                    else
                    {
                        nrSelected = (int)Math.Floor(percentof);
                    }

                    foreach (var it in eligibiliOrderedList)
                    {
                        if (remuneratiFaza2[it.Key.FacultateServiciu].Count == nrSelected)
                        {
                            break;
                        }

                        if(!remuneratiFaza1[it.Key.FacultateServiciu].Contains(it.Key.ID_Profesor))
                        {
                            remuneratiFaza2[it.Key.FacultateServiciu].Add(it.Key.ID_Profesor);
                        }
                    }
                }

                resultIndex = 0;
                for (int rowNr = 5; rowNr <= rows.Count; rowNr++)
                {
                    if (resultIndex >= profesori.Count)
                    {
                        break;
                    }

                    if (remuneratiFaza1[profesori[resultIndex].FacultateServiciu].Contains(profesori[resultIndex].ID_Profesor))
                    {
                        ws.Column(10).Cell(rowNr).Value = "DA";
                        ws.Column(10).Cell(rowNr).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    }
                    
                    if (remuneratiFaza2[profesori[resultIndex].FacultateServiciu].Contains(profesori[resultIndex].ID_Profesor))
                    {
                        ws.Column(11).Cell(rowNr).Value = "DA";
                        ws.Column(11).Cell(rowNr).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    }

                    resultIndex++;
                }

                workbook.Save();
            }
        }
    }
}