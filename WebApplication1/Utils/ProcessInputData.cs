using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using ProfApreciat.Models;
using SpreadsheetLight;
using System.Drawing;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Globalization;

namespace ProfApreciat
{
    public class ProcessInputData
    {
        public static int GetProcentOf(int number)
        {
            int result = 0;
            double percentof = number * GlobalValues.PROCENT_REMUNERATI / 100.0;

            if (percentof < 0.5 || percentof + 0.5 >= Math.Ceiling(percentof))
            {
                result = (int)Math.Ceiling(percentof);
            }
            else
            {
                result = (int)Math.Floor(percentof);
            }

            return result;
        }

        public static JObject GetProfesorsForPS(string ps)
        {
            JObject jsonResponse = new JObject();

            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            {
                var rvp = context.RezultatVotProfesorProgramStudius.Where(rv => rv.ProgramStudiu.DenumireScurta == ps.ToUpper());

                if (!rvp.Any())
                {
                    jsonResponse.Add("error", "Programul de studii nu a fost gasit.");
                    return jsonResponse;
                }

                List<Profesor> prfs = rvp.Select(rv => rv.Profesor).ToList();

                int nrProfesoriDeVotat = GetProcentOf(prfs.Count);

                jsonResponse.Add("max", nrProfesoriDeVotat);

                List<string> optiuni = new List<string>();

                foreach(Profesor profesor in prfs)
                {
                    optiuni.Add("(Nr.crt_" + profesor.ID_Profesor + ") " + profesor.Nume + " " + profesor.Prenume); 
                }

                jsonResponse.Add("optiuni", new JArray(optiuni));
            }

            return jsonResponse;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string FetchMoodleData()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("moodlews_baseURL"));

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                string moodlewstoken = ConfigurationManager.AppSettings.Get("moodlews_token");
                string moodlewsFunctionName = ConfigurationManager.AppSettings.Get("moodlews_function_get_questionnaire_responses");
                string substrcmidnumber = ConfigurationManager.AppSettings.Get("moodlews_questionnaire_pa_substrcmidnumber");
                string queryParams = "?wstoken=" + moodlewstoken + "&wsfunction=" + moodlewsFunctionName + "&moodlewsrestformat=json" + "&substrcmidnumber=" + substrcmidnumber;

                HttpResponseMessage httpResponseMessage = client.GetAsync(queryParams).Result;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var jsonString = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(jsonString);

                    if (json["reports"] == null)
                    {
                        return String.Empty;
                    }

                    using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
                    {

                        foreach (var report in json["reports"])
                        {
                            string cmidnumber = report["cmidnumber"].ToString();
                            string denumireScurtaPS = cmidnumber.Substring(cmidnumber.IndexOf(substrcmidnumber) + substrcmidnumber.Length);
                            var listPS = context.ProgramStudius.Where(ps => ps.DenumireScurta.ToUpper().Equals(denumireScurtaPS.ToUpper())).ToList();

                            if (listPS.Count == 0)
                            {
                                // issue warning?
                                continue;
                            }

                            ProgramStudiu programStudii = listPS[0];

                            if (report["closedate"] != null && !String.IsNullOrEmpty(report["closedate"].ToString()))
                            {
                                String dateStr = report["closedate"].ToString().Replace('-', '/');
                                programStudii.DataInchidereVot = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            }

                            int totalVotes = 0;

                            foreach (var option in report["options"])
                            {
                                string optionText = option["optiontext"].ToString();
                                if (optionText.IndexOf("(") == -1)
                                {
                                    continue;
                                }
                                optionText = optionText.Substring(optionText.IndexOf("(") + 1);
                                if (optionText.IndexOf("_") == -1)
                                {
                                    continue;
                                }
                                optionText = optionText.Substring(optionText.IndexOf("_") + 1);
                                if (optionText.IndexOf(")") == -1)
                                {
                                    continue;
                                }
                                string nrcrtProfesorStr = optionText.Substring(0, optionText.IndexOf(")"));
                                int nrcrtProfesor = 0;

                                if (!int.TryParse(nrcrtProfesorStr, out nrcrtProfesor))
                                {
                                    // issue warning?
                                    continue;
                                }

                                var listProf = context.Profesors.Where(prf => prf.ID_Profesor.Equals(nrcrtProfesor)).ToList();

                                if (listProf.Count == 0)
                                {
                                    // issue warning?
                                    continue;
                                }

                                Profesor profesor = listProf[0];
                                var listRez = programStudii.RezultatVotProfesorProgramStudius.Where(res => res.ID_Profesor.Equals(profesor.ID_Profesor)).ToList();

                                if (listRez.Count == 0)
                                {
                                    // issue warning?
                                    continue;
                                }

                                var rez = listRez[0];

                                if (option["nbofvotes"] != null && !String.IsNullOrEmpty(option["nbofvotes"].ToString()))
                                {
                                    rez.NumarVoturi = short.Parse(option["nbofvotes"].ToString());
                                    totalVotes += rez.NumarVoturi;
                                }
                            }

                            programStudii.NumarVotanti = totalVotes / GetProcentOf(report["options"].Count());

                            if (report["nbofstudents"] != null)
                            {
                                programStudii.NumarAbsolventi = int.Parse(report["nbofstudents"].ToString());
                            }
                        }
                        context.SaveChanges();
                    }
                    return jsonString;
                }
                else
                {
                    return "Error status code : " + httpResponseMessage.StatusCode.ToString();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static DataSet makeDataSet()
        {
            DataSet dataSet = new DataSet();

            DataColumn column;
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            DataRow row;
            DataRelation relation;

            //DataTable tableFacultate = new DataTable("Facultate");

            //column = new DataColumn();
            //column.DataType = Type.GetType("System.Int32");
            //column.ColumnName = "ID_Facultate";
            //column.ReadOnly = true;
            //column.Unique = true;
            //column.AutoIncrement = true;
            //column.AutoIncrementSeed = 1;
            //column.AutoIncrementStep = 1;
            //tableFacultate.Columns.Add(column);

            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "DenumireScurta";
            //column.Unique = true;
            //column.AllowDBNull = false;
            //tableFacultate.Columns.Add(column);

            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "Denumire";
            //tableFacultate.Columns.Add(column);

            //PrimaryKeyColumns[0] = tableFacultate.Columns["ID_Facultate"];
            //tableFacultate.PrimaryKey = PrimaryKeyColumns;

            //dataSet.Tables.Add(tableFacultate);

            DataTable tableTipCicluInvatamant = new DataTable("TipCicluInvatamant");

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_TipCiclu";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            tableTipCicluInvatamant.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Denumire";
            column.Unique = true;
            column.AllowDBNull = false;
            tableTipCicluInvatamant.Columns.Add(column);

            PrimaryKeyColumns[0] = tableTipCicluInvatamant.Columns["ID_TipCiclu"];
            tableTipCicluInvatamant.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableTipCicluInvatamant);

            DataTable tableProgramStudiu = new DataTable("ProgramStudiu");

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_ProgramStudiu";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Facultate";
            column.AllowDBNull = true;
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_TipCiclu";
            column.AllowDBNull = false;
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DenumireScurta";
            column.Unique = true;
            column.AllowDBNull = false;
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Denumire";
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "NumarAbsolventi";
            column.AllowDBNull = false;
            tableProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "NumarVotanti";
            column.AllowDBNull = false;
            tableProgramStudiu.Columns.Add(column);

            PrimaryKeyColumns[0] = tableProgramStudiu.Columns["ID_ProgramStudiu"];
            tableProgramStudiu.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableProgramStudiu);

            //relation = new DataRelation("ProgramStudiu_Facultate", tableFacultate.Columns["ID_Facultate"], tableProgramStudiu.Columns["ID_Facultate"]);
            //tableProgramStudiu.ParentRelations.Add(relation);

            relation = new DataRelation("ProgramStudiu_TipCicluInvatamant", tableTipCicluInvatamant.Columns["ID_TipCiclu"], tableProgramStudiu.Columns["ID_TipCiclu"]);
            tableProgramStudiu.ParentRelations.Add(relation);

            DataTable tableProfesor = new DataTable("Profesor");

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_Profesor";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "FacultateServiciu";
            column.AllowDBNull = true;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Email";
            column.AllowDBNull = true;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nume";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Prenume";
            column.AllowDBNull = true;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "GradDidactic";
            column.AllowDBNull = true;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Boolean");
            column.ColumnName = "EligibilRemunerare";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            PrimaryKeyColumns[0] = tableProfesor.Columns["ID_Profesor"];
            tableProfesor.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableProfesor);

            //relation = new DataRelation("Profesor_Facultate", tableFacultate.Columns["ID_Facultate"], tableProfesor.Columns["ID_FacultateServiciu"]);
            //tableProfesor.ParentRelations.Add(relation);

            DataTable tableRezultatVotProfesorProgramStudiu = new DataTable("RezultatVotProfesorProgramStudiu");

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_RezultatVot";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            tableRezultatVotProfesorProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_ProgramStudiu";
            column.AllowDBNull = false;
            tableRezultatVotProfesorProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_Profesor";
            column.AllowDBNull = false;
            tableRezultatVotProfesorProgramStudiu.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int16");
            column.ColumnName = "NumarVoturi";
            column.AllowDBNull = false;
            tableRezultatVotProfesorProgramStudiu.Columns.Add(column);

            PrimaryKeyColumns[0] = tableRezultatVotProfesorProgramStudiu.Columns["ID_RezultatVot"];
            tableRezultatVotProfesorProgramStudiu.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableRezultatVotProfesorProgramStudiu);

            relation = new DataRelation("EvaluareProfesorApreciat_Profesor", tableProfesor.Columns["ID_Profesor"], tableRezultatVotProfesorProgramStudiu.Columns["ID_Profesor"]);
            tableRezultatVotProfesorProgramStudiu.ParentRelations.Add(relation);

            relation = new DataRelation("EvaluareProfesorApreciat_ProgramStudiu", tableProgramStudiu.Columns["ID_ProgramStudiu"], tableRezultatVotProfesorProgramStudiu.Columns["ID_ProgramStudiu"]);
            tableRezultatVotProfesorProgramStudiu.ParentRelations.Add(relation);

            // insert in TipCicluInvatamant licenta si master

            foreach(string denumireTipCicluInvatamant in GlobalValues.Map_TIP_CICLU_INVATAMANT_ID.Keys)
            {
                row = dataSet.Tables["TipCicluInvatamant"].NewRow();
                row["Denumire"] = denumireTipCicluInvatamant;
                dataSet.Tables["TipCicluInvatamant"].Rows.Add(row);
            }

            return dataSet;
        }

        public static string InsertData()
        {
            ObjectParameter responseMg = new ObjectParameter("responseMessage", Type.GetType("System.String"));
            ObjectParameter insertedId = new ObjectParameter("insertedID", Type.GetType("System.Int32"));
            string msg = String.Empty;
            DataSet dataSet = makeDataSet();

            ReadData(dataSet);

            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            {
                context.spClearDatabase();

                foreach (DataRow tipCiclu in dataSet.Tables["TipCicluInvatamant"].Rows)
                {
                    context.spAdaugaTipCicluInvatamant((string)tipCiclu["Denumire"], responseMg, insertedId);
                }

                foreach (DataRow ps in dataSet.Tables["ProgramStudiu"].Rows)
                {
                    context.spAdaugaProgramStudiu(
                        (string)ps["Facultate"],
                        (int)ps["ID_TipCiclu"],
                        (string)ps["DenumireScurta"],
                        null,
                        (int)ps["NumarAbsolventi"],
                        (int)ps["NumarVotanti"],
                        responseMg,
                        insertedId
                        );
                }

                foreach (DataRow prof in dataSet.Tables["Profesor"].Rows)
                {
                    context.spAdaugaProfesor(
                        (string)prof["Nume"],
                        (string)prof["Prenume"],
                        (string)prof["Email"],
                        (string)prof["GradDidactic"],
                        (string)prof["FacultateServiciu"],
                        (bool)prof["EligibilRemunerare"],
                        responseMg,
                        insertedId
                        );
                }

                foreach (DataRow rv in dataSet.Tables["RezultatVotProfesorProgramStudiu"].Rows)
                {
                    context.spAdaugaRezultatVotProfesorProgramStudiu(
                        (int)rv["ID_ProgramStudiu"],
                        (int)rv["ID_Profesor"],
                        (short)rv["NumarVoturi"],
                        responseMg,
                        insertedId
                        );
                }
            }

            dataSet.Dispose();
            return msg;
        }


        public static DataSet ReadData(DataSet dataSet)
        {
            Dictionary<int, int> votanti = new Dictionary<int, int>();
            Dictionary<string, int> denumireFacultateCuId = new Dictionary<string, int>();
            Dictionary<string, int> denumirePSCuId = new Dictionary<string, int>();
            DataRow row;
            bool auFormatCorectProgrameleDeStudii = true;

            using (SLDocument sl = new SLDocument(GlobalValues.PATH_SOURCE_FILE))
            {
                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                int iStartColumnIndex = stats.StartColumnIndex, psStartColumnIndex = 0;
                String denumireFacultate = String.Empty, denumirePS = String.Empty;
                int indexColumnEmail = 0, indexColumnNume = 0, indexColumnPrenume = 0;
                int indexColumnGradDidactic = 0, indexColumnFacultatea = 0;

                for (int columnIndex = stats.StartColumnIndex; columnIndex <= 6; columnIndex++)
                {
                    string columnHeader = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);

                    if (String.IsNullOrEmpty(columnHeader))
                    {
                        break;
                    }

                    if (columnHeader.ToUpper().Equals(GlobalValues.ColumnHeader_NUME))
                    {
                        indexColumnNume = columnIndex;
                        continue;
                    }

                    if (columnHeader.ToUpper().Equals(GlobalValues.ColumnHeader_PRENUME))
                    {
                        indexColumnPrenume = columnIndex;
                        continue;
                    }

                    if (columnHeader.ToUpper().Equals(GlobalValues.ColumnHeader_EMAIL))
                    {
                        indexColumnEmail = columnIndex;
                        continue;
                    }

                    if (columnHeader.ToUpper().Equals(GlobalValues.ColumnHeader_GRAD_DIDACTIC))
                    {
                        indexColumnGradDidactic = columnIndex;
                        continue;
                    }

                    if (columnHeader.ToUpper().Equals(GlobalValues.ColumnHeader_FACULTATEA))
                    {
                        indexColumnFacultatea = columnIndex;
                        psStartColumnIndex = indexColumnFacultatea + 1;
                        continue;
                    }
                }

                for (int columnIndex = psStartColumnIndex; columnIndex <= stats.EndColumnIndex; columnIndex++)
                {
                    denumirePS = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);
                    denumireFacultate = sl.GetCellValueAsString(stats.StartRowIndex + 1, columnIndex);

                    if (String.IsNullOrEmpty(denumirePS) && String.IsNullOrEmpty(denumireFacultate))
                    {
                        break;
                    }

                    if (!String.IsNullOrEmpty(denumirePS) && !denumirePS.ToUpper().StartsWith("L") && !denumirePS.ToUpper().StartsWith("M"))
                    {
                        auFormatCorectProgrameleDeStudii = false;
                        break;
                    }
                }


                for (int columnIndex = psStartColumnIndex; columnIndex <= stats.EndColumnIndex; columnIndex++)
                {
                    denumirePS = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);
                    denumireFacultate = sl.GetCellValueAsString(stats.StartRowIndex + 1, columnIndex);

                    if (String.IsNullOrEmpty(denumirePS) && String.IsNullOrEmpty(denumireFacultate))
                    {
                        break;
                    }

                    if (String.IsNullOrEmpty(denumirePS))
                    {
                        continue;
                        // TO DO : ISSUE WARNING?
                    }

                    // adauga programul de studiu

                    row = dataSet.Tables["ProgramStudiu"].NewRow();
                    row["DenumireScurta"] = denumirePS;
                    row["Facultate"] = denumireFacultate;
                    row["NumarAbsolventi"] = 0;
                    row["NumarVotanti"] = 0;

                    if ((auFormatCorectProgrameleDeStudii && denumirePS.ToUpper().StartsWith("M")) || sl.GetCellStyle(stats.StartRowIndex, columnIndex).Fill.PatternForegroundColor.R == 195) //PS : Master
                    {
                        row["ID_TipCiclu"] = GlobalValues.Map_TIP_CICLU_INVATAMANT_ID[GlobalValues.Denumire_TIP_CICLU_INVATAMANT_MASTER];
                    }
                    else   //PS : LICENTA
                    {
                        row["ID_TipCiclu"] = GlobalValues.Map_TIP_CICLU_INVATAMANT_ID[GlobalValues.Denumire_TIP_CICLU_INVATAMANT_LICENTA];
                    }

                    dataSet.Tables["ProgramStudiu"].Rows.Add(row);
                    denumirePSCuId.Add(denumirePS, (int)row["ID_ProgramStudiu"]);
                }

                string nume = String.Empty, prenume = String.Empty, email = String.Empty, gradDidactic = String.Empty, facultateServiciu = String.Empty;
                int idProfesor;

                for (int rowIndex = stats.StartRowIndex + 4; rowIndex <= stats.EndRowIndex; ++rowIndex)
                {
                    nume = sl.GetCellValueAsString(rowIndex, indexColumnNume);
                    prenume = sl.GetCellValueAsString(rowIndex, indexColumnPrenume);

                    if (String.IsNullOrEmpty(nume) && String.IsNullOrEmpty(prenume))
                    {
                        break;
                    }

                    row = dataSet.Tables["Profesor"].NewRow();
                    row["Nume"] = nume;
                    row["Prenume"] = prenume;

                    if (indexColumnEmail != 0)
                    {
                        email = sl.GetCellValueAsString(rowIndex, indexColumnEmail);
                        row["Email"] = email;
                    }
                    else
                    {
                        row["Email"] = String.Empty;
                    }

                    gradDidactic = sl.GetCellValueAsString(rowIndex, indexColumnGradDidactic);
                    facultateServiciu = sl.GetCellValueAsString(rowIndex, indexColumnFacultatea);

                    row["GradDidactic"] = gradDidactic;
                    row["FacultateServiciu"] = facultateServiciu;

                    if (gradDidactic.ToUpper().Equals(GlobalValues.DenumireScurta_CADRU_EXTERN))
                    {
                        row["EligibilRemunerare"] = false;
                    }
                    else
                    {
                        row["EligibilRemunerare"] = true;
                    }

                    dataSet.Tables["Profesor"].Rows.Add(row);

                    //ADD REZULTATE VOT

                    for (int columnIndex = psStartColumnIndex; columnIndex <= stats.EndColumnIndex; columnIndex++)
                    {
                        denumirePS = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);
                        denumireFacultate = sl.GetCellValueAsString(stats.StartRowIndex + 1, columnIndex);

                        if (String.IsNullOrEmpty(denumirePS) && String.IsNullOrEmpty(denumireFacultate))
                        {
                            break;
                        }

                        if (String.IsNullOrEmpty(denumirePS))
                        {
                            continue;
                        }

                        if (!sl.GetCellValueAsString(rowIndex, columnIndex).ToUpper().Equals("DA"))
                        {
                            continue;
                        }

                        idProfesor = (int)row["ID_Profesor"];
                        row = dataSet.Tables["RezultatVotProfesorProgramStudiu"].NewRow();
                        row["ID_Profesor"] = idProfesor;
                        row["ID_ProgramStudiu"] = denumirePSCuId[denumirePS];
                        row["NumarVoturi"] = 0;                  
                        dataSet.Tables["RezultatVotProfesorProgramStudiu"].Rows.Add(row);
                    }
                }

            }

            return dataSet;
        }

        

    }
}