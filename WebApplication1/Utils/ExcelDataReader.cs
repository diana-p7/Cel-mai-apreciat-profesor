using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using ProfApreciat.Models;
using SpreadsheetLight;
using System.Drawing;
using System.Diagnostics;

namespace ProfApreciat
{
    public class ExcelDataReader
    {
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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ReadData(dataSet);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            msg += "ReadData : " + elapsedTime;

            stopWatch = new Stopwatch();
            stopWatch.Start();
            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            {
                context.spClearDatabase();
                
                foreach (DataRow tipCiclu in dataSet.Tables["TipCicluInvatamant"].Rows)
                {
                    //context.TipCicluInvatamants.Add(new TipCicluInvatamant()
                    //{
                    //    Denumire = (string)tipCiclu["Denumire"]
                    //});

                     context.spAdaugaTipCicluInvatamant((string)tipCiclu["Denumire"], responseMg, insertedId);
                }

                foreach (DataRow ps in dataSet.Tables["ProgramStudiu"].Rows)
                {
                    //context.ProgramStudius.Add(new ProgramStudiu()
                    //{
                    //    ID_TipCiclu = (int)ps["ID_TipCiclu"],
                    //    Facultate = (string)ps["Facultate"],
                    //    DenumireScurta = (string)ps["DenumireScurta"],
                    //    NumarAbsolventi = (int)ps["NumarAbsolventi"],
                    //    NumarVotanti = (int)ps["NumarVotanti"]
                    //});

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
                    //context.Profesors.Add(new Profesor()
                    //{
                    //    Nume = (string)prof["Nume"],
                    //    Prenume = (string)prof["Prenume"],
                    //    Email = (string)prof["Email"],
                    //    GradDidactic = (string)prof["GradDidactic"],
                    //    FacultateServiciu = (string)prof["FacultateServiciu"],
                    //    EligibilRemunerare = (bool)prof["EligibilRemunerare"]
                    //});

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
                    //context.RezultatVotProfesorProgramStudius.Add(new RezultatVotProfesorProgramStudiu()
                    //{
                    //    ID_ProgramStudiu = (int)rv["ID_ProgramStudiu"],
                    //    ID_Profesor = (int)rv["ID_Profesor"],
                    //    NumarVoturi = (short)rv["NumarVoturi"]
                    //});

                    context.spAdaugaRezultatVotProfesorProgramStudiu(
                        (int)rv["ID_ProgramStudiu"],
                        (int)rv["ID_Profesor"],
                        (short)rv["NumarVoturi"],
                        responseMg,
                        insertedId
                        );
                }

               // context.SaveChanges();
            }

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            msg += "InsertData : " + elapsedTime;

            dataSet.Dispose();
            return msg;
        }

        public static DataSet ReadData(DataSet dataSet)
        {
            int cnt = 0;
            Random rand = new Random();
            int voturi = 0;
            Dictionary<int, int> votanti = new Dictionary<int, int>();
            Dictionary<string, int> denumireFacultateCuId = new Dictionary<string, int>();
            Dictionary<string, int> denumirePSCuId = new Dictionary<string, int>();
            DataRow row;

            // **********TO DO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1 : xl => xlsx

            using (SLDocument sl = new SLDocument(GlobalValues.Path_FILE_INPUT))
            {
                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                int iStartColumnIndex = stats.StartColumnIndex, psStartColumnIndex = 0;
                String denumireFacultate = String.Empty, denumirePS = String.Empty;
                int numarAbsolventi = 0, numarVotanti = 0; // TO DO : receive "numarVotanti" from db *******************************!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Color psPatternForegroundColor;
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

                    if (String.IsNullOrEmpty(denumirePS))
                    {
                        continue;
                        // TO DO : ISSUE WARNING?
                    }

                    numarAbsolventi = sl.GetCellValueAsInt32(stats.StartRowIndex + 2, columnIndex);
                    numarVotanti = sl.GetCellValueAsInt32(stats.StartRowIndex + 3, columnIndex);
                    psPatternForegroundColor = sl.GetCellStyle(stats.StartRowIndex, columnIndex).Fill.PatternForegroundColor;          

                    // adauga programul de studiu

                    row = dataSet.Tables["ProgramStudiu"].NewRow();
                    row["DenumireScurta"] = denumirePS;                  
                    row["Facultate"] = denumireFacultate;
                    row["NumarAbsolventi"] = numarAbsolventi;
                    row["NumarVotanti"] = numarVotanti;

                    // TO DO : CODIFICARE SIGUR!!!!
                    if (psPatternForegroundColor.R == GlobalValues.PatternForegroundColor_MASTER_RGB.Item1 &&
                        psPatternForegroundColor.G == GlobalValues.PatternForegroundColor_MASTER_RGB.Item2 &&
                        psPatternForegroundColor.B == GlobalValues.PatternForegroundColor_MASTER_RGB.Item3) //PS : Licenta
                    {
                        row["ID_TipCiclu"] = GlobalValues.Map_TIP_CICLU_INVATAMANT_ID[GlobalValues.Denumire_TIP_CICLU_INVATAMANT_MASTER];
                    }
                    else   //PS : Master
                    {
                        row["ID_TipCiclu"] = GlobalValues.Map_TIP_CICLU_INVATAMANT_ID[GlobalValues.Denumire_TIP_CICLU_INVATAMANT_LICENTA];
                    }

                    cnt++;
                    dataSet.Tables["ProgramStudiu"].Rows.Add(row);
                    denumirePSCuId.Add(denumirePS, (int)row["ID_ProgramStudiu"]);
                    votanti.Add((int)row["ID_ProgramStudiu"], numarVotanti);
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
                    int vot1 = rand.Next(cnt + iStartColumnIndex + 6) + iStartColumnIndex + 6;
                    int vot2 = rand.Next(cnt + iStartColumnIndex + 6) + iStartColumnIndex + 6;
                    if (vot2 == vot1)
                    {
                        vot2++;
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
                        }

                        if (!sl.GetCellValueAsString(rowIndex, columnIndex).ToUpper().Equals("DA") && vot1 != columnIndex && vot2 != columnIndex)
                        {
                            continue;
                        }

                        idProfesor = (int)row["ID_Profesor"];
                        row = dataSet.Tables["RezultatVotProfesorProgramStudiu"].NewRow();
                        row["ID_Profesor"] = idProfesor;
                        row["ID_ProgramStudiu"] = denumirePSCuId[denumirePS];
                        voturi = rand.Next(votanti[denumirePSCuId[denumirePS]]);
                        votanti[denumirePSCuId[denumirePS]] -= voturi;
                        row["NumarVoturi"] = voturi;                  
                        dataSet.Tables["RezultatVotProfesorProgramStudiu"].Rows.Add(row);
                        //sl.SetCellValue(rowIndex, columnIndex, "DA");
                    }
                }


            }

            return dataSet;
        }

        

    }
}