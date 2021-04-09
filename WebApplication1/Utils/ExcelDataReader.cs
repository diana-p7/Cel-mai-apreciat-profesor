using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlTypes;
using ProfApreciat.Models;
using SpreadsheetLight;


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

            DataTable tableFacultate = new DataTable("Facultate");

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_Facultate";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            tableFacultate.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DenumireScurta";
            column.Unique = true;
            column.AllowDBNull = false;
            tableFacultate.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Denumire";
            tableFacultate.Columns.Add(column);

            PrimaryKeyColumns[0] = tableFacultate.Columns["ID_Facultate"];
            tableFacultate.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableFacultate);

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
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_Facultate";
            column.AllowDBNull = false;
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

            PrimaryKeyColumns[0] = tableProgramStudiu.Columns["ID_ProgramStudiu"];
            tableProgramStudiu.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableProgramStudiu);

            relation = new DataRelation("ProgramStudiu_Facultate", tableFacultate.Columns["ID_Facultate"], tableProgramStudiu.Columns["ID_Facultate"]);
            tableProgramStudiu.ParentRelations.Add(relation);

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
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ID_FacultateServiciu";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Email";
            column.Unique = true;
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Nume";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Prenume";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "GradDidactic";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Boolean");
            column.ColumnName = "EligibilRemunerare";
            column.AllowDBNull = false;
            tableProfesor.Columns.Add(column);

            PrimaryKeyColumns[0] = tableProfesor.Columns["ID_Profesor"];
            tableProfesor.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableProfesor);

            relation = new DataRelation("Profesor_Facultate", tableFacultate.Columns["ID_Facultate"], tableProfesor.Columns["ID_FacultateServiciu"]);
            tableProfesor.ParentRelations.Add(relation);

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

            PrimaryKeyColumns[0] = tableRezultatVotProfesorProgramStudiu.Columns["ID_Profesor"];
            tableRezultatVotProfesorProgramStudiu.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(tableRezultatVotProfesorProgramStudiu);

            relation = new DataRelation("EvaluareProfesorApreciat_Profesor", tableProfesor.Columns["ID_Profesor"], tableRezultatVotProfesorProgramStudiu.Columns["ID_Profesor"]);
            tableRezultatVotProfesorProgramStudiu.ParentRelations.Add(relation);

            relation = new DataRelation("EvaluareProfesorApreciat_ProgramStudiu", tableProgramStudiu.Columns["ID_ProgramStudiu"], tableRezultatVotProfesorProgramStudiu.Columns["ID_ProgramStudiu"]);
            tableRezultatVotProfesorProgramStudiu.ParentRelations.Add(relation);

            // insert in TipCicluInvatamant licenta si master

            row = dataSet.Tables["TipCicluInvatamant"].NewRow();
            row["Denumire"] = "Licenta";
            dataSet.Tables["TipCicluInvatamant"].Rows.Add(row);

            row = dataSet.Tables["TipCicluInvatamant"].NewRow();
            row["Denumire"] = "Master";
            dataSet.Tables["TipCicluInvatamant"].Rows.Add(row);

            return dataSet;
        }

        public static string InsertData()
        {
            String message = String.Empty;
            DataSet dataSet = makeDataSet();
            ReadData(dataSet);
            ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
            ObjectParameter insertedID = new ObjectParameter("insertedID", typeof(int));

            using (MyDNNDatabaseEntities1 context = new MyDNNDatabaseEntities1())
            {
                context.spClearDatabase();

                foreach (DataRow tipCiclu in dataSet.Tables["TipCicluInvatamant"].Rows)
                {
                    context.spAdaugaTipCicluInvatamant((string)tipCiclu["Denumire"], responseMessage, insertedID);
                }

                foreach (DataRow fac in dataSet.Tables["Facultate"].Rows)
                {
                    context.spAdaugaFacultate((string)fac["DenumireScurta"], null, responseMessage, insertedID);
                }

                foreach (DataRow ps in dataSet.Tables["ProgramStudiu"].Rows)
                {
                    context.spAdaugaProgramStudiu((int)ps["ID_Facultate"], (int)ps["ID_TipCiclu"], (string)ps["DenumireScurta"], null, responseMessage, insertedID);
                }

                foreach (DataRow prof in dataSet.Tables["Profesor"].Rows)
                {
                    context.spAdaugaProfesor((string)prof["Nume"], (string)prof["Prenume"], (string)prof["Email"], (string)prof["GradDidactic"], (int)prof["ID_FacultateServiciu"], (bool)prof["EligibilRemunerare"], responseMessage, insertedID);
                }

                foreach (DataRow rv in dataSet.Tables["RezultatVotProfesorProgramStudiu"].Rows)
                {
                    context.spAdaugaRezultatVotProfesorProgramStudiu((int)rv["ID_ProgramStudiu"], (int)rv["ID_Profesor"], (short)rv["NumarVoturi"], responseMessage, insertedID);
                }
            }

            return message;
        }

        public static DataSet ReadData(DataSet dataSet)
        {
            const int idLicenta = 1, idMaster = 2;
            Dictionary<string, int> denumireFacultateCuId = new Dictionary<string, int>();
            Dictionary<string, int> denumirePSCuId = new Dictionary<string, int>();
            DataRow row;

            using (SLDocument sl = new SLDocument(@"C:\Excel\0X_Cel_mai_apreciat_profesor_2021_univ_de lucru.xlsx", "nr_votanti"))
            {
                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                int iStartColumnIndex = stats.StartColumnIndex;
                String denumireFacultate = String.Empty, denumirePS = String.Empty;

                for (int columnIndex = iStartColumnIndex + 6; columnIndex <= stats.EndColumnIndex; columnIndex++)
                {
                    if (String.IsNullOrEmpty(sl.GetCellValueAsString(stats.StartRowIndex, columnIndex)))
                    {
                        break;
                    }

                    denumirePS = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);
                    denumireFacultate = sl.GetCellValueAsString(stats.StartRowIndex + 1, columnIndex);

                    // adauga facultatea daca e necesar
                    if (!denumireFacultateCuId.ContainsKey(denumireFacultate))
                    {
                        row = dataSet.Tables["Facultate"].NewRow();
                        row["DenumireScurta"] = denumireFacultate;
                        dataSet.Tables["Facultate"].Rows.Add(row);

                        denumireFacultateCuId.Add(denumireFacultate, (int)row["ID_Facultate"]);
                    }

                    // adauga programul de studiu

                    sl.GetCellStyle(stats.StartRowIndex, columnIndex).Fill.PatternBackgroundColor.ToArgb();
                    row = dataSet.Tables["ProgramStudiu"].NewRow();
                    row["DenumireScurta"] = denumirePS;
                    row["ID_Facultate"] = denumireFacultateCuId[denumireFacultate];

                    if (sl.GetCellStyle(stats.StartRowIndex, columnIndex).Fill.PatternForegroundColor.R == 5) //PS : Licenta
                    {
                        row["ID_TipCiclu"] = idLicenta;
                    }
                    else //PS : Master
                    {
                        row["ID_TipCiclu"] = idMaster;
                    }

                    dataSet.Tables["ProgramStudiu"].Rows.Add(row);
                    denumirePSCuId.Add(denumirePS, (int)row["ID_ProgramStudiu"]);
                }

                string nume = String.Empty, prenume = String.Empty, email = String.Empty, gradDidactic = String.Empty, facultateServiciu = String.Empty;
                int idProfesor;

                for (int rowIndex = stats.StartRowIndex + 4; rowIndex <= stats.EndRowIndex; ++rowIndex)
                {
                    nume = sl.GetCellValueAsString(rowIndex, iStartColumnIndex + 1);
                    prenume = sl.GetCellValueAsString(rowIndex, iStartColumnIndex + 2);
                    email = sl.GetCellValueAsString(rowIndex, iStartColumnIndex + 3);
                    gradDidactic = sl.GetCellValueAsString(rowIndex, iStartColumnIndex + 4);
                    facultateServiciu = sl.GetCellValueAsString(rowIndex, iStartColumnIndex + 5);

                    if (String.IsNullOrEmpty(nume) &&
                        String.IsNullOrEmpty(prenume) &&
                        String.IsNullOrEmpty(email) &&
                        String.IsNullOrEmpty(gradDidactic))
                    {
                        break;
                    }

                    row = dataSet.Tables["Profesor"].NewRow();
                    row["Nume"] = nume;
                    row["Prenume"] = prenume;
                    row["Email"] = email;
                    row["GradDidactic"] = gradDidactic;

                    if (gradDidactic.ToUpper().Equals("CE"))
                    {
                        row["EligibilRemunerare"] = false;
                    }
                    else
                    {
                        row["EligibilRemunerare"] = true;
                    }

                    row["ID_FacultateServiciu"] = denumireFacultateCuId[facultateServiciu]; // to analye

                    dataSet.Tables["Profesor"].Rows.Add(row);

                    //ADD REZULTATE VOT

                    for (int columnIndex = iStartColumnIndex + 6; columnIndex <= stats.EndColumnIndex; columnIndex++)
                    {
                        denumirePS = sl.GetCellValueAsString(stats.StartRowIndex, columnIndex);

                        if (String.IsNullOrEmpty(denumirePS))
                        {
                            break;
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