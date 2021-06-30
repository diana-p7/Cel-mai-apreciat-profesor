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
        public static string ExportFinalResults()
        {
            ProcessInputData.FetchMoodleData();

            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            {
                List<Profesor> profesori = context.Profesors.ToList();

                Dictionary<string, List<int>> remuneratiFaza1 = new Dictionary<string, List<int>>();
                Dictionary<string, List<int>> remuneratiFaza2 = new Dictionary<string, List<int>>();
                Dictionary<string, Dictionary<Profesor, int>> facultateVoturiTitulariLicentaProfesori = new Dictionary<string, Dictionary<Profesor, int>>();
                Dictionary<string, Dictionary<Profesor, float>> facultateVoturiTitulariLicentaMasterProfesori = new Dictionary<string, Dictionary<Profesor, float>>();


                if (!File.Exists(GlobalValues.PATH_SOURCE_FILE))
                {
                    return null;
                }

                string destFileName = "Profesor_Apreciat_Rezultate_Finale.xlsx";
                string destFilePath = Path.Combine(Path.GetDirectoryName(GlobalValues.PATH_SOURCE_FILE), destFileName);

                if (File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }

                File.Copy(GlobalValues.PATH_SOURCE_FILE, destFilePath);

                using (XLWorkbook workbook = new XLWorkbook(destFilePath))
                {
                    var worksheets = workbook.Worksheets.Where(w => w.Visibility.Equals(XLWorksheetVisibility.Visible)).ToList();

                    if (worksheets == null || worksheets.Count <= 0)
                    {
                        return null;
                    }

                    var ws = worksheets[0];
                    ws.Column(5).InsertColumnsAfter(1);
                    ws.Column(6).FirstCell().Value = "VOTURI LICENTA";
                    ws.Column(6).Width = 20;
                    ws.Column(6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Column(6).InsertColumnsAfter(1);
                    ws.Column(7).FirstCell().Value = "VOTURI MASTER";
                    ws.Column(7).Width = 20;
                    ws.Column(7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Column(7).InsertColumnsAfter(1);
                    ws.Column(8).FirstCell().Value = "PROCENTAJ";
                    ws.Column(8).Width = 15;
                    ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Column(8).InsertColumnsAfter(1);
                    ws.Column(9).FirstCell().Value = "REMUNERAT FAZA 1";
                    ws.Column(9).Width = 25;
                    ws.Column(9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws.Column(9).InsertColumnsAfter(1);
                    ws.Column(10).FirstCell().Value = "REMUNERAT FAZA 2";
                    ws.Column(10).Width = 25;
                    ws.Column(10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    int columnCount = ws.ColumnCount();

                    int resultIndex = 0;
                    var rows = ws.Rows().ToList();

                    if (rows.Count == 0)
                    {
                        return null;
                    }

                    int nrVoturiLicenta = 0;
                    int nrVoturiMaster = 0;
                    int nrVotantiProgrameDeStudii = 0;
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

                        for (int columnindex = 11; columnindex < cells.Count; columnindex++)
                        {
                            string denumirePS = ws.Column(columnindex).FirstCell().Value.ToString();
                            var vot = currResult.RezultatVotProfesorProgramStudius.Where(v => v.ProgramStudiu.DenumireScurta == denumirePS).ToList();

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

                        ws.Column(6).Cell(rowIndex + 1).Value = nrVoturiLicenta;
                        ws.Column(6).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        ws.Column(7).Cell(rowIndex + 1).Value = nrVoturiMaster;
                        ws.Column(7).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        totalVoturi = nrVoturiLicenta + nrVoturiMaster;
                        nrVotantiProgrameDeStudii = currResult.RezultatVotProfesorProgramStudius.Select(r => r.ProgramStudiu).Select(ps => ps.NumarVotanti).Sum();
                        float raportNrVoturiNrVotanti = (totalVoturi * 1.0f) / nrVotantiProgrameDeStudii;
                        if (totalVoturi != 0)
                        {
                            ws.Column(8).Cell(rowIndex + 1).Value = raportNrVoturiNrVotanti.ToString();
                        }
                        else
                        {
                            ws.Column(8).Cell(rowIndex + 1).Value = "0";
                        }
                        ws.Column(8).Cell(rowIndex + 1).Style.NumberFormat.Format = "0.000%";
                        ws.Column(8).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        resultIndex++;

                        if (!facultateVoturiTitulariLicentaProfesori.ContainsKey(currResult.FacultateServiciu))
                        {
                            facultateVoturiTitulariLicentaProfesori.Add(currResult.FacultateServiciu, new Dictionary<Profesor, int>());
                        }

                        if (!facultateVoturiTitulariLicentaMasterProfesori.ContainsKey(currResult.FacultateServiciu))
                        {
                            facultateVoturiTitulariLicentaMasterProfesori.Add(currResult.FacultateServiciu, new Dictionary<Profesor, float>());
                        }

                        if (currResult.EligibilRemunerare == true)
                        {
                            facultateVoturiTitulariLicentaProfesori[currResult.FacultateServiciu].Add(currResult, nrVoturiLicenta);
                            facultateVoturiTitulariLicentaMasterProfesori[currResult.FacultateServiciu].Add(currResult, raportNrVoturiNrVotanti);
                        }

                    }

                    foreach (var entry in facultateVoturiTitulariLicentaProfesori)
                    {
                        var eligibiliDict = entry.Value;
                        var eligibiliOrderedList = eligibiliDict.OrderByDescending(el => el.Value).ToList();
                        int nrSelected = ProcessInputData.GetProcentOf(eligibiliOrderedList.Count);

                        remuneratiFaza1.Add(entry.Key, eligibiliOrderedList.Take(nrSelected).Select(e => e.Key.ID_Profesor).ToList());
                    }

                    foreach (var entry in facultateVoturiTitulariLicentaMasterProfesori)
                    {
                        var eligibiliDict = entry.Value;
                        var eligibiliOrderedList = eligibiliDict.OrderByDescending(el => el.Value).ToList();
                        remuneratiFaza2.Add(entry.Key, new List<int>());

                        int nrSelected = ProcessInputData.GetProcentOf(eligibiliOrderedList.Count);

                        foreach (var it in eligibiliOrderedList)
                        {
                            if (remuneratiFaza2[it.Key.FacultateServiciu].Count == nrSelected)
                            {
                                break;
                            }

                            if (!remuneratiFaza1[it.Key.FacultateServiciu].Contains(it.Key.ID_Profesor))
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
                            ws.Column(9).Cell(rowNr).Value = "DA";
                            ws.Column(9).Cell(rowNr).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        if (remuneratiFaza2[profesori[resultIndex].FacultateServiciu].Contains(profesori[resultIndex].ID_Profesor))
                        {
                            ws.Column(10).Cell(rowNr).Value = "DA";
                            ws.Column(10).Cell(rowNr).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        resultIndex++;
                    }

                    // adauga numar votanti si numar absolventi si data inchiderii votului
                    ws.Row(4).InsertRowsBelow(1);
                    ws.Row(5).Cell(2).Value = "Votarea Terminata";
                    ws.Row(5).Cell(2).Style = ws.Row(4).Cell(2).Style;
                    var range = ws.Range(ws.Row(5).Cell(2), ws.Row(5).Cell(5)).Merge();
                    range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    int cellsCount = rows[0].Cells().Count();
                    for (int columnindex = 11; columnindex < cellsCount; columnindex++)
                    {
                        string denumirePS = ws.Column(columnindex).FirstCell().GetValue<String>();

                        if (String.IsNullOrEmpty(denumirePS))
                        {
                            break;
                        }

                        var programList = context.ProgramStudius.Where(p => p.DenumireScurta.Equals(denumirePS)).ToList();

                        if (programList == null || programList.Count == 0)
                        {
                            continue;
                        }

                        var ps = programList[0];

                        ws.Column(columnindex).Cell(3).Value = ps.NumarAbsolventi;
                        ws.Column(columnindex).Cell(4).Value = ps.NumarVotanti;

                        if (ps.DataInchidereVot.HasValue)
                        {
                            if (ps.DataInchidereVot.Value <= DateTime.Now && ps.DataInchidereVot.Value.Year == DateTime.Now.Year)
                            {
                                ws.Row(5).Cell(columnindex).Value = "Y";
                            }
                            else if (ps.DataInchidereVot.Value <= DateTime.Now && ps.DataInchidereVot.Value.Year != DateTime.Now.Year)
                            {
                                ws.Row(5).Cell(columnindex).Value = "X";
                            }
                            else if (ps.DataInchidereVot.Value > DateTime.Now)
                            {
                                ws.Row(5).Cell(columnindex).Value = "N";
                            }
                        }
                        else
                        {
                            ws.Row(5).Cell(columnindex).Value = "N";
                        }
                    }

                    workbook.Save();
                }
                return destFilePath;
            }
        }
        static public string ExportVotingStatus()
        {
            ProcessInputData.FetchMoodleData();

            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            {
                List<Profesor> profesori = context.Profesors.ToList();

                if (!File.Exists(GlobalValues.PATH_SOURCE_FILE))
                {
                    return null;
                }

                string destFileName = "Profesor_Apreciat_Status_Voturi_Elearning.xlsx";
                string destFilePath = Path.Combine(Path.GetDirectoryName(GlobalValues.PATH_SOURCE_FILE), destFileName);

                if (File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }

                File.Copy(GlobalValues.PATH_SOURCE_FILE, destFilePath);

                List<ProgramStudiu> programStudius = context.ProgramStudius.ToList();

                using (XLWorkbook workbook = new XLWorkbook(destFilePath))
                {
                    List<IXLWorksheet> worksheets = workbook.Worksheets.Where(w => w.Visibility.Equals(XLWorksheetVisibility.Visible)).ToList();

                    if (worksheets == null || worksheets.Count <= 0)
                    {
                        return null;
                    }

                    IXLWorksheet ws = worksheets[0];
                    var rows = ws.Rows().ToList();
                    int indexProfesor = 0;

                    for (int rowIndex = 4; rowIndex < rows.Count; rowIndex++)
                    {
                        if (indexProfesor >= profesori.Count)
                        {
                            break;
                        }

                        var row = rows[rowIndex];
                        var cells = row.Cells().ToList();
                        var profesor = profesori[indexProfesor];

                        for (int columnindex = 6; columnindex < cells.Count; columnindex++)
                        {
                            var vot = profesor.RezultatVotProfesorProgramStudius.Where(v => v.ProgramStudiu.DenumireScurta == ws.Column(columnindex).FirstCell().GetValue<String>()).ToList();

                            if (vot.Count == 0)
                            {
                                continue;
                            }

                            ws.Column(columnindex).Cell(rowIndex + 1).Value = vot.First().NumarVoturi;
                            ws.Column(columnindex).Cell(rowIndex + 1).Style.Font.Bold = true;
                            ws.Column(columnindex).Cell(rowIndex + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        indexProfesor++;
                    }

                    // adauga numar votanti si numar absolventi si data inchiderii votului
                    ws.Row(4).InsertRowsBelow(1);
                    ws.Row(5).Cell(2).Value = "Votarea Terminata";
                    ws.Row(5).Cell(2).Style = ws.Row(4).Cell(2).Style;
                    var range = ws.Range(ws.Row(5).Cell(2), ws.Row(5).Cell(5)).Merge();
                    range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    int cellsCount = rows[0].Cells().Count();
                    for (int columnindex = 6; columnindex < cellsCount; columnindex++)
                    {
                        if (String.IsNullOrEmpty(ws.Column(columnindex).FirstCell().GetValue<String>()))
                        {
                            break;
                        }

                        var programList = programStudius.Where(p => p.DenumireScurta.Equals(ws.Column(columnindex).FirstCell().GetValue<String>())).ToList();
                        if (programList == null || programList.Count == 0)
                        {
                            continue;
                        }
                        var ps = programList[0];

                        ws.Column(columnindex).Cell(3).Value = ps.NumarAbsolventi;
                        ws.Column(columnindex).Cell(4).Value = ps.NumarVotanti;

                        if (ps.DataInchidereVot.HasValue)
                        {
                            if (ps.DataInchidereVot.Value <= DateTime.Now && ps.DataInchidereVot.Value.Year == DateTime.Now.Year)
                            {
                                ws.Row(5).Cell(columnindex).Value = "Y";
                            }
                            else if (ps.DataInchidereVot.Value <= DateTime.Now && ps.DataInchidereVot.Value.Year != DateTime.Now.Year)
                            {
                                ws.Row(5).Cell(columnindex).Value = "X";
                            }
                            else if (ps.DataInchidereVot.Value > DateTime.Now)
                            {
                                ws.Row(5).Cell(columnindex).Value = "N";
                            }
                        }
                        else
                        {
                            ws.Row(5).Cell(columnindex).Value = "N";
                        }
                    }
                    workbook.Save();
                }
                return destFilePath;
            }
        }
    }
}